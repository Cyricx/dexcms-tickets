using System.Web.Routing;
using System.Web.Mvc;
using DexCMS.Core.Models;

namespace DexCMS.Tickets.Mvc
{
    public static class TicketsMvcRoutes
    {
        public static void CreateDefaultRoutes(RouteCollection routes, DexCMSConfiguration config)
        {
            routes.MapRoute(
                name: "EvetFaqItems",
                url: "EventFaqItems/{action}/{id}",
                defaults: new { controller = "EventFaqItems" });

            routes.MapRoute(
                name: "Events",
                url: "Events/{urlSegment}",
                defaults: new { category = "events", controller = "Events", action = "Index" });

            routes.MapRoute(
                name: "EventSchedule",
                url: "Events/{category}/Schedule",
                defaults: new { urlSegment = "schedule", action = "Schedule", controller = "Events" });

            routes.MapRoute(
                name: "EventPrice",
                url: "Events/{category}/Prices",
                defaults: new { urlSegment = "prices", action = "Prices", controller = "Events" });

            routes.MapRoute(
                name: "EventRegistration",
                url: "Events/{category}/Registration",
                defaults: new { urlSegment = "registration", action = "Registration", controller = "Events" });

            routes.MapRoute(
                name: "EventFaq",
                url: "Events/{category}/FAQ",
                defaults: new { urlSegment = "faq", action = "FAQ", controller = "Events" });
        }
    }
}
