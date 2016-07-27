using STV.Mappers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using System;
using System.Web;
using System.Web.Security;
using System.Security.Principal;
using STV.Auth;

namespace STV
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.RegisterMappings();
        }

        //protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        //{
        //    if (HttpContext.Current.User == null) return;
        //    if (!HttpContext.Current.User.Identity.IsAuthenticated) return;
        //    if (HttpContext.Current.User.Identity is FormsIdentity)
        //    {
        //        var id = (FormsIdentity)HttpContext.Current.User.Identity;
        //        SessionContext auth = new SessionContext();
        //        var userData = auth.GetUserData();
        //        string[] roles = userData.Role.Split(',');
        //        HttpContext.Current.User = new GenericPrincipal(id, roles);
        //    }
        //}

    }
}
