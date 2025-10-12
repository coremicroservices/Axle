using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    public class WelcomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
