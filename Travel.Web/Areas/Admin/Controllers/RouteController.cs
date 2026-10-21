using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.RouteDtos;
using Travel.Web.Services.RouteService;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RouteController(IRouteService _routeService, IMapper _mapper) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var routes = await _routeService.GetAllRoutesAsync();
            return View(routes);
        }


        [HttpPost]
        public async Task<IActionResult> Index(string city)
        {
            var routes = await _routeService.GetAllRoutesAsync();
            if(string.IsNullOrEmpty(city))
            {
                return View(routes);
            }
            var routesbyCity = await _routeService.GetAllByCityAsynyc(city);
            return View(routesbyCity);
        }


      

       
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(CreateRouteDto createRouteDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createRouteDto);
            }
            await _routeService.CreateRouteAsync(createRouteDto);
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var route = await _routeService.GetRouteByIdAsync(id);
            var updateRouteDto = _mapper.Map<UpdateRouteDto>(route);
            return View(updateRouteDto);
        }



        [HttpGet]
        public async Task<IActionResult> Update(UpdateRouteDto updateRouteDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateRouteDto);
            }
            await _routeService.UpdateRouteAsync(updateRouteDto);
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Delete(string id)
        {
            await _routeService.DeleteRouteAsync(id);
            return RedirectToAction(nameof(Index));

        }

     
    }
}
