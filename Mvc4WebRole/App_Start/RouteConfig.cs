using System.Web.Mvc;
using System.Web.Routing;

namespace Mvc4WebRole
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ProductRoutes",
                url: "otomobilvasita/aracozellikleri/{Name}/{GroupId}/{CategoryId}/{PriceCurrencyId}/{prdcId}",
                defaults: new { controller = "otomobilvasita", action = "aracozellikleri" }, namespaces: new[] { "Mvc4WebRole.Controllers" } 
               );

            routes.MapRoute(
                name: "SparepartsRoutes",
                url: "otomobilvasita/aracozellikleri/{urunno}/{grup}/{kategori}/{fiyattipi}/{isim}/{kod}",
                defaults: new { controller = "otomobilvasita", action = "aracozellikleri" }, namespaces: new[] { "Mvc4WebRole.Controllers" } 
               );

            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "otomobilvasita", action = "yeniikinciel", id = UrlParameter.Optional }, namespaces: new[] { "Mvc4WebRole.Controllers" } 
                );

        }
    }
}
