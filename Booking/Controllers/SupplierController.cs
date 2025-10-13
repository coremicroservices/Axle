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
        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        public async Task<IActionResult> Dashboard(CancellationToken cancellationToken = default)
        {
            var supplier = SessionHelper.GetObjectFromSession<SupplierModel>(HttpContext.Session, SessionKeys.Supplier.LoggedInSupplierName);
            if (supplier is null)
            {
                return RedirectToAction("Index", "Supplier");
            }
            ViewBag.supplierName = SessionHelper.GetObjectFromSession<SupplierModel>(HttpContext.Session, SessionKeys.Supplier.LoggedInSupplierName).SupplierName ?? null;
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
                            new Claim(ClaimTypes.Name, result.Id), // This becomes Identity.Name
                            new Claim("role", "supplier")
                        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


                return RedirectToAction("Dashboard", "Supplier");
            }
            return RedirectToAction("Index", "Supplier");
        }
    }
}
