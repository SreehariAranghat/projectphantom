using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using SupportPanda.Core;
using Newtonsoft.Json;
using System.Security.Principal;
using System.Threading;

namespace SupportPanda.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);            
        }


        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null && (!string.IsNullOrEmpty(authCookie.Value)))
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                User user                            = JsonConvert.DeserializeObject<User>(authTicket.UserData);

                AppIdentity identity     = new AppIdentity(user.Id.ToString(), user);
                IPrincipal userPrincipal = new GenericPrincipal(identity, user.Permissions.ToArray());

                Thread.CurrentPrincipal = userPrincipal;
                Context.User            = userPrincipal;
            }
        }
    }
}