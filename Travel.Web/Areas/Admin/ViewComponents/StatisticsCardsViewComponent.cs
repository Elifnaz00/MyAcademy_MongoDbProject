using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.StatisticDto;
using Travel.Web.Services.QuestionServices;
using Travel.Web.Services.ReservationServices;
using Travel.Web.Services.TourServices;
using Travel.Web.Services.UserServices;

namespace Travel.Web.Areas.Admin.ViewComponents
{
    public class StatisticsCardsViewComponent : ViewComponent
    {
        private readonly ITourService _tourService;
        private readonly IReservationService _reservationService;
        private readonly IQuestionService _questionService;
        private readonly IUserService _userService;

        public StatisticsCardsViewComponent(ITourService tourService, IReservationService reservationService, IQuestionService questionService, IUserService userService)
        {
            _tourService = tourService;
            _reservationService = reservationService;
            _questionService = questionService;
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //toplam tur sayısı
            //aktif tur sayısı
            //Toplam rez s
            //popular tur sayııs
            //toplam kullanıcı s
            //yanıt bekleyen sorular sayısı


            var statistics= new StatisticDto
            {
                TotalTours = await _tourService.GetTotalCountToursAsync(),
                ActiveTours = await _tourService.GetActiveToursCountAsync(),
                TotalReservations = await _reservationService.GetTotalReservationsCountAsync(),
                PopularTours = await _tourService.GetPopularToursCountAsync(),
                TotalUsers = await _userService.GetTotalUsersCountAsync(),
                PendingQuestions = await _questionService.GetPendingQuestionsCountAsync()
            };
            


            return View(statistics);
        }
   
    }
}
