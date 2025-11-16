using Axle.Hubs;
using Booking.Attributes;
using Booking.Data.Tables;
using Booking.FCMNotification;
using Booking.Helper;
using Booking.Hubs;
using Booking.Models;
using Booking.MyConfiguration;
using Booking.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using Paytm;

namespace Booking.Controllers
{
    
    public class HomeController : CustomerBaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookingService _bookingService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IFCMNotification _FCMNotification;
        private readonly ConcurrentDictionary<string, string> keyValuePairs = [];

        private readonly MyConfigurationSettinggs _myConfigurationSettinggs;

        private readonly List<(string Code, string Name)> _products = new()
        {
            ("GEN", "General Cargo"),
            ("ELEC", "Electronics"),
            ("FRG", "Fragile"),
            ("BATT", "Batteries")
        };

        public HomeController(ILogger<HomeController> logger, IBookingService bookingService, IHubContext<ChatHub> hubContext, IFCMNotification FCMNotification,IOptions<MyConfigurationSettinggs> myConfigurationSettinggs)
        {
            _logger = logger;
            _bookingService = bookingService;
            _hubContext = hubContext;
            _FCMNotification = FCMNotification;
            _myConfigurationSettinggs = myConfigurationSettinggs.Value;
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
        public async Task<IActionResult> Create(ShipmentViewModel model, List<string> SelectedSupplierIds,CancellationToken cancellationToken = default )
        {
            ViewBag.Products = _products;

            if (!ModelState.IsValid)
            {
                // return view with validation messages
                return View("Index",model);
            }

           var UserDto = SessionHelper.GetObjectFromSession<UserDto>(HttpContext.Session, SessionKeys.User.LoggedInUserDetail);

            string shipmentId = await _bookingService.CreateShipmentAsync(model, SelectedSupplierIds, UserDto.Id, cancellationToken);
            if(shipmentId is not null)
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
                        BookingId = shipmentId,
                        CustomerName = "Sharad",
                        DropLocation = $"{model.DestinationAddress} . Pin Code({model.DestinationPincode}) ",
                        PickupLocation = $"{model.SourceAddress} . Pin Code({model.SourcePincode}) ",
                        ScheduledTime = model.BookingDate,
                        TruckType = _products.First(x=>x.Code.Equals(model.ProductCode)).Name ?? string.Empty
                    };
                    await _hubContext.Clients.User(supplierId).SendAsync(SessionKeys.User.SendNotificationToPartner, bookingRequest);
                    // await _bookingService.LinkShipmentToSupplierAsync(model.ShipmentId, int.Parse(supplierId));
                    if (keyValuePairs.ContainsKey(supplierId))
                        await _FCMNotification.SendPushNotification(keyValuePairs[supplierId], "New Shipment Created", $"A new shipment with ID {shipmentId} has been created.");
                });
            }
            else
            {
                TempData["Error"] = "Error occurred while creating shipment (demo).";
                return View(model);
            }

            // Prepare parameters for Paytm Initiate Transaction API
            Dictionary<string, object> body = new Dictionary<string, object>();
            Dictionary<string, string> head = new Dictionary<string, string>();
            Dictionary<string, object> requestBody = new Dictionary<string, object>();

            Dictionary<string, string> txnAmount = new Dictionary<string, string>();
            txnAmount.Add("value", "1.00");
            txnAmount.Add("currency", "INR");
            Dictionary<string, string> userInfo = new Dictionary<string, string>();
            userInfo.Add("custId", userDto.Id);
            body.Add("requestType", "Payment");
            body.Add("mid", _myConfigurationSettinggs.MID);
            body.Add("websiteName", "https://www.axle.com/");
            body.Add("orderId", shipmentId);
            body.Add("txnAmount", txnAmount);
            body.Add("userInfo", userInfo);
            body.Add("callbackUrl", "https://<callback URL to be used by merchant>");



            /*
            * Generate checksum by parameters we have in body
            * Find your Merchant Key in your Paytm Dashboard at https://dashboard.paytmpayments.com/next/apikeys 
            */
            
            string paytmChecksum = Checksum.generateSignature(JsonConvert.SerializeObject(body), "YOUR_KEY_HERE");

            head.Add("signature", paytmChecksum);

            requestBody.Add("body", body);
            requestBody.Add("head", head);

            string post_data = JsonConvert.SerializeObject(requestBody);

            //For  Staging
            string url = $"https://securestage.paytmpayments.com/theia/api/v1/initiateTransaction?mid={_myConfigurationSettinggs.MID}&orderId={shipmentId}";

            //For  Production 
            //string  url  =  "https://secure.paytmpayments.com/theia/api/v1/initiateTransaction?mid=YOUR_MID_HERE&orderId=ORDERID_98765";

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

            webRequest.Method = "POST";
            webRequest.ContentType = "application/json";
            webRequest.ContentLength = post_data.Length;

            using (StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream()))
            {
                requestWriter.Write(post_data);
            }

            string responseData = string.Empty;

            using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
            {
                responseData = responseReader.ReadToEnd();
                Console.WriteLine(responseData);
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
