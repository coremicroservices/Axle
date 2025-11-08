using Booking.Helper;
using Booking.Models;
using Booking.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace TruckBookingApp.Api.Controllers;
 
public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService  ;
    }
    public IActionResult Index()
    { 

        return View(new AuthViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login([Bind(Prefix = "login")] LoginViewModel login, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var result = await _authService.LoginUserAsync(login.UserName, login.Password, cancellationToken);
            if (result != null)
            {
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
                TempData["success"] = "Success";
                return RedirectToAction("Index", "Welcome");
            }
            TempData["error"] = "Invalid username or password";
            return RedirectToAction("Index", "Auth");
        }
        return RedirectToAction("Index", "Auth");
    }  
}