using System.Web;
using System.Web.Mvc;
using static STV.MvcApplication;

namespace STV
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleExceptionsAttribute());
            filters.Add(new HandleErrorAttribute()
            {
                View = "Error"
            });
        }
    }
}
