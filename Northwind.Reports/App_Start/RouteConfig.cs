using System.Web.Mvc;
using System.Web.Routing;

namespace Northwind.Web {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "DefaulPager",
                url: "{controller}/{action}/{size}-{page}",
                defaults: new { controller = "home", action = "page", size = 10, index = 1 }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "home", action = "index", id = UrlParameter.Optional }
            );
        }
    }
}
