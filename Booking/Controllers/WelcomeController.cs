using Booking.Data.Tables;
using Booking.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
namespace Booking.Controllers
{
    [AllowAnonymous]
    public class WelcomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            ViewBag.isNotLoggedIn = HttpContext.Session.Get(SessionKeys.User.LoggedInUserDetail) == null;

            if (ViewBag.isNotLoggedIn is false)
            {
                TempData[SessionKeys.User.LoggedInUserDetail] = SessionHelper.GetObjectFromSession<UserDto>(HttpContext.Session, SessionKeys.User.LoggedInUserDetail);
                ViewBag.LoggedInUser = $"{(TempData[SessionKeys.User.LoggedInUserDetail] as UserDto).Name}";
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
