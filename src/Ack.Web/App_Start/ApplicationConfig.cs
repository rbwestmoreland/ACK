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
            WebApiInitialize(GlobalConfiguration.Configuration);
            MvcInitialize();
        }

        public static void WebApiInitialize(HttpConfiguration config)
        {
            WebApiFormattersConfig.Configure(config);
            IoCConfig.RegisterWebApi(config);
            RouteConfig.RegisterWebApiRoutes(config);
        }

        public static void MvcInitialize()
        {
            IoCConfig.RegisterMvc(ControllerBuilder.Current, GlobalFilters.Filters);
            //AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterMvcRoutes(RouteTable.Routes);
        }
    }
}