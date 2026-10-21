using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Security.Claims;
using Travel.Web.DTOs.ReservationDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.Enums;
using Travel.Web.Services.ReservationServices;
using Travel.Web.Services.TourServices;


namespace Travel.Web.Controllers
{
    [AllowAnonymous]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly ITourService _tourService;

        public ReservationController(IReservationService reservationService, ITourService tourService)
        {
            _reservationService = reservationService;
            _tourService = tourService;
        }

        public async Task<IActionResult> Create([FromBody]CreateReservationDto createReservationDto)
        {
     
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }


            if (createReservationDto.Adult < 1)
            {
                return BadRequest(new
                {
                    message = "En az 1 yetişkin bulunmalıdır."
                });
            }

            if (createReservationDto.Child < 0)
            {
                return BadRequest(new
                {
                    message = "Çocuk sayısı geçersiz."
                });
            }

            var tour= await  _tourService.GetTourByIdAsync(createReservationDto.TourId);
            if (tour is null)
            {
                return NotFound("Tur bulunamadı.");
            }

            Console.WriteLine($"Tur fiyatı: {tour.Price}");

            var totalAdult = createReservationDto.Adult * tour.Price;
            var totalChild = 0.5M * createReservationDto.Child * tour.Price;
            var totalPrice = totalAdult + totalChild;

            Console.WriteLine($"Hesaplanan toplam: {totalPrice}");

            var tourDate =  tour.TourDates.FirstOrDefault(x=> x.StartDate.Date ==  createReservationDto.StartDate && x.EndDate.Date== createReservationDto.EndDate);

            if (tourDate is null)
            {
                return BadRequest("Seçilen tur tarihi bulunamadı.");
            }

            var totalPerson = createReservationDto.Adult + createReservationDto.Child;
            if(totalPerson > tourDate.Quota)
            {
                return BadRequest("Seçilen tarih için yeterli kontenjan bulunmamaktadır.");
            }

           


            var reservation = new CreateReservationDto
            {
                TotalPrice = totalPrice,
                Adult = createReservationDto.Adult,
                Child = createReservationDto.Child,
                EndDate = createReservationDto.EndDate,
                StartDate = createReservationDto.StartDate,
                ReservationCreatedDate = DateTime.Now,
                TourId= tour.Id,
               
                ReservationStatus = ReservationStatus.Bekliyor
            
            };
           
            await _reservationService.CreateAsync(reservation, userId);
            return RedirectToAction("Index","Tour");
        }


    }
}
