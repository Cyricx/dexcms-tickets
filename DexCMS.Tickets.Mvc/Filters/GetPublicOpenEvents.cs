using DexCMS.Tickets.Events.Interfaces;
using System;
using System.Linq;
using System.Web.Mvc;

namespace DexCMS.Tickets.Mvc.Filters
{
    public class GetPublicOpenEvents : ActionFilterAttribute
    {
        IEventRepository repository;

        public GetPublicOpenEvents(IEventRepository _repo)
        {
            repository = _repo;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest() && filterContext.Controller.ViewBag.OpenEvents == null)
            {
                filterContext.Controller.ViewBag.OpenEvents = repository.Items.Where(x => x.IsPublic && x.EventEnd > DateTime.Now).OrderBy(x => x.PageContent.Heading).ToList();
            }
        }
    }
}
