using System.Diagnostics;
using Booking.Models;
using Booking.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Booking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookingService _bookingService;

        private readonly List<(string Code, string Name)> _products = new()
        {
            ("GEN", "General Cargo"),
            ("ELEC", "Electronics"),
            ("FRG", "Fragile"),
            ("BATT", "Batteries")
        };

        public HomeController(ILogger<HomeController> logger, IBookingService bookingService)
        {
            _logger = logger;
            _bookingService = bookingService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            ViewBag.CurrentStep = 3;
            ViewBag.Products = _products;
            ViewBag.Suppliers = await _bookingService.GetSuppliersAsync(cancellationToken);
            return View(new ShipmentViewModel());
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShipmentViewModel model)
        {
            ViewBag.Products = _products;

            if (!ModelState.IsValid)
            {
                // return view with validation messages
                return View(model);
            }

            await _bookingService.CreateShipmentAsync(model);

            TempData["Success"] = "Shipment created successfully (demo).";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult UpdateStep([FromBody] StepModel model)
        {
            ViewBag.CurrentStep = model.Step;
            return Ok();
        }

        public class StepModel
        {
            public int Step { get; set; }
        }
    }
}
