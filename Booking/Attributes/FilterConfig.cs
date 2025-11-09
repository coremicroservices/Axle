using System.Web.Mvc;

namespace Booking.Attributes
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomErrorFilter());
        }

    }
}
