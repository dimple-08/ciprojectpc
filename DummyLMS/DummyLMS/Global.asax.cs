using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;

namespace DummyLMS
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            if (!WebSecurity.Initialized)
            {
                string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=LMS;Integrated Security=True;";
                WebSecurity.InitializeDatabaseConnection("connection", "tblLogin", "id", "userName", autoCreateTables: true);
            }

        }
    }
}
