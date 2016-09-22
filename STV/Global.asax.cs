﻿using STV.Mappers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using System;
using System.Web;
using System.Web.Security;
using System.Security.Principal;
using STV.Auth;
using System.Collections.Generic;
using Newtonsoft.Json;

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

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            if (HttpContext.Current.User == null) return;
            if (!HttpContext.Current.User.Identity.IsAuthenticated) return;
            if (HttpContext.Current.User.Identity is FormsIdentity)
            {
                var id = (FormsIdentity)HttpContext.Current.User.Identity;
                SessionContext auth = new SessionContext();
                var userData = auth.GetUserData();

                List<string> lstRoles = new List<string>();
                foreach (var role in userData.Roles)
                {
                    lstRoles.Add(role.Nome);
                }
                string[] roles = lstRoles.ToArray();

                HttpContext.Current.User = new GenericPrincipal(id, roles);
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpContext context = app.Context;
            Exception ex = context.Server.GetLastError();
            bool isAjaxCall = string.Equals(
                "XMLHttpRequest",
                context.Request.Headers["x-requested-with"],
                StringComparison.OrdinalIgnoreCase
            );
            if (isAjaxCall)
            {
                context.Response.StatusCode = 500;
                context.Response.Write(ex.Message);
            }
            else
            {
                // TODO: Handle the case of non async calls
            }
        }

    }
}
