using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Booking.Controllers
{
    public class CustomerBaseController : Controller
    {
        // Optional: override this in derived controllers to skip auth
        protected virtual bool IsPublicPage => false;

        // Optional: override this to require specific roles
        protected virtual string[] RequiredRoles => Array.Empty<string>();

         
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var path = context.HttpContext.Request.Path.Value;

            // Skip auth check for public pages
            if (IsPublicPage)
            {
                base.OnActionExecuting(context);
                return;
            }

            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                // Prevent redirect loop
                if (!path.Contains("/Auth/Index", StringComparison.OrdinalIgnoreCase))
                {
                    context.Result = RedirectToAction("Index", "Auth");
                    return;
                }
            }
            else if (RequiredRoles.Length > 0)
            {
                var hasRole = RequiredRoles.Any(role => user.IsInRole(role));
                if (!hasRole)
                {
                    context.Result = RedirectToAction("AccessDenied", "Account");
                    return;
                }
            }

            base.OnActionExecuting(context);
        }

        // Optional helper to get current user ID
        protected string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier);

    }
}
