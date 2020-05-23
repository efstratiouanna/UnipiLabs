using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using UnipiLabs.ActionFilters;

namespace UnipiLabs
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            //χρησιμοποιείται ώστε να παίρνουμε το UserName του συνδεδεμένου χρήστη και να το εμφανίζουμε στην κάθε σελίδα
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters); 
            GlobalFilters.Filters.Add(new UserNameActionFilter(), 0);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
