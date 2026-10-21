using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.Controllers
{
    public class LanguageController : Controller
    {
      
            public IActionResult SetLanguage(string culture, string returnUrl)
        {
            var supportedCultures = new[] { "tr-TR", "en-US" };

            if (!supportedCultures.Contains(culture))
            {
                culture = "tr-TR";
            }

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(
                    new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax
                });

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}

