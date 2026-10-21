using Travel.Web.DTOs.ReservationDtos;

namespace Travel.Web.Services.ReservationServices
{
    public interface IReservationService
    {
        Task CreateAsync(CreateReservationDto createReservationDto,string Id);

        Task<ResultReservationDto> GetByIdAsync(string id);

        Task<List<ResultReservationDto>> GetAllAsync();

        Task DeleteAsync(string id);


        int GetTotalReservations();

        Task<List<ResultReservationDto>> GetMyReservationsAsync(string userId);

        Task<long> GetTotalReservationsCountAsync();

        Task<List<ToursWithReservationDto>> GetPopularToursWithReservationAsync();

        Task<List<RecentBookingsDto>> GetRecentBookingsAsync();

        Task<List<TourParticipantReportDto>> GetTourParticipantReportAsync(string tourId);

        Task<List<MonthlyReservationStatsDto>> GetLastSixMonthsReservationStatsAsync();

        Task ConfirmReservationAsync(string id);
        Task CancelReservationAsync(string id);





    }
}
