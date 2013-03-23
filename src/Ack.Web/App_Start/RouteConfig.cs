using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ack.Web
{
    public class RouteConfig
    {
        public static void RegisterMvcRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{*url}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }

        public static void RegisterWebApiRoutes(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "POST /api/events",
                routeTemplate: "api/events",
                defaults: new { controller = "Events", action = "Post" }
            );

            config.Routes.MapHttpRoute(
                name: "CatchAll",
                routeTemplate: "api/{*path}",
                defaults: new { controller = "Errors", action = "CatchAll" }
            );
        }
    }
}