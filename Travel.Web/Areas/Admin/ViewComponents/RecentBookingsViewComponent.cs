using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.ReservationServices;

namespace Travel.Web.Areas.Admin.ViewComponents
{
    public class RecentBookingsViewComponent : ViewComponent
    {
        private readonly IReservationService _reservationService;

        public RecentBookingsViewComponent(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var recentBookings= await _reservationService.GetRecentBookingsAsync();
            return View(recentBookings);
        }
    }
}
