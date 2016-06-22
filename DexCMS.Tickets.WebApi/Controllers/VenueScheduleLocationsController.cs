using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DexCMS.Tickets.Venues.Interfaces;
using DexCMS.Tickets.Venues.Models;
using DexCMS.Tickets.WebApi.ApiModels;

namespace DexCMS.Tickets.WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VenueScheduleLocationsController : ApiController
    {
        private IVenueScheduleLocationRepository repository;

        public VenueScheduleLocationsController(IVenueScheduleLocationRepository repo)
        {
            repository = repo;
        }

        // GET api/VenueScheduleLocations
        public List<VenueScheduleLocationApiModel> GetVenueScheduleLocations()
        {
            var items = repository.Items.Select(x => new VenueScheduleLocationApiModel
            {
                VenueScheduleLocationID = x.VenueScheduleLocationID,
                Name = x.Name,
                IsActive = x.IsActive,
                CssClass = x.CssClass,
                VenueID = x.VenueID,
                VenueName = x.Venue.Name,
                ScheduleItemCount = x.ScheduleItems.Count
            }).ToList();

            return items;
        }

        // GET api/VenueScheduleLocations/5
        [ResponseType(typeof(VenueScheduleLocation))]
        public async Task<IHttpActionResult> GetVenueScheduleLocation(int id)
        {
            VenueScheduleLocation venueScheduleLocation = await repository.RetrieveAsync(id);
            if (venueScheduleLocation == null)
            {
                return NotFound();
            }

            VenueScheduleLocationApiModel model = new VenueScheduleLocationApiModel()
            {
                VenueScheduleLocationID = venueScheduleLocation.VenueScheduleLocationID,
                Name = venueScheduleLocation.Name,
                IsActive = venueScheduleLocation.IsActive,
                CssClass = venueScheduleLocation.CssClass,
                VenueID = venueScheduleLocation.VenueID,
                VenueName = venueScheduleLocation.Venue.Name,
                ScheduleItemCount = venueScheduleLocation.ScheduleItems.Count
            };

            return Ok(model);
        }

        [ResponseType(typeof(List<VenueScheduleLocationApiModel>))]
        public IHttpActionResult GetVenueScheduleLocations(string bytype, int id)
        {
            var items = new List<VenueScheduleLocationApiModel>();

            if (bytype == "byvenue")
            {
                items = repository.Items.Where(x => x.VenueID == id).OrderBy(x => x.Name).Select(x => new VenueScheduleLocationApiModel
                {
                    VenueScheduleLocationID = x.VenueScheduleLocationID,
                    Name = x.Name,
                    IsActive = x.IsActive,
                    CssClass = x.CssClass,
                    VenueID = x.VenueID,
                    VenueName = x.Venue.Name,
                    ScheduleItemCount = x.ScheduleItems.Count
                }).ToList();
            }
            else if (bytype == "byevent")
            {
                items = repository.Items.Where(x => x.Venue.Events.Count(y => y.EventID == id) > 0)
                    .OrderBy(x => x.Name).Select(x => new VenueScheduleLocationApiModel
                    {
                        VenueScheduleLocationID = x.VenueScheduleLocationID,
                        Name = x.Name,
                        IsActive = x.IsActive,
                        CssClass = x.CssClass,
                        VenueID = x.VenueID,
                        VenueName = x.Venue.Name,
                        ScheduleItemCount = x.ScheduleItems.Count
                    }).ToList();
            }
            else
            {
                return NotFound();
            }

            return Ok(items);
        }

        // PUT api/VenueScheduleLocations/5
        public async Task<IHttpActionResult> PutVenueScheduleLocation(int id, VenueScheduleLocation venueScheduleLocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != venueScheduleLocation.VenueScheduleLocationID)
            {
                return BadRequest();
            }

            await repository.UpdateAsync(venueScheduleLocation, venueScheduleLocation.VenueScheduleLocationID);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/VenueScheduleLocations
        [ResponseType(typeof(VenueScheduleLocation))]
        public async Task<IHttpActionResult> PostVenueScheduleLocation(VenueScheduleLocation venueScheduleLocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await repository.AddAsync(venueScheduleLocation);

            return CreatedAtRoute("DefaultApi", new { id = venueScheduleLocation.VenueScheduleLocationID }, venueScheduleLocation);
        }

        // DELETE api/VenueScheduleLocations/5
        [ResponseType(typeof(VenueScheduleLocation))]
        public async Task<IHttpActionResult> DeleteVenueScheduleLocation(int id)
        {
            VenueScheduleLocation venueScheduleLocation = await repository.RetrieveAsync(id);
            if (venueScheduleLocation == null)
            {
                return NotFound();
            }

            await repository.DeleteAsync(venueScheduleLocation);

            return Ok(venueScheduleLocation);
        }

    }


}