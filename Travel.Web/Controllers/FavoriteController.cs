using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Travel.Web.DTOs.FavoriteDto;
using Travel.Web.Services.FavoriteServices;

namespace Travel.Web.Controllers
{

    public class FavoriteController : Controller
    {
       private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

      
     
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AddFavoriteDto addFavoriteDto)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserId is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            addFavoriteDto.UserId = UserId;
            await _favoriteService.AddFavoriteTourAsync(addFavoriteDto);

            return RedirectToAction("Index","Home");
           
               
            
        }

       
      
     
    }
}
