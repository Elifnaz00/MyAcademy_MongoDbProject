using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Models;
using Travel.Web.Models.HomeModels;
using Travel.Web.Services.DestinationServices;
using Travel.Web.Services.TourServices;

namespace Travel.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
       
        private readonly IDestinationService _destinationService;
        private readonly ITourService _tourService;

        public HomeController(IDestinationService destinationService, ITourService tourService)
        {
            _destinationService = destinationService;
            _tourService = tourService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var popularTours=  await _tourService.GetPopularToursAsync();

            var allDestinations= await _destinationService.GetAllAsync();

            var destinationLookup = allDestinations
        .ToDictionary(d => d.Id, d => d);


            var model = new HomeIndexViewModel
            {
                PopularTours = popularTours,
                DestinationLookup = destinationLookup
            };

            return View(model);
        }
    }
}
