using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MongoDB.Bson;
using System.Security.Claims;
using Travel.Web.DTOs.CommentDtos;
using Travel.Web.DTOs.QıuestionDtos;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Services.CommentServices;
using Travel.Web.Services.QuestionServices;
using Travel.Web.Services.TourServices;
using Travel.Web.Services.UserServices;

namespace Travel.Web.Controllers
{
    [AllowAnonymous]
    public class TourController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly ITourService _tourService;
        private readonly IUserService _userService;
        private readonly IQuestionService _questionService;

        public TourController(IQuestionService questionService, ICommentService commentService, ITourService tourService, IUserService userService)
        {
            _commentService = commentService;
            _tourService = tourService;
            _userService = userService;
            _questionService = questionService;
        }


        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] TourFilterDto filter)
        {
            var tours = await _tourService.GetAllAsync();

           
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.Trim();

                tours = tours.Where(t =>
                    (t.Name ?? "").Contains(
                        search, StringComparison.CurrentCultureIgnoreCase) ||
                    (t.StartingCity ?? "").Contains(
                        search, StringComparison.CurrentCultureIgnoreCase) ||
                    (t.TourRoute ?? "").Contains(
                        search, StringComparison.CurrentCultureIgnoreCase)
                ).ToList();
            }

            
            if (filter.Destinations.Count > 0)
            {
                tours = tours.Where(t =>
                    filter.Destinations.Any(country =>
                        string.Equals(
                            t.DestinationCountry?.Trim(),
                            country.Trim(),
                            StringComparison.CurrentCultureIgnoreCase
                        )
                    )
                ).ToList();
            }

           
            if (filter.Durations.Count > 0)
            {
                tours = tours.Where(t =>
                {
                    if (!int.TryParse(t.DayTimeDuration, out int duration))
                        return false;

                    return filter.Durations.Any(key => key switch
                    {
                        "1-3" => duration >= 1 && duration <= 3,
                        "4-5" => duration >= 4 && duration <= 5,
                        "6-7" => duration >= 6 && duration <= 7,
                        "8+" => duration >= 8,
                        _ => false
                    });
                }).ToList();
            }

            
            if (filter.Prices.Count > 0)
            {
                tours = tours.Where(t =>
                    filter.Prices.Any(key => key switch
                    {
                        "p1" => t.Price >= 0 && t.Price < 25000,
                        "p2" => t.Price >= 25000 && t.Price < 40000,
                        "p3" => t.Price >= 40000 && t.Price < 60000,
                        "p4" => t.Price >= 60000,
                        _ => false
                    })
                ).ToList();
            }

            
            if (filter.Types.Count > 0)
            {
                tours = tours.Where(t =>
                    filter.Types.Any(type =>
                        string.Equals(
                            t.TourType?.Trim(),
                            type.Trim(),
                            StringComparison.CurrentCultureIgnoreCase
                        )
                    )
                ).ToList();
            }

            ViewBag.Filter = filter;

            return View(tours);
        }

        public async Task<IActionResult> Details(string id)
        {
            var tour= await _tourService.GetTourByIdAsync(id);
            if(tour is null)
            {
                return NotFound();
            }
            var comments = await _commentService.GetCommentsByTourIdAsync(id);
            var questions = await _questionService.GetAllQuestionsByTourAsync(id);
            var tourDetailDto= new TourDetailDto
            {
                Tour = tour,
                Comments = comments,
                Questions = questions
            };  

            return View(tourDetailDto);
        }


        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentDto createCommentDto)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", "Tour", new { id = createCommentDto.TourId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            await _commentService.CreateAsync(createCommentDto, userId);

            return RedirectToAction("Details", "Tour", new { id = createCommentDto.TourId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion(CreateQuestionDto createQuestionDto)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", "Tour", new { id = createQuestionDto.TourId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            await _questionService.CreateQuestionAsync(createQuestionDto, userId);

            return RedirectToAction("Details", "Tour", new { id = createQuestionDto.TourId });
        }
    }
}
