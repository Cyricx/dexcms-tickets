using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using DexCMS.Tickets.Events.Interfaces;

namespace DexCMS.Tickets.Mvc.Controllers
{
    public class EventFaqItemsController : Controller
    {
        private IEventFaqItemRepository repository;

        public EventFaqItemsController(IEventFaqItemRepository repo)
        {
            repository = repo;
        }

        // GET: EventFaqItems
        public async Task<JsonResult> Helpful(int id)
        {
            var faq = await repository.RetrieveAsync(id);

            if (faq == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            if (faq.HelpfulMarks.HasValue)
            {
                faq.HelpfulMarks++;
            } else
            {
                faq.HelpfulMarks = 1;
            }

            await repository.UpdateAsync(faq, faq.EventFaqItemID);

            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Unhelpful(int id)
        {
            var faq = await repository.RetrieveAsync(id);

            if (faq == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            if (faq.UnhelpfulMarks.HasValue)
            {
                faq.UnhelpfulMarks--;
            }
            else
            {
                faq.UnhelpfulMarks = -1;
            }

            await repository.UpdateAsync(faq, faq.EventFaqItemID);

            return Json("Success", JsonRequestBehavior.AllowGet);
        }
    }
}