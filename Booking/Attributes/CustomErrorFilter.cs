using Booking.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Booking.Attributes
{
    public class CustomErrorFilter : IExceptionFilter,
    IFilterMetadata
    {
        private readonly ILogger<CustomErrorFilter> _logger;

        public CustomErrorFilter(ILogger<CustomErrorFilter> logger)
        {
            _logger = logger;
        }

        public static List<string> GetExceptionDetails(Exception ex)
        {
            var details = new List<string>();
            int level = 0;

            while (ex != null)
            {
                details.Add($"Level {level}: {ex.GetType().Name} - {ex.Message}");
                ex = ex.InnerException;
                level++;
            }

            return details;
        }

        public void OnException(ExceptionContext context)
        {
            context.HttpContext.Session.SetString("exception", string.Join(",", GetExceptionDetails(context.Exception)));

            if (context.ExceptionHandled) return;

            _logger.LogError(context.Exception, "Unhandled exception occurred.");

            var viewData = new ViewDataDictionary(
            new EmptyModelMetadataProvider(), context.ModelState) {
        {
          "Exception",
          context.Exception
        }
      };

            context.Result = new ViewResult
            {

                ViewName = "Error",
                ViewData = new ViewDataDictionary<ErrorViewModel>(
              metadataProvider: new EmptyModelMetadataProvider(), modelState: context.ModelState)
                {
                    Model = new ErrorViewModel
                    {
                        RequestId = context.HttpContext.TraceIdentifier
                    }
                }

            };

            context.ExceptionHandled = true;

        }
    }
}