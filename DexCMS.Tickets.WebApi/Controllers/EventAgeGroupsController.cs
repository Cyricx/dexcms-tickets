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
    public class EventAgeGroupsController : ApiController
    {
        private IEventAgeGroupRepository repository;

        public EventAgeGroupsController(IEventAgeGroupRepository repo)
        {
            repository = repo;
        }

        // GET api/EventAgeGroups
        public List<EventAgeGroupApiModel> GetEventAgeGroups()
        {
            var items = repository.Items.Select(x => new EventAgeGroupApiModel
            {
                EventAgeGroupID = x.EventAgeGroupID,
                MinimumAge = x.MinimumAge,
                MaximumAge = x.MaximumAge,
                Name = x.Name,
                EventID = x.EventID
            }).ToList();

            return items;
        }

        // GET api/EventAgeGroups/1
        [ResponseType(typeof(EventAgeGroupApiModel))]
        public async Task<IHttpActionResult> GetEventAgeGroups(int id)
        {
            EventAgeGroup eventAgeGroup = await repository.RetrieveAsync(id);
            if (eventAgeGroup == null)
            {
                return NotFound();
            }

            EventAgeGroupApiModel model = new EventAgeGroupApiModel()
            {
                EventAgeGroupID = eventAgeGroup.EventAgeGroupID,
                Name = eventAgeGroup.Name,
                MinimumAge = eventAgeGroup.MinimumAge,
                MaximumAge = eventAgeGroup.MaximumAge,
                EventID = eventAgeGroup.EventID,
                TicketOptionsCount = eventAgeGroup.TicketOptionChoices.Count,
                TicketPricesCount = eventAgeGroup.TicketPrices.Count
            };

            return Ok(model);
        }

        [ResponseType(typeof(List<EventAgeGroupApiModel>))]
        public IHttpActionResult GetEventAgeGroup(string bytype, int id)
        {
            var items = new List<EventAgeGroupApiModel>();

            if (bytype == "byevent")
            {
                items = repository.Items.OrderBy(x => x.MinimumAge).Where(x => x.EventID == id).Select(x => new EventAgeGroupApiModel
                {
                    EventAgeGroupID = x.EventAgeGroupID,
                    MinimumAge = x.MinimumAge,
                    MaximumAge = x.MaximumAge,
                    Name = x.Name,
                    EventID = x.EventID,
                    TicketOptionsCount = x.TicketOptionChoices.Count,
                    TicketPricesCount = x.TicketPrices.Count
                }).ToList();
            } else
            {
                return NotFound();
            }

            return Ok(items);
        }


        // PUT api/EventAgeGroups/5
        public async Task<IHttpActionResult> PutEventAgeGroup(int id, EventAgeGroup eventAgeGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eventAgeGroup.EventAgeGroupID)
            {
                return BadRequest();
            }

            await repository.UpdateAsync(eventAgeGroup, eventAgeGroup.EventAgeGroupID);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/EventAgeGroups
        [ResponseType(typeof(EventAgeGroup))]
        public async Task<IHttpActionResult> PostEventAgeGroup(EventAgeGroup eventAgeGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await repository.AddAsync(eventAgeGroup);

            return CreatedAtRoute("DefaultApi", new { id = eventAgeGroup.EventAgeGroupID }, eventAgeGroup);
        }

        // DELETE api/EventAgeGroups/5
        [ResponseType(typeof(EventAgeGroup))]
        public async Task<IHttpActionResult> DeleteEventAgeGroup(int id)
        {
            EventAgeGroup eventAgeGroup = await repository.RetrieveAsync(id);
            if (eventAgeGroup == null)
            {
                return NotFound();
            }

            await repository.DeleteAsync(eventAgeGroup);

            return Ok(eventAgeGroup);
        }

    }



}