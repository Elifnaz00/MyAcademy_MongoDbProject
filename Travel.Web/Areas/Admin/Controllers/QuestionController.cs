using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.QıuestionDtos;
using Travel.Web.Services.QuestionServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var questions = await _questionService.GetUnAnswerdQuestionsAsync();
            return View(questions);
        }

        [HttpGet]
        public async Task<IActionResult> Reply(string id)
        {
            var question= await _questionService.GetByIdQuestionAsync(id);
            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> Reply(ReplyQuestionDto replyQuestionDto)
        {
            if (!ModelState.IsValid) { 
                return View(replyQuestionDto);
            }
            await _questionService.ReplyQuestionAsync(replyQuestionDto);
            return RedirectToAction(nameof(Index));
        }
    }
}
