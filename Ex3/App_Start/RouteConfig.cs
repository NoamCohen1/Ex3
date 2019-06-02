using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ex3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("chooseDisplay", "display/{str}/{num}",
            defaults: new { controller = "Home", action = "chooseDisplay" }
           );

            routes.MapRoute("pathDisplay", "display/{ip}/{port}/{time}",
            defaults: new { controller = "Home", action = "pathDisplay" }
           );

            routes.MapRoute("fileDisplay", "save/{ip}/{port}/{time}/{seconds}/{fileName}",
            defaults: new { controller = "Home", action = "fileDisplay" }
           );

           // routes.MapRoute("loadDisplay", "display/{fileName}/{time}",
           // defaults: new { controller = "Home", action = "loadDisplay", }
           //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
