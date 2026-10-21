using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Services.TourServices;

namespace Travel.Web.Areas.Admin.ViewComponents
{
    public class TourInformationViewComponent : ViewComponent
    {
        private readonly ITourService _tourService;

        public TourInformationViewComponent(ITourService tourService)
        {
          _tourService = tourService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var statisticsDto= new ResultTourStatisticsDto
            {
                ActiveTourCount = await _tourService.GetActiveToursCountAsync(),
                PassiveTourCount = await _tourService.GetPassiveToursCountAsync(),
                TotalTourCount = await _tourService.GetTotalCountToursAsync(),
                TotalQuota = await _tourService.GetTotalQuotaCountAsync()
            };
            return View(statisticsDto);

        }
    }
}
