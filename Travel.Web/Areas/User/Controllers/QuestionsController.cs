using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Travel.Web.Services.QuestionServices;

namespace Travel.Web.Areas.User.Controllers
{
    [Area("User")]
    public class QuestionsController : Controller
    {
        private readonly IQuestionService _questionService;

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        public async Task <IActionResult> Index()
        {
            var userId= User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Auth");
            }
            var questions = await _questionService.GetAllQuestionsByUserIdAsync(userId);
            return View(questions);
        }
    }
}
