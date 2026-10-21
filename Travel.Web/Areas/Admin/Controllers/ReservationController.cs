using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.ReservationServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var reservations= await _reservationService.GetAllAsync();
            return View(reservations);
        }

     
        public async Task<IActionResult> ConfirmReservation(string id)
        {
            await _reservationService.ConfirmReservationAsync(id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CancelReservation(string id)
        {
            await _reservationService.CancelReservationAsync(id);
            return RedirectToAction(nameof(Index));
        }


    }
}
