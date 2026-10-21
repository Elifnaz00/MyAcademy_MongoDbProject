using Travel.Web.DTOs.RouteDtos;

namespace Travel.Web.Services.RouteService
{
    public interface IRouteService
    {
        Task<List<ResultRouteDto>> GetAllRoutesAsync();

        Task<ResultRouteDto> GetRouteByIdAsync(string id);

        Task CreateRouteAsync(CreateRouteDto createRouteDto);

        Task UpdateRouteAsync(UpdateRouteDto updateRouteDto);

        Task DeleteRouteAsync(string id);

        Task<List<ResultRouteDto>> GetAllByCityAsynyc(string city);
    }
}
