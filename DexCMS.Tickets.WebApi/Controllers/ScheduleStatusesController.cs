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
    public class ScheduleStatusesController : ApiController
    {
        private IScheduleStatusRepository repository;

        public ScheduleStatusesController(IScheduleStatusRepository repo)
        {
            repository = repo;
        }

        // GET api/ScheduleStatuses
        public List<ScheduleStatusApiModel> GetScheduleStatuses()
        {
            var items = repository.Items.OrderBy(x => x.Name).Select(x => new ScheduleStatusApiModel
            {
                ScheduleStatusID = x.ScheduleStatusID,
                Name = x.Name,
                IsActive = x.IsActive,
                CssClass = x.CssClass,
                ScheduleItemCount = x.ScheduleItems.Count
            }).ToList();

            return items;
        }

        // GET api/ScheduleStatuses/5
        [ResponseType(typeof(ScheduleStatus))]
        public async Task<IHttpActionResult> GetScheduleStatus(int id)
        {
            ScheduleStatus scheduleStatus = await repository.RetrieveAsync(id);
            if (scheduleStatus == null)
            {
                return NotFound();
            }

            ScheduleStatusApiModel model = new ScheduleStatusApiModel()
            {
                ScheduleStatusID = scheduleStatus.ScheduleStatusID,
                Name = scheduleStatus.Name,
                IsActive = scheduleStatus.IsActive,
                CssClass = scheduleStatus.CssClass,
                ScheduleItemCount = scheduleStatus.ScheduleItems.Count

            };

            return Ok(model);
        }

        // PUT api/ScheduleStatuses/5
        public async Task<IHttpActionResult> PutScheduleStatus(int id, ScheduleStatus ScheduleStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ScheduleStatus.ScheduleStatusID)
            {
                return BadRequest();
            }

            await repository.UpdateAsync(ScheduleStatus, ScheduleStatus.ScheduleStatusID);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/ScheduleStatuses
        [ResponseType(typeof(ScheduleStatus))]
        public async Task<IHttpActionResult> PostScheduleStatus(ScheduleStatus ScheduleStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await repository.AddAsync(ScheduleStatus);

            return CreatedAtRoute("DefaultApi", new { id = ScheduleStatus.ScheduleStatusID }, ScheduleStatus);
        }

        // DELETE api/ScheduleStatuses/5
        [ResponseType(typeof(ScheduleStatus))]
        public async Task<IHttpActionResult> DeleteScheduleStatus(int id)
        {
            ScheduleStatus ScheduleStatus = await repository.RetrieveAsync(id);
            if (ScheduleStatus == null)
            {
                return NotFound();
            }

            await repository.DeleteAsync(ScheduleStatus);

            return Ok(ScheduleStatus);
        }

    }


}