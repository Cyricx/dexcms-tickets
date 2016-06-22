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
    public class EventFaqCategoriesController : ApiController
    {
        private IEventFaqCategoryRepository repository;

        public EventFaqCategoriesController(IEventFaqCategoryRepository repo)
        {
            repository = repo;
        }

        // GET api/EventFaqCategories
        public List<EventFaqCategoryApiModel> GetEventFaqCategories()
        {
            var items = repository.Items.Select(x => new EventFaqCategoryApiModel
            {
                EventFaqCategoryID = x.EventFaqCategoryID,
                Name = x.Name,
                IsActive = x.IsActive,
                EventID = x.EventID,
                DisplayOrder = x.DisplayOrder,
                ItemCount = x.EventFaqItems.Count
            }).ToList();

            return items;
        }

        // GET api/EventFaqCategories/1
        [ResponseType(typeof(EventFaqCategoryApiModel))]
        public async Task<IHttpActionResult> GetEventAgeGroups(int id)
        {
            EventFaqCategory eventFaqCategory = await repository.RetrieveAsync(id);
            if (eventFaqCategory == null)
            {
                return NotFound();
            }

            EventFaqCategoryApiModel model = new EventFaqCategoryApiModel()
            {
                EventFaqCategoryID = eventFaqCategory.EventFaqCategoryID,
                Name = eventFaqCategory.Name,
                IsActive = eventFaqCategory.IsActive,
                EventID = eventFaqCategory.EventID,
                DisplayOrder = eventFaqCategory.DisplayOrder,
                ItemCount = eventFaqCategory.EventFaqItems.Count
            };

            return Ok(model);
        }

        //GET api/EventFaqCategories/byevent/1
        [ResponseType(typeof(List<EventFaqCategoryApiModel>))]
        public IHttpActionResult GetEventFaqCategories(string bytype, int id)
        {
            var items = new List<EventFaqCategoryApiModel>();

            if (bytype == "byevent")
            {
                items = repository.Items.Where(x => x.EventID == id).OrderBy(x => x.DisplayOrder).Select(x => new EventFaqCategoryApiModel
                {
                    EventID = x.EventID,
                    EventFaqCategoryID = x.EventFaqCategoryID,
                    Name = x.Name,
                    IsActive = x.IsActive,
                    DisplayOrder = x.DisplayOrder,
                    ItemCount = x.EventFaqItems.Count
                }).ToList();
            }
            else
            {
                return NotFound();
            }

            return Ok(items);
        }

        // PUT api/EventFaqCategories/5
        public async Task<IHttpActionResult> PutEventFaqCategory(int id, EventFaqCategory eventFaqCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eventFaqCategory.EventFaqCategoryID)
            {
                return BadRequest();
            }

            await repository.UpdateAsync(eventFaqCategory, eventFaqCategory.EventFaqCategoryID);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/EventFaqCategories
        [ResponseType(typeof(EventFaqCategory))]
        public async Task<IHttpActionResult> PostEventFaqCategory(EventFaqCategory eventFaqCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await repository.AddAsync(eventFaqCategory);

            return CreatedAtRoute("DefaultApi", new { id = eventFaqCategory.EventFaqCategoryID }, eventFaqCategory);
        }

        // DELETE api/EventFaqCategories/5
        [ResponseType(typeof(EventFaqCategory))]
        public async Task<IHttpActionResult> DeleteEventFaqCategory(int id)
        {
            EventFaqCategory eventFaqCategory = await repository.RetrieveAsync(id);
            if (eventFaqCategory == null)
            {
                return NotFound();
            }

            await repository.DeleteAsync(eventFaqCategory);

            return Ok(eventFaqCategory);
        }

    }



}