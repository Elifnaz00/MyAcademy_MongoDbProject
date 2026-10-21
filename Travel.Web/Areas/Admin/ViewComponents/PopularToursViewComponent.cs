using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.ReservationServices;
using Travel.Web.Services.TourServices;

namespace Travel.Web.Areas.Admin.ViewComponents
{
    public class PopularToursViewComponent : ViewComponent
    {
        private readonly IReservationService _reservationService;

        public PopularToursViewComponent(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var popularTours =
               await _reservationService
                   .GetPopularToursWithReservationAsync();

            ViewBag.ReservationChartData =
                await _reservationService
                    .GetLastSixMonthsReservationStatsAsync();

            return View(popularTours);
        }
    
    }
}
