using Booking.Data.Tables;
using Booking.FCMNotification;
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
    public class SupplierController : Controller
    {
        private readonly ILogger<SupplierController> _logger;
        private readonly ISupplierService _supplierService;
        private readonly IFCMNotification _fCMNotification;
        public SupplierController(ISupplierService supplierService, IFCMNotification fCMNotification)
        {
            _supplierService = supplierService;
            _fCMNotification = fCMNotification;
        }
        public async Task<IActionResult> Dashboard(CancellationToken cancellationToken = default)
        {
            var supplier = SessionHelper.GetObjectFromSession<SupplierOnboardingDto>(HttpContext.Session, SessionKeys.Supplier.LoggedInSupplierName);
            if (supplier is null)
            {
                return RedirectToAction("Index", "Supplier");
            }
            ViewBag.supplierName = SessionHelper.GetObjectFromSession<SupplierOnboardingDto>(HttpContext.Session, SessionKeys.Supplier.LoggedInSupplierName).OwnerName ?? null;
            ViewBag.IncomingBookingCount = await _supplierService.IncomingBookingCount(supplier.Id, cancellationToken);
            return View();
        }

        public IActionResult Index()
        {
            return View(new SupplierLoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(SupplierLoginViewModel supplierLoginViewModel, CancellationToken cancellationToken = default)
        {
            var result = await _supplierService.IsValidSupplierAsync(supplierLoginViewModel.Contact, supplierLoginViewModel.Password, cancellationToken);
            if (result is not null)
            {
                HttpContext.Session.Set(SessionKeys.Supplier.LoggedInSupplierName, Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(result)));
                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, result.Id.ToString()), // Ensure Id is a string
                            new Claim("role", "supplier")
                        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                FcmDeviceToken fcmDeviceToken = new FcmDeviceToken
                {
                    UserId = result.Id,
                    DeviceToken = await _fCMNotification.GetAccessTokenAsync(),
                    Platform = GetPlatform()
                };  

                await _supplierService.AddDeviceTokenAsync(fcmDeviceToken, cancellationToken); 

                return RedirectToAction("Dashboard", "Supplier");
            }
            return RedirectToAction("Index", "Supplier");
        }

        public string GetPlatform() {
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var platform = "Web"; // Default to web

            if (userAgent.Contains("Android"))
            {
                platform = "Android";
            }
            else if (userAgent.Contains("iPhone") || userAgent.Contains("iPad") || userAgent.Contains("iOS"))
            {
                platform = "iOS";
            }
            return platform;
        }
    }
}
