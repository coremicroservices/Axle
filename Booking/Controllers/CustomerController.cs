using Booking.Data.Tables;
using Booking.Helper;
using Booking.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace Booking.Controllers
{
    public class CustomerController : CustomerBaseController
    {
        private readonly ICustomerService _customerService;
       
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MyBookings(CancellationToken cancellationToken = default)
        {
            
            var shipments = await _customerService.GetShipmentsAsync(userDto.Id, cancellationToken);
            return View(shipments);
        }

        //[HttpPost]
        //public async Task<IActionResult> Login(IFormCollection fc, CancellationToken cancellationToken = default)
        //{
        //    var result = await _authService.LoginUserAsync(fc["Username"], fc["Password"], cancellationToken);
        //    if (result != null)
        //    {
        //        HttpContext.Session.Set(SessionKeys.User.LoggedInUserDetail, Encoding.GetEncoding("utf-8").GetBytes(System.Text.Json.JsonSerializer.Serialize(result)));
        //        var claims = new List<Claim>
        //{
        //    new Claim(ClaimTypes.Name, result.Name),
        //    new Claim(ClaimTypes.Role, "Admin")
        //};

        //        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //        var authProperties = new AuthenticationProperties
        //        {
        //            IsPersistent = true, // for "Remember Me"
        //            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
        //        };

        //        await HttpContext.SignInAsync(
        //            CookieAuthenticationDefaults.AuthenticationScheme,
        //            new ClaimsPrincipal(claimsIdentity),
        //            authProperties
        //        );


        //        HttpContext.Session.Set(SessionKeys.User.LoggedInUserDetail, Encoding.GetEncoding("utf-8").GetBytes(System.Text.Json.JsonSerializer.Serialize(result)));

        //        TempData["success"] = "Login Successful";
        //        return RedirectToAction("Index", "Welcome");
        //    }
        //    TempData["error"] = "Login failed — incorrect credentials. Not registered yet? Sign up now and join us!";
        //    return RedirectToAction("Index", "Customer");
        //}


        public async Task<IActionResult> Logout(CancellationToken cancellationToken = default)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Welcome");
        }
    }
}
