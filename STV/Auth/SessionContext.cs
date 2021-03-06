﻿using Newtonsoft.Json;
using STV.Models;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace STV.Auth
{
    [Authorize]
    public class SessionContext
    {
        public void SetAuthenticationToken(string name, bool isPersistant, Usuario userData)
        {
            string data = null;
            if (userData != null)
                data = JsonConvert.SerializeObject(userData);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, name, DateTime.Now, DateTime.Now.AddHours(2), isPersistant, data);

            string cookieData = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieData)
            {
                HttpOnly = true,
                Expires = ticket.Expiration
            };

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public Usuario GetUserData()
        {
            Usuario userData = null;

            HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                userData = JsonConvert.DeserializeObject(ticket.UserData, typeof(Usuario)) as Usuario;
            }
            return userData;
        }
    }
}