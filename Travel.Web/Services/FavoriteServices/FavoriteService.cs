using AutoMapper;
using MongoDB.Driver;
using Travel.Web.DTOs.FavoriteDto;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.FavoriteServices
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IMongoCollection<Favorite> _favoriteCollection;
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly IMapper _mapper;

        public FavoriteService(IDatabaseSettings databaseSettings, IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _favoriteCollection = database.GetCollection<Favorite>(databaseSettings.FavoriteCollectionName);
            _tourCollection = database.GetCollection<Tour>(databaseSettings.TourCollectionName);
            _mapper = mapper;
        }

        public async Task AddFavoriteTourAsync(AddFavoriteDto addFavoriteDto)
        {
           
            var mappedFavorite= _mapper.Map<Favorite>(addFavoriteDto);
            await _favoriteCollection.InsertOneAsync(mappedFavorite);
        }

        public async Task<List<ResultFavoriteDto>> GetFavoriteToursByUserIdAsync(string userId)
        {
            var favorites= await _favoriteCollection.Find(f => f.UserId == userId).ToListAsync();
            var tours= await _tourCollection.Find(_=>true).ToListAsync();   

            return favorites.Select(item=>
            {
                var tour = tours.FirstOrDefault(t => t.Id == item.TourId);

                return new ResultFavoriteDto
                {
                    TourName = tour?.Name,
                    CreatedAt = item.CreatedAt,
                    ShortImportantDescription = tour?.ShortImportantDescription,
                    Price = tour?.Price ?? 0,
                    ImageUrl = tour?.ImageUrl
                };
            }).OrderByDescending(x => x.CreatedAt).ToList();

        }
    }
}
