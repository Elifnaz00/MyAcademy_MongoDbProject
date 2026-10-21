using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Travel.Web.Services.FavoriteServices;

namespace Travel.Web.Areas.User.Controllers
{
    [Area("User")]
    public class FavoriteToursController : Controller
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteToursController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        public async Task<IActionResult> Index()
        {
            var userId= User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return RedirectToAction("Login", "Auth");
            }
             var favoriteTours= await _favoriteService.GetFavoriteToursByUserIdAsync(userId);

            return View(favoriteTours);
        }
    }
}
