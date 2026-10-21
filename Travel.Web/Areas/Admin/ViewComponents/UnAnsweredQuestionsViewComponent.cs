using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.Areas.Admin.ViewComponents
{
    public class UnAnsweredQuestionsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
