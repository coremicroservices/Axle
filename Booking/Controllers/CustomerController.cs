using Booking.Helper;
using Booking.Services;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
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
                TempData["success"] =   "Login Successful";
                return RedirectToAction("Index", "Welcome");
            }
            return RedirectToAction("Index", "Welcome");
        }
       
    }
}
