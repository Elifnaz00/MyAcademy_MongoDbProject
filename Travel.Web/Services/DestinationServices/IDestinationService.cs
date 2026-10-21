using Travel.Web.DTOs.BannerDtos;
using Travel.Web.DTOs.DestinationDtos;

namespace Travel.Web.Services.DestinationServices
{
    public interface IDestinationService
    {
        Task<List<ResultDestinationDto>> GetAllAsync();

        Task<List<ResultDestinationDto>> GetPopularDestinationsAsync();


       
    }
}
