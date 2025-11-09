using Booking.Data.Tables;
using Booking.Helper;
using Booking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Booking.Controllers
{
    [AllowAnonymous]
    public class WelcomeController : Controller
    {
        private readonly IAuthService _authService;
        public WelcomeController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            TempData["isNotLoggedIn"] = HttpContext.Session.Get(SessionKeys.User.LoggedInUserDetail) == null;

            if (TempData["isNotLoggedIn"] is false)
            {
                TempData[SessionKeys.User.LoggedInUserDetail] = SessionHelper.GetObjectFromSession<UserDto>(HttpContext.Session, SessionKeys.User.LoggedInUserDetail);
                ViewBag.LoggedInUser = $"{(TempData[SessionKeys.User.LoggedInUserDetail] as UserDto).Name}";
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
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
                TempData["success"] = "Login Successful";
                return RedirectToAction("Index", "Welcome");
            }
            TempData["error"] = "Login failed — incorrect credentials. Not registered yet? Sign up now and join us!";
            return RedirectToAction("Index", "Welcome");
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
