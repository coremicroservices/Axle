using System;
using System.Web.Mvc;


namespace Booking.Handler
{
    public class GlobalErrorHandler : System.Web.Mvc.HandleErrorAttribute

    {

        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;

            // Capture error details
            var exception = filterContext.Exception;
            //var controller = (string)filterContext.RouteData.Values["controller"];
            //var action = (string)filterContext.RouteData.Values["action"];

            // Example: Log to file, database, or external service
            // Logger.LogError(exception, controller, action);

            // Redirect to a friendly error view
            TempDataDictionary tempData = filterContext.Controller.TempData;
            tempData["error"] = "An unexpected error occurred. Please try again later.";

            filterContext.ExceptionHandled = true;
        }


    }
}
