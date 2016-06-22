using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DexCMS.Tickets.Venues.Models;
using DexCMS.Tickets.Venues.Interfaces;
using DexCMS.Tickets.WebApi.ApiModels;

namespace DexCMS.Tickets.WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VenuesController : ApiController
    {
		private IVenueRepository repository;

		public VenuesController(IVenueRepository repo) 
		{
			repository = repo;
		}

        // GET api/Venues
        public List<VenueApiModel> GetVenues()
        {
			var items = repository.Items.Select(x => new VenueApiModel {
				VenueID = x.VenueID,
				Name = x.Name,
				Address = x.Address,
				City = x.City,
				StateID = x.StateID,
				ZipCode = x.ZipCode
			}).ToList();

			return items;
        }

        // GET api/Venues/5
        [ResponseType(typeof(Venue))]
        public async Task<IHttpActionResult> GetVenue(int id)
        {
			Venue venue = await repository.RetrieveAsync(id);
            if (venue == null)
            {
                return NotFound();
            }

			VenueApiModel model = new VenueApiModel()
			{
				VenueID = venue.VenueID,
				Name = venue.Name,
				Address = venue.Address,
				City = venue.City,
				StateID = venue.StateID,
				ZipCode = venue.ZipCode,
                VenueAreas = venue.VenueAreas.Select(va => new VenueAreaApiModel
                {
                    VenueAreaID = va.VenueAreaID,
                    Name = va.Name,
                    IsGA = va.IsGA,
                    GASeatCount = va.GASeatCount,
                    VenueSections = va.VenueSections.Select(vs => new VenueSectionApiModel
                    {
                        Name = vs.Name,
                        VenueSectionID = vs.VenueSectionID,
                        VenueRows = vs.VenueRows.Select(vr => new VenueRowApiModel
                        {
                             VenueRowID = vr.VenueRowID,
                             Designation = vr.Designation,
                             SeatCount = vr.SeatCount
                        }).ToList()
                    }).ToList()
                }).ToList()
			};

            return Ok(model);
        }

        // PUT api/Venues/5
        public async Task<IHttpActionResult> PutVenue(int id, Venue venue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != venue.VenueID)
            {
                return BadRequest();
            }

			await repository.UpdateAsync(venue, venue.VenueID);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Venues
        [ResponseType(typeof(Venue))]
        public async Task<IHttpActionResult> PostVenue(Venue venue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

			await repository.AddAsync(venue);

            return CreatedAtRoute("DefaultApi", new { id = venue.VenueID }, venue);
        }

        // DELETE api/Venues/5
        [ResponseType(typeof(Venue))]
        public async Task<IHttpActionResult> DeleteVenue(int id)
        {
			Venue venue = await repository.RetrieveAsync(id);
            if (venue == null)
            {
                return NotFound();
            }

			await repository.DeleteAsync(venue);

            return Ok(venue);
        }

    }



}