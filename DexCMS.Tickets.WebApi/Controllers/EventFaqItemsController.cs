using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DexCMS.Tickets.Events.Interfaces;
using DexCMS.Tickets.Events.Models;
using DexCMS.Tickets.WebApi.ApiModels;

namespace DexCMS.Tickets.WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EventFaqItemsController : ApiController
    {
        private IEventFaqItemRepository repository;

        public EventFaqItemsController(IEventFaqItemRepository repo)
        {
            repository = repo;
        }

        // GET api/EventFaqItems
        public List<EventFaqItemApiModel> GetEventFaqItems()
        {
            var items = repository.Items.Select(x => new EventFaqItemApiModel
            {
                EventFaqItemID = x.EventFaqItemID,
                Answer = x.Answer,
                DisplayOrder = x.DisplayOrder,
                EventFaqCategoryName = x.EventFaqCategory.Name,
                EventFaqCategoryID = x.EventFaqCategoryID,
                HelpfulMarks = x.HelpfulMarks,
                IsActive = x.IsActive,
                LastUpdated = x.LastUpdated,
                LastUpdatedBy = x.LastUpdatedBy,
                Question = x.Question,
                UnhelpfulMarks = x.UnhelpfulMarks
            }).ToList();

            return items;
        }

        // GET api/EventFaqItems/1
        [ResponseType(typeof(EventFaqItemApiModel))]
        public async Task<IHttpActionResult> GetEventFaqItems(int id)
        {
            EventFaqItem eventFaqItem = await repository.RetrieveAsync(id);
            if (eventFaqItem == null)
            {
                return NotFound();
            }

            EventFaqItemApiModel model = new EventFaqItemApiModel()
            {
                EventFaqItemID = eventFaqItem.EventFaqItemID,
                Answer = eventFaqItem.Answer,
                DisplayOrder = eventFaqItem.DisplayOrder,
                EventFaqCategoryName = eventFaqItem.EventFaqCategory.Name,
                EventFaqCategoryID = eventFaqItem.EventFaqCategoryID,
                HelpfulMarks = eventFaqItem.HelpfulMarks,
                IsActive = eventFaqItem.IsActive,
                LastUpdated = eventFaqItem.LastUpdated,
                LastUpdatedBy = eventFaqItem.LastUpdatedBy,
                Question = eventFaqItem.Question,
                UnhelpfulMarks = eventFaqItem.UnhelpfulMarks
            };

            return Ok(model);
        }

        //GET api/EventFaqItems/byfaqcategory/1
        [ResponseType(typeof(List<EventFaqItemApiModel>))]
        public IHttpActionResult GetEventFaqItems(string bytype, int id)
        {
            var items = new List<EventFaqItemApiModel>();

            if (bytype == "byfaqcategory")
            {
                items = repository.Items.Where(x => x.EventFaqCategoryID == id).OrderBy(x => x.DisplayOrder).Select(x => new EventFaqItemApiModel
                {
                    EventFaqItemID = x.EventFaqItemID,
                    Answer = x.Answer,
                    DisplayOrder = x.DisplayOrder,
                    EventFaqCategoryName = x.EventFaqCategory.Name,
                    EventFaqCategoryID = x.EventFaqCategoryID,
                    HelpfulMarks = x.HelpfulMarks,
                    IsActive = x.IsActive,
                    LastUpdated = x.LastUpdated,
                    LastUpdatedBy = x.LastUpdatedBy,
                    Question = x.Question,
                    UnhelpfulMarks = x.UnhelpfulMarks
                }).ToList();
            }
            else
            {
                return NotFound();
            }

            return Ok(items);
        }

        // PUT api/EventFaqItems/5
        public async Task<IHttpActionResult> PutEventFaqItem(int id, EventFaqItem eventFaqItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eventFaqItem.EventFaqItemID)
            {
                return BadRequest();
            }

            if (eventFaqItem.ResetMarks.HasValue && eventFaqItem.ResetMarks.Value)
            {
                eventFaqItem.HelpfulMarks = null;
                eventFaqItem.UnhelpfulMarks = null;
            }

            await repository.UpdateAsync(eventFaqItem, eventFaqItem.EventFaqItemID);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/EventFaqItems
        [ResponseType(typeof(EventFaqItem))]
        public async Task<IHttpActionResult> PostEventFaqItem(EventFaqItem eventFaqItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await repository.AddAsync(eventFaqItem);

            return CreatedAtRoute("DefaultApi", new { id = eventFaqItem.EventFaqItemID }, eventFaqItem);
        }

        // DELETE api/EventFaqItems/5
        [ResponseType(typeof(EventFaqItem))]
        public async Task<IHttpActionResult> DeleteEventFaqItem(int id)
        {
            EventFaqItem eventFaqItem = await repository.RetrieveAsync(id);
            if (eventFaqItem == null)
            {
                return NotFound();
            }

            await repository.DeleteAsync(eventFaqItem);

            return Ok(eventFaqItem);
        }

    }


}