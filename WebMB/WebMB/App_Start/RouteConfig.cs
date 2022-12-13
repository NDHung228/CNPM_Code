using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebMB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Gioi-thieu",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "About", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Add-product-cart",
                url: "{controller}/{action}",
                defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional, quantity = UrlParameter.Optional },
                namespaces: new[] {"WebMB.Controllers"}
            );
        }
    }
}
