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
    public class EventSeriesController : ApiController
    {
        private IEventSeriesRepository repository;

        public EventSeriesController(IEventSeriesRepository repo)
        {
            repository = repo;
        }

        // GET api/EventSeries
        public List<EventSeriesApiModel> GetEventSeries()
        {
            var items = repository.Items.Select(x => new EventSeriesApiModel
            {
                EventSeriesID = x.EventSeriesID,
                SeriesName = x.SeriesName,
                IsActive = x.IsActive,
                AllowMultiplePublic = x.AllowMultiplePublic,
                SeriesUrlSegment = x.SeriesUrlSegment
            }).ToList();

            return items;
        }

        // GET api/EventSeries/5
        [ResponseType(typeof(EventSeries))]
        public async Task<IHttpActionResult> GetEventSeries(int id)
        {
            EventSeries eventSeries = await repository.RetrieveAsync(id);
            if (eventSeries == null)
            {
                return NotFound();
            }

            EventSeriesApiModel model = new EventSeriesApiModel()
            {
                EventSeriesID = eventSeries.EventSeriesID,
                SeriesName = eventSeries.SeriesName,
                IsActive = eventSeries.IsActive,
                AllowMultiplePublic = eventSeries.AllowMultiplePublic,
                SeriesUrlSegment = eventSeries.SeriesUrlSegment
            };

            return Ok(model);
        }

        // PUT api/EventSeries/5
        public async Task<IHttpActionResult> PutEventSeries(int id, EventSeries eventSeries)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eventSeries.EventSeriesID)
            {
                return BadRequest();
            }

            await repository.UpdateAsync(eventSeries, eventSeries.EventSeriesID);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/EventSeries
        [ResponseType(typeof(EventSeries))]
        public async Task<IHttpActionResult> PostEventSeries(EventSeries eventSeries)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await repository.AddAsync(eventSeries);

            return CreatedAtRoute("DefaultApi", new { id = eventSeries.EventSeriesID }, eventSeries);
        }

        // DELETE api/EventSeries/5
        [ResponseType(typeof(EventSeries))]
        public async Task<IHttpActionResult> DeleteEventSeries(int id)
        {
            EventSeries eventSeries = await repository.RetrieveAsync(id);
            if (eventSeries == null)
            {
                return NotFound();
            }

            await repository.DeleteAsync(eventSeries);

            return Ok(eventSeries);
        }

    }


}