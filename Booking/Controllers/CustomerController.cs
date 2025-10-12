using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MyBookings()
        {
            return View();
        }
    }
}
