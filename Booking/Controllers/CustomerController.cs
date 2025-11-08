using Booking.Data.Tables;
using Booking.Helper;
using Booking.Models;
using Booking.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace Booking.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IAuthService _authService;
        public CustomerController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(IFormCollection fc, CancellationToken cancellationToken = default)
        {
            var result = await _authService.LoginUserAsync(fc["Username"], fc["Password"], cancellationToken);
            if (result != null)
            {
                HttpContext.Session.Set(SessionKeys.User.LoggedInUserDetail, Encoding.GetEncoding("utf-8").GetBytes(System.Text.Json.JsonSerializer.Serialize(result)));
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, result.Name),
            new Claim(ClaimTypes.Role, "Admin")
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // for "Remember Me"
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );


                HttpContext.Session.Set(SessionKeys.User.LoggedInUserDetail, Encoding.GetEncoding("utf-8").GetBytes(System.Text.Json.JsonSerializer.Serialize(result)));

                TempData["success"] =   "Login Successful";
                return RedirectToAction("Index", "Welcome");
            }
            TempData["error"] = "Login failed — incorrect credentials. Not registered yet? Sign up now and join us!";
            return RedirectToAction("Index", "Customer");
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Register(IFormCollection formcollecion)
        {
            string mobileNumber = formcollecion["MobileNumber"];
            string name = formcollecion["Name"];
            string email = formcollecion["Email"];
            string password = formcollecion["Password"];

            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterUserAsync(new UserModel
                {
                    Name = name,
                    Email = email,
                    PasswordHash = password,
                    Id = GuideHelper.GetGuide(),
                    CreatedAt = DateTime.UtcNow,
                    FcmDeviceTokenId = string.Empty,
                    Contactnumber = mobileNumber
                });
                // Success: register contains valid data
                TempData["register"] = $"Registration successfully Done";
                return RedirectToAction("Index", "Welcome");
            }

            // Rehydrate wrapper model for redisplay
            TempData["error"] = "Oops , Something went wrong";
            return RedirectToAction("Index", "Welcome");
        }

    }
}
