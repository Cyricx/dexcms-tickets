using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DexCMS.Tickets.Schedules.Interfaces;
using DexCMS.Tickets.Schedules.Models;
using DexCMS.Tickets.WebApi.ApiModels;

namespace DexCMS.Tickets.WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ScheduleItemsController : ApiController
    {
        private IScheduleItemRepository repository;

        public ScheduleItemsController(IScheduleItemRepository repo)
        {
            repository = repo;
        }

        // GET api/ScheduleItems
        public List<ScheduleItemApiModel> GetScheduleItems()
        {
            var items = repository.Items.Select(x => new ScheduleItemApiModel
            {
                ScheduleItemID = x.ScheduleItemID,
                Title = x.Title,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                IsAllDay = x.IsAllDay,
                OtherLocation = x.OtherLocation,
                VenueScheduleLocationID = x.VenueScheduleLocationID,
                VenueScheduleLocationName = x.VenueScheduleLocationID.HasValue ? x.VenueScheduleLocation.Name : "",
                ScheduleStatusID = x.ScheduleStatusID,
                ScheduleStatusName = x.ScheduleStatus.Name,
                ScheduleTypeID = x.ScheduleTypeID,
                ScheduleTypeName = x.ScheduleType.Name,
                Details = x.Details,
                EventID = x.EventID
            }).ToList();

            return items;
        }

        // GET api/ScheduleItems/5
        [ResponseType(typeof(ScheduleItem))]
        public async Task<IHttpActionResult> GetScheduleItem(int id)
        {
            ScheduleItem scheduleItem = await repository.RetrieveAsync(id);
            if (scheduleItem == null)
            {
                return NotFound();
            }

            ScheduleItemApiModel model = new ScheduleItemApiModel()
            {
                ScheduleItemID = scheduleItem.ScheduleItemID,
                Title = scheduleItem.Title,
                StartDate = scheduleItem.StartDate,
                EndDate = scheduleItem.EndDate,
                IsAllDay = scheduleItem.IsAllDay,
                OtherLocation = scheduleItem.OtherLocation,
                VenueScheduleLocationID = scheduleItem.VenueScheduleLocationID,
                VenueScheduleLocationName = scheduleItem.VenueScheduleLocationID.HasValue ? scheduleItem.VenueScheduleLocation.Name : "",
                ScheduleStatusID = scheduleItem.ScheduleStatusID,
                ScheduleStatusName = scheduleItem.ScheduleStatus.Name,
                ScheduleTypeID = scheduleItem.ScheduleTypeID,
                ScheduleTypeName = scheduleItem.ScheduleType.Name,
                Details = scheduleItem.Details,
                EventID = scheduleItem.EventID
            };

            return Ok(model);
        }

        [ResponseType(typeof(List<ScheduleItemApiModel>))]
        public IHttpActionResult GetScheduleItems(string bytype, int id)
        {
            var items = new List<ScheduleItemApiModel>();

            if (bytype == "byevent")
            {
                items = repository.Items.Where(x => x.EventID == id).Select(x => new ScheduleItemApiModel
                {
                    ScheduleItemID = x.ScheduleItemID,
                    Title = x.Title,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    IsAllDay = x.IsAllDay,
                    OtherLocation = x.OtherLocation,
                    VenueScheduleLocationID = x.VenueScheduleLocationID,
                    VenueScheduleLocationName = x.VenueScheduleLocationID.HasValue ? x.VenueScheduleLocation.Name : "",
                    ScheduleStatusID = x.ScheduleStatusID,
                    ScheduleStatusName = x.ScheduleStatus.Name,
                    ScheduleTypeID = x.ScheduleTypeID,
                    ScheduleTypeName = x.ScheduleType.Name,
                    Details = x.Details,
                    EventID = x.EventID
                }).ToList();
            }
            else
            {
                return NotFound();
            }

            return Ok(items);
        }

        // PUT api/ScheduleItems/5
        public async Task<IHttpActionResult> PutScheduleItem(int id, ScheduleItem venueScheduleLocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != venueScheduleLocation.ScheduleItemID)
            {
                return BadRequest();
            }

            await repository.UpdateAsync(venueScheduleLocation, venueScheduleLocation.ScheduleItemID);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/ScheduleItems
        [ResponseType(typeof(ScheduleItem))]
        public async Task<IHttpActionResult> PostScheduleItem(ScheduleItem venueScheduleLocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await repository.AddAsync(venueScheduleLocation);

            return CreatedAtRoute("DefaultApi", new { id = venueScheduleLocation.ScheduleItemID }, venueScheduleLocation);
        }

        // DELETE api/ScheduleItems/5
        [ResponseType(typeof(ScheduleItem))]
        public async Task<IHttpActionResult> DeleteScheduleItem(int id)
        {
            ScheduleItem venueScheduleLocation = await repository.RetrieveAsync(id);
            if (venueScheduleLocation == null)
            {
                return NotFound();
            }

            await repository.DeleteAsync(venueScheduleLocation);

            return Ok(venueScheduleLocation);
        }

    }


}