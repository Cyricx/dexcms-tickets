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
    public class ScheduleTypesController : ApiController
    {
        private IScheduleTypeRepository repository;

        public ScheduleTypesController(IScheduleTypeRepository repo)
        {
            repository = repo;
        }

        // GET api/ScheduleTypes
        public List<ScheduleTypeApiModel> GetScheduleTypes()
        {
            var items = repository.Items.OrderBy(x => x.Name).Select(x => new ScheduleTypeApiModel
            {
                ScheduleTypeID = x.ScheduleTypeID,
                Name = x.Name,
                IsActive = x.IsActive,
                CssClass = x.CssClass,
                ScheduleItemCount = x.ScheduleItems.Count
            }).ToList();

            return items;
        }

        // GET api/ScheduleTypes/5
        [ResponseType(typeof(ScheduleType))]
        public async Task<IHttpActionResult> GetScheduleType(int id)
        {
            ScheduleType scheduleType = await repository.RetrieveAsync(id);
            if (scheduleType == null)
            {
                return NotFound();
            }

            ScheduleTypeApiModel model = new ScheduleTypeApiModel()
            {
                ScheduleTypeID = scheduleType.ScheduleTypeID,
                Name = scheduleType.Name,
                IsActive = scheduleType.IsActive,
                CssClass = scheduleType.CssClass,
                ScheduleItemCount = scheduleType.ScheduleItems.Count
            };

            return Ok(model);
        }

        // PUT api/ScheduleTypes/5
        public async Task<IHttpActionResult> PutScheduleType(int id, ScheduleType ScheduleType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ScheduleType.ScheduleTypeID)
            {
                return BadRequest();
            }

            await repository.UpdateAsync(ScheduleType, ScheduleType.ScheduleTypeID);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/ScheduleTypes
        [ResponseType(typeof(ScheduleType))]
        public async Task<IHttpActionResult> PostScheduleType(ScheduleType ScheduleType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await repository.AddAsync(ScheduleType);

            return CreatedAtRoute("DefaultApi", new { id = ScheduleType.ScheduleTypeID }, ScheduleType);
        }

        // DELETE api/ScheduleTypes/5
        [ResponseType(typeof(ScheduleType))]
        public async Task<IHttpActionResult> DeleteScheduleType(int id)
        {
            ScheduleType ScheduleType = await repository.RetrieveAsync(id);
            if (ScheduleType == null)
            {
                return NotFound();
            }

            await repository.DeleteAsync(ScheduleType);

            return Ok(ScheduleType);
        }

    }


}