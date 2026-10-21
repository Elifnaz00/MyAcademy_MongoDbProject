using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Travel.Web.Services.ReservationServices;

namespace Travel.Web.Areas.User.Controllers
{
    [Area("User")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public async Task<IActionResult> Index()
        {
            var userId= User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Forbid();
            }
            var reservations =
               await _reservationService.GetMyReservationsAsync(userId);

            return View(reservations);
        }
    }
}
