using Travel.Web.DTOs.FavoriteDto;

namespace Travel.Web.Services.FavoriteServices
{
    public interface IFavoriteService
    {
        Task AddFavoriteTourAsync(AddFavoriteDto addFavoriteDto);

        Task<List<ResultFavoriteDto>> GetFavoriteToursByUserIdAsync(string userId);
    }
}
