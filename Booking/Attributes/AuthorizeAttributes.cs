using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


namespace Booking.Attributes
{
    public class MyAuthorizeAttribute : Attribute, IAuthorizationFilter, IFilterMetadata
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var path = filterContext.HttpContext.Request.Path.Value;

            var excludedPaths = new[] {"/", "/Welcome/Index", "/Account/Login" };
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)


            {
                var factory = filterContext.HttpContext.RequestServices.GetService(typeof(ITempDataDictionaryFactory)) as ITempDataDictionaryFactory;
                var tempData = factory?.GetTempData(filterContext.HttpContext);

                if (tempData != null)
                {
                    tempData["error"] = "You are not authorized to access this resource.";
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Auth", action = "Index" }));
                }
            }
            Console.WriteLine("Authorization filter executed.");
        }


    }
}
