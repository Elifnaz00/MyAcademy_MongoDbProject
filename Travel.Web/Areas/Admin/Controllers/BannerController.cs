using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Travel.Web.DTOs.BannerDtos;
using Travel.Web.Services.BannerServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class BannerController(IBannerService _bannerService, IMapper mapper) : Controller
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var banners = await _bannerService.GetAllAsync();
            return View(banners);
        }

       
        public async Task<ActionResult> Details(int id)
        {
            
            return View();
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        public async Task<ActionResult> Create(CreateBannerDto createBannerDto)
        {
           if(!ModelState.IsValid)
           {
                return View(createBannerDto);
            }
            await _bannerService.CreateAsync(createBannerDto);
           return RedirectToAction(nameof(Index));
           
        }

        
        public async Task<ActionResult> Update(string id)
        {
            var banner= await _bannerService.GetByIdAsync(id); 
            var mappedBanner= mapper.Map<UpdateBannerDto>(banner);
            return View(mappedBanner);
        }

        
        [HttpPost]
        public async Task<ActionResult> Update(UpdateBannerDto updateBannerDto)
        {
            if(!ModelState.IsValid)
            {
                return View(updateBannerDto);
            }

            await _bannerService.UpdateAsync(updateBannerDto);
            return RedirectToAction(nameof(Index));
            
            
            
        }

        
        public async Task<ActionResult> Delete(string id)
        {
            await _bannerService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
           
        }

    }
}
