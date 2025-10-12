using Booking.Helper;
using Booking.Models;
using Booking.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;

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
        public IActionResult Dashboard()
        {
            ViewBag.supplierName = SessionHelper.GetObjectFromSession<SupplierModel>(HttpContext.Session, SessionKeys.Supplier.LoggedInSupplierName).SupplierName ?? null;
            return View();
        }

        public IActionResult Index()
        {
            return View(new SupplierLoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(SupplierLoginViewModel supplierLoginViewModel, CancellationToken cancellationToken = default)
        {
            var result =await _supplierService.IsValidSupplierAsync(supplierLoginViewModel.Contact, supplierLoginViewModel.Password, cancellationToken);
            if (result is not null)
            {
                HttpContext.Session.Set(SessionKeys.Supplier.LoggedInSupplierName, Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(result)));
                return RedirectToAction("Dashboard", "Supplier");
            }
            return RedirectToAction("Index", "Supplier");
        }
    }
}
