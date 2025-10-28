using Axle.Hubs;
using Booking.Data.Tables;
using Booking.FCMNotification;
using Booking.Helper;
using Booking.Hubs;
using Booking.Models;
using Booking.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using static Booking.Helper.SessionKeys;

namespace Booking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookingService _bookingService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IFCMNotification _FCMNotification;
        private readonly ConcurrentDictionary<string, string> keyValuePairs = [];

        private readonly List<(string Code, string Name)> _products = new()
        {
            ("GEN", "General Cargo"),
            ("ELEC", "Electronics"),
            ("FRG", "Fragile"),
            ("BATT", "Batteries")
        };

        public HomeController(ILogger<HomeController> logger, IBookingService bookingService, IHubContext<ChatHub> hubContext, IFCMNotification FCMNotification)
        {
            _logger = logger;
            _bookingService = bookingService;
            _hubContext = hubContext;
            _FCMNotification = FCMNotification;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            TempData[SessionKeys.User.LoggedInUserDetail] = SessionHelper.GetObjectFromSession<UserDto>(HttpContext.Session, SessionKeys.User.LoggedInUserDetail);

            var suppliers = await _bookingService.GetSuppliersAsync(cancellationToken);
            ViewBag.CurrentStep = 3;
            ViewBag.Products = _products;
            ViewBag.Suppliers = suppliers;
            suppliers.ForEach(x =>
            {
                if (x.FcmDeviceToken?.DeviceToken is not null)
                    keyValuePairs.AddOrUpdate(x.Id, x.FcmDeviceToken.DeviceToken, (key, oldValue) => x.FcmDeviceToken.DeviceToken);
            });
            if (TempData[SessionKeys.User.LoggedInUserDetail] is not null)
            {
                ViewBag.LoggedInUser = $"Welcome {(TempData[SessionKeys.User.LoggedInUserDetail] as UserDto).Name}";
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

            string shipmetId =  await _bookingService.CreateShipmentAsync(model, SelectedSupplierIds  );
            if(shipmetId is not null)
            {
                var suppliers = await _bookingService.GetSuppliersAsync();
                suppliers.ForEach(x =>
                {
                    if (x.FcmDeviceToken?.DeviceToken is not null)
                        keyValuePairs.AddOrUpdate(x.Id, x.FcmDeviceToken.DeviceToken, (key, oldValue) => x.FcmDeviceToken.DeviceToken);
                });
                SelectedSupplierIds.ForEach(async supplierId =>
                {
                    var bookingRequest = new BookingRequest()
                    {
                        CustomerName = "Sharad",
                        Drop = "Mumbai",
                        Pickup = "Pune",
                        ScheduledTime = DateTime.UtcNow.ToString(),
                        TruckType = "4L"
                    };
                    await _hubContext.Clients.User(supplierId).SendAsync(SessionKeys.User.SendNotificationToPartner, bookingRequest);
                    // await _bookingService.LinkShipmentToSupplierAsync(model.ShipmentId, int.Parse(supplierId));
                    if (keyValuePairs.ContainsKey(supplierId))
                        await _FCMNotification.SendPushNotification(keyValuePairs[supplierId], "New Shipment Created", $"A new shipment with ID {shipmetId} has been created.");
                });
            }
            else
            {
                TempData["Error"] = "Error occurred while creating shipment (demo).";
                return View(model);
            }

      

            TempData["success"] = "Shipment created successfully.";
            return RedirectToAction("MyBookings", "Customer");
            //Customer/MyBookings
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
