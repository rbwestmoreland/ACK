using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Ack.Web
{
    public static class ApplicationConfig
    {
        public static void Initialize()
        {
            WebApiConfig(GlobalConfiguration.Configuration);
            MvcConfig();
        }

        public static void WebApiConfig(HttpConfiguration config)
        {
            RouteConfig.RegisterWebApiRoutes(config);
        }

        public static void MvcConfig()
        {
            //AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterMvcRoutes(RouteTable.Routes);
        }
    }
}