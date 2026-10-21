using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Travel.Web.DTOs.ReservationDtos;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.Enums;
using Travel.Web.Services.ReservationServices;
using Travel.Web.Settings;

namespace Travel.Web.Services.ReservationServices
{
    public class ReservationService : IReservationService
    {
        private readonly IMongoCollection<Reservation> _reservationCollection;
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly IMapper _mapper;
        private readonly IMongoCollection<User> _userCollection;


        public ReservationService(IDatabaseSettings databaseSettings, IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _userCollection = database.GetCollection<User>(
    databaseSettings.UserCollectionName);
            _reservationCollection = database.GetCollection<Reservation>(databaseSettings.ReservationCollectionName);
            _tourCollection = database.GetCollection<Tour>(databaseSettings.TourCollectionName);
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateReservationDto createReservationDto, string Id)
        {
            var reservation = _mapper.Map<Reservation>(createReservationDto);
            reservation.UserId = Id;
            await _reservationCollection.InsertOneAsync(reservation);
        }

      
        public async Task DeleteAsync(string id)
        {

            await _reservationCollection.DeleteOneAsync(b => b.Id == id);
        }

      
        public async Task<List<ResultReservationDto>> GetAllAsync()
        {
            var reservationList = await _reservationCollection
                .Find(x => true)
                .ToListAsync();

            var tourList = await _tourCollection
                .Find(x => true)
                .ToListAsync();

            var result = reservationList.Select(reservation =>
            {
                var tour = tourList.FirstOrDefault(x => x.Id == reservation.TourId);

                return new ResultReservationDto
                {
                    Id = reservation.Id,
                    Child = reservation.Child,
                    Adult = reservation.Adult,
                    TotalPrice = reservation.TotalPrice,
                    ReservationCreatedDate = reservation.ReservationCreatedDate,
                    StartDate = reservation.StartDate,
                    EndDate = reservation.EndDate,
                    TourId = reservation.TourId,
                    ReservationStatus = reservation.ReservationStatus,

                    TourName = tour?.Name,
                    TourImageUrl = tour?.ImageUrl,
                    TourType = tour?.TourType
                };
            }).ToList();

            return result;
        
        }


        public async Task<ResultReservationDto> GetByIdAsync(string id)
        {
            var Reservation = await _reservationCollection.Find(b => b.Id == id).FirstOrDefaultAsync();
            var mappedReservation = _mapper.Map<ResultReservationDto>(Reservation);
            return mappedReservation;

        }

        public async Task<List<ResultReservationDto>> GetMyReservationsAsync(string userId)
        {
            var reservations = await _reservationCollection
      .Find(x => x.UserId == userId)
      .ToListAsync();

            var tours = await _tourCollection
                .Find(x => true)
                .ToListAsync();

            return reservations.Select(reservation =>
            {
                var tour = tours.FirstOrDefault(
                    x => x.Id == reservation.TourId);

                return new ResultReservationDto
                {
                    Id = reservation.Id,
                    Child = reservation.Child,
                    Adult = reservation.Adult,
                    TotalPrice = reservation.TotalPrice,
                    ReservationCreatedDate = reservation.ReservationCreatedDate,
                    StartDate = reservation.StartDate,
                    EndDate = reservation.EndDate,
                    TourId = reservation.TourId,
                    ReservationStatus = reservation.ReservationStatus,
                    TourName = tour?.Name,
                    TourImageUrl = tour?.ImageUrl,
                    TourType = tour?.TourType
                };
            }).ToList();
        }

        public int GetTotalReservations()
        {
            var count= _reservationCollection.AsQueryable().Count();
            return count;
        }

        public async Task<long> GetTotalReservationsCountAsync()
        {
            return await _reservationCollection.CountDocumentsAsync(_ => true);
        }

        public async Task<List<ToursWithReservationDto>> GetPopularToursWithReservationAsync()
        {
            var popularTours = await _reservationCollection.Find(x => x.ReservationStatus == ReservationStatus.Onaylandı).ToListAsync();
            

            var groupedTours= popularTours.GroupBy(r => r.TourId).Select(g => new ToursWithReservationDto
            {

                ReservationCount = g.Count(),
                TourId = g.Key
            }).Take(5).OrderByDescending(s=>s.ReservationCount);

            var result = new List<ToursWithReservationDto>();
            foreach(var item in groupedTours)
            {
                var tour= await _tourCollection.Find(t=> t.Id == item.TourId).FirstOrDefaultAsync();

                if (tour != null)
                {
                    result.Add(new ToursWithReservationDto
                    {
                        TourName = tour.Name,
                        ReservationCount = item.ReservationCount,
                        ImageUrl= tour.ImageUrl
                    });
                }
            }

            return result;
        }

        public async Task<List<RecentBookingsDto>> GetRecentBookingsAsync()
        {
            var recentBookings = await _reservationCollection
                               .AsQueryable()
                               .OrderByDescending(x => x.ReservationCreatedDate)
                               .Take(4)
                               .ToListAsync();
            return _mapper.Map<List<RecentBookingsDto>>(recentBookings);
        }


        public async Task<List<TourParticipantReportDto>> GetTourParticipantReportAsync(
            string tourId)
        {
           
            var reservations = await _reservationCollection
                .Find(r => r.TourId == tourId)
                .ToListAsync();

            
            var tour = await _tourCollection
                .Find(t => t.Id == tourId)
                .FirstOrDefaultAsync();

            if (tour == null)
            {
                return new List<TourParticipantReportDto>();
            }

           
            var users = await _userCollection
                .Find(_ => true)
                .ToListAsync();

            
            var report = reservations.Select(reservation =>
            {
                var user = users.FirstOrDefault(
                    u => u.Id == reservation.UserId);

                return new TourParticipantReportDto
                {
                    FullName = user == null
                        ? "Kullanıcı bulunamadı"
                        : $"{user.UserName} {user.UserSurName}".Trim(),

                    Email = user?.UserEmail ?? string.Empty,

                    Telephone = user?.Telephone ?? string.Empty,

                    TourName = tour.Name ?? string.Empty,

                    StartDate = reservation.StartDate,

                    EndDate = reservation.EndDate,

                    Adult = reservation.Adult,

                    Child = reservation.Child,

                    ReservationCreatedDate =
                        reservation.ReservationCreatedDate,

                    TotalPrice = reservation.TotalPrice,

                    Status = reservation.ReservationStatus.ToString()
                };
            }).ToList();

            return report;
        }

        public async Task<List<MonthlyReservationStatsDto>> GetLastSixMonthsReservationStatsAsync()
        {
            var nowDate= DateTime.Now;
            var startDate = new DateTime(nowDate.Year, nowDate.Month, 1).AddMonths(-5);
            var endDate = startDate.AddMonths(6);
            var reservations = await _reservationCollection
       .Find(x => x.ReservationCreatedDate >= startDate
               && x.ReservationCreatedDate < endDate)
       .ToListAsync();

            var result = new List<MonthlyReservationStatsDto>();

            for (int i = 0; i < 6; i++)
            {
                var monthDate = startDate.AddMonths(i);

                var count = reservations.Count(x =>
                    x.ReservationCreatedDate.Year == monthDate.Year &&
                    x.ReservationCreatedDate.Month == monthDate.Month);

                result.Add(new MonthlyReservationStatsDto
                {
                    Month = monthDate.ToString(
                        "MMMM",
                        new System.Globalization.CultureInfo("tr-TR")),

                    ReservationCount = count
                });
            }

            return result;
        }

        public async Task ConfirmReservationAsync(string id)
        {
            var filter = Builders<Reservation>.Filter.Eq(g => g.Id, id);
            var update = Builders<Reservation>.Update
                        .Set(g => g.ReservationStatus, ReservationStatus.Onaylandı);

            await _reservationCollection.UpdateOneAsync(filter, update);
        }

        public async Task CancelReservationAsync(string id)
        {
            var filter = Builders<Reservation>.Filter.Eq(g => g.Id, id);
            var update = Builders<Reservation>.Update
                        .Set(g => g.ReservationStatus, ReservationStatus.İptal);

            await _reservationCollection.UpdateOneAsync(filter, update);


        }
    }
}
