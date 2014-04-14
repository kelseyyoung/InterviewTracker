using InterviewTracker.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Validation.Providers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace InterviewTracker
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            GlobalConfiguration.Configuration.Services.RemoveAll(
                typeof(System.Web.Http.Validation.ModelValidatorProvider),
                v => v is InvalidModelValidatorProvider);

        }

        // Override to register logged in user and set in HttpContext
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
           HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            // If cookie isn't null or hasn't expired yet
            // TODO: change expiration to certain time
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (!authTicket.Expired)
                {
                    if (authTicket.Name == "Logout")
                    {
                        // Logout the user
                        HttpContext.Current.User = null;
                    }
                    else
                    {
                        // Login the user
                        CustomPrincipal newUser = new CustomPrincipal(authTicket.Name, authTicket.UserData);
                        HttpContext.Current.User = newUser;
                    }
                }
            }
        }
    }
}