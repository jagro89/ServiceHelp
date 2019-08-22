using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HelpDesk
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Contact",
                url: "kontakt",
                defaults: new { controller = "Information", action = "Contact" }
            );

            routes.MapRoute(
               name: "KnowledgeBase",
               url: "bazawiedzy",
               defaults: new { controller = "Information", action = "KnowledgeBase" }
           );

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
              name: "Default2",
              url: "{controller}/{action}",
              defaults: new { controller = "Home", action = "Index" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
