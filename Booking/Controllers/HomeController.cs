using Axle.Hubs;
using Booking.Data.Tables;
using Booking.Helper;
using Booking.Models;
using Booking.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Diagnostics;

namespace Booking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookingService _bookingService;
        private readonly IHubContext<ChatHub> _hubContext;  


        private readonly List<(string Code, string Name)> _products = new()
        {
            ("GEN", "General Cargo"),
            ("ELEC", "Electronics"),
            ("FRG", "Fragile"),
            ("BATT", "Batteries")
        };

        public HomeController(ILogger<HomeController> logger, IBookingService bookingService, IHubContext<ChatHub> hubContext)
        {
            _logger = logger;
            _bookingService = bookingService;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            TempData[SessionKeys.User.LoggedInUserDetail] = SessionHelper.GetObjectFromSession<UserModel>(HttpContext.Session, SessionKeys.User.LoggedInUserDetail);

            ViewBag.CurrentStep = 3;
            ViewBag.Products = _products;
            ViewBag.Suppliers = await _bookingService.GetSuppliersAsync(cancellationToken);
            if (TempData[SessionKeys.User.LoggedInUserDetail] is not null)
            {
                ViewBag.LoggedInUser = $"Welcome {(TempData[SessionKeys.User.LoggedInUserDetail] as UserModel).Name}";
            }
            return View(new ShipmentViewModel());
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShipmentViewModel model, List<string> SelectedSupplierIds)
        {
            ViewBag.Products = _products;

            if (!ModelState.IsValid)
            {
                // return view with validation messages
                return View(model);
            }

           string shipmetId =  await _bookingService.CreateShipmentAsync(model);
            if(shipmetId is not null)
            {
                SelectedSupplierIds.ForEach(async supplierId =>
                {                    
                   await _hubContext.Clients.All.SendAsync(SessionKeys.User.SendNotificationToPartner, supplierId, "New shipment created.");
                   // await _bookingService.LinkShipmentToSupplierAsync(model.ShipmentId, int.Parse(supplierId));
                });
            }
            else
            {
                TempData["Error"] = "Error occurred while creating shipment (demo).";
                return View(model);
            }   
          

            TempData["Success"] = "Shipment created successfully.";
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
