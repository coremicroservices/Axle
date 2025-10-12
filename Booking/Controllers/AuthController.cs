using Booking.Data;
using Booking.Data.Tables;
using Booking.Helper;
using Booking.Models;
using Booking.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;

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
            var result = await _authService.LoginUserAsync(login.Email, login.Password, cancellationToken);
            if (result != null)
            {
                HttpContext.Session.Set(SessionKeys.User.LoggedInUserDetail, Encoding.GetEncoding("utf-8").GetBytes(System.Text.Json.JsonSerializer.Serialize(result)));
                 
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Auth");
        }
        return RedirectToAction("Index", "Auth");
    }
 

    [HttpPost]
    public async Task<IActionResult> Register([Bind(Prefix = "Register")] RegisterViewModel register)
    {
        if (ModelState.IsValid)
        {
         var result =  await  _authService.RegisterUserAsync(new UserModel
            {
                Name = register.Name,
                Email = register.Email,
                PasswordHash = register.Password,
                Id = GuideHelper.GetGuide(),
                CreatedAt = DateTime.UtcNow
            });
            // Success: register contains valid data
            TempData["register"] = $"Registration successfully Done.. with uniqe Id {result}";
            return RedirectToAction("Index", "Auth");
        }

        // Rehydrate wrapper model for redisplay
        var model = new AuthViewModel { Register = register };
        return View("LoginRegister", model);
    }

}