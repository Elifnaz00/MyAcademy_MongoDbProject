using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Travel.Web.Services.CommentServices;

namespace Travel.Web.Areas.User.Controllers
{
    [Area("User")]
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task<IActionResult> Index()
        {
            var userId=  User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Auth");
            }
            var comments = await _commentService.GetCommentsByUserIdAsync(userId);
            return View(comments);
        }
    }
}
