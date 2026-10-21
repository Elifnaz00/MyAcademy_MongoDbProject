using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Travel.Web.DTOs.AuthDtos;
using Travel.Web.Entities;
using Travel.Web.Services.UserServices;

namespace Travel.Web.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if(!ModelState.IsValid)
            {
                return View(registerDto);
            }

            registerDto.RoleName = "user"; 
            await _userService.CreateUserAsync(registerDto);
            return RedirectToAction(nameof(Login));
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        


        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }
              
            var user = await _userService.GetUser(loginDto.UserName, loginDto.Password);
            if (user is null)
             {
                ViewBag.Message = "Geçersiz hesap bilgisi!";
                return View(loginDto);
             }
           

            var claims = new List<Claim>() {
                   new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Id)),
                   new Claim(ClaimTypes.Name, user.UserName),
                   new Claim(ClaimTypes.Role, user.RoleName),
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync("cookie", principal, new AuthenticationProperties());

            if (user.RoleName == "admin")
            {
                return RedirectToAction("Index", "Tour", new { area = "Admin" });
            }
            return RedirectToAction("Index", "Home");
        }
            
        



        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("cookie");
            return RedirectToAction("Login", "Auth");
        }



    }

}

