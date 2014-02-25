using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using StackExchange.Profiling;
using PageTest.App_Start;
using PageTest.Controllers;
using System.Web.Security;
using System.Security.Principal;

namespace PageTest
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var app = (MvcApplication)sender;
            var context = app.Context;
            var ex = app.Server.GetLastError();
            context.Response.Clear();
            context.ClearError();

            var httpException = ex as HttpException;
            var rouetData = new RouteData();
            rouetData.Values["controller"] = "Error";
            rouetData.Values["exception"] = ex;
            rouetData.Values["action"] = "Index";

            if (httpException != null)
            {
                switch (httpException.GetHttpCode())
                {
                    case 403:
                        rouetData.Values["action"] = "Forbidden";
                        break;
                    case 404:
                        rouetData.Values["action"] = "PageNotFound";
                        break;
                    case 500:
                        rouetData.Values["action"] = "InternalError";
                        break;
                    default:
                        rouetData.Values["action"] = "GenericError";
                        break;
                }
            }

            rouetData.Values.Add("Error", ex.Message);
            context.Response.TrySkipIisCustomErrors = true;
            IController controller = new ErrorController();
            controller.Execute(new RequestContext(new HttpContextWrapper(context), rouetData));
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            bool hasUser = HttpContext.Current.User != null;
            bool isAuthenticated = hasUser && HttpContext.Current.User.Identity.IsAuthenticated;
            bool isIdentity = isAuthenticated && HttpContext.Current.User.Identity is FormsIdentity;

            if (isIdentity)
            {
                FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                var ticket = id.Ticket;
                string userData = ticket.UserData;
                string[] roles = userData.Split(',');
                HttpContext.Current.User = new GenericPrincipal(id, roles);
            }
        }

    }
}
