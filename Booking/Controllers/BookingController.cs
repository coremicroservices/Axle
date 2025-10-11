using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
