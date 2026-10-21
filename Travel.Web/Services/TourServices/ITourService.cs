using MongoDB.Bson;
using Travel.Web.DTOs.ReservationDtos;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Entities;

namespace Travel.Web.Services.TourServices
{
    public interface ITourService
    {
        Task<List<ResultTourDto>> GetAllAsync();

        Task CreateAsync(CreateTourDto createTourDto);

        Task<ResultTourDto> GetTourByIdAsync(string tourId);

        Task<List<ResultPopularTourDto>> GetPopularToursAsync();
        Task ChangeStatusAsync(ResultTourDto resultTourDto);

        Task<List<ResultTourByCategoryDto>> GetTourByCategoryAsync();

        Task<bool> DeleteAsync(string id);

        Task UpdateAsync(UpdateTourDto updateTourDto);

        Task<long> GetTotalCountToursAsync();

        Task<long> GetActiveToursCountAsync();

        Task<long> GetPassiveToursCountAsync();
        Task<int> GetTotalQuotaCountAsync(); 
        Task<List<ResultTourDto>> GetFilteredToursAsync(
    TourFilterDto filter);

        Task<long> GetPopularToursCountAsync(); 

       






    }
}
