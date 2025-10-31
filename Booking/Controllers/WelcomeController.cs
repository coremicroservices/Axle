using Booking.Data.Tables;
using Booking.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    public class WelcomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.isNotLoggedIn = HttpContext.Session.Get(SessionKeys.User.LoggedInUserDetail) == null;

            if (ViewBag.isNotLoggedIn is false)
            {
                TempData[SessionKeys.User.LoggedInUserDetail] = SessionHelper.GetObjectFromSession<UserDto>(HttpContext.Session, SessionKeys.User.LoggedInUserDetail);
                ViewBag.LoggedInUser = $"Welcome {(TempData[SessionKeys.User.LoggedInUserDetail] as UserDto).Name}";
            }
            return View();
        }
    }
}
