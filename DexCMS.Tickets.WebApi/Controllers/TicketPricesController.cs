using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DexCMS.Tickets.Tickets.Interfaces;
using DexCMS.Tickets.Tickets.Models;
using DexCMS.Tickets.WebApi.ApiModels;

namespace DexCMS.Tickets.WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TicketPricesController : ApiController
    {
        private ITicketPriceRepository repository;

        public TicketPricesController(ITicketPriceRepository repo)
        {
            repository = repo;
        }

        // GET api/TicketPrices
        public List<TicketPriceApiModel> GetTicketPrices()
        {
            var items = repository.Items.Select(x => new TicketPriceApiModel
            {
                TicketPriceID = x.TicketPriceID,
                BasePrice = x.BasePrice,
                EventAgeGroupID = x.EventAgeGroupID,
                TicketAreaID = x.TicketAreaID,
                TicketCutoffID = x.TicketCutoffID
            }).ToList();

            return items;
        }

        // GET api/TicketPrices/5
        [ResponseType(typeof(TicketPriceApiModel))]
        public async Task<IHttpActionResult> GetTicketPrice(int id)
        {
            TicketPrice ticketPrice = await repository.RetrieveAsync(id);
            if (ticketPrice == null)
            {
                return NotFound();
            }

            TicketPriceApiModel model = new TicketPriceApiModel()
            {
                TicketPriceID = ticketPrice.TicketPriceID
            };

            return Ok(model);
        }

        [ResponseType(typeof(List<TicketPriceApiModel>))]
        public IHttpActionResult GetTicketPrice(string bytype, int id)
        {
            var items = new List<TicketPriceApiModel>();

            if (bytype == "byevent")
            {
                items = repository.Items.Where(x => x.TicketArea.EventID == id).Select(x => new TicketPriceApiModel
                {
                    TicketPriceID = x.TicketPriceID,
                    BasePrice = x.BasePrice,
                    EventAgeGroupID = x.EventAgeGroupID,
                    TicketAreaID = x.TicketAreaID,
                    TicketCutoffID = x.TicketCutoffID
                }).ToList();
            }
            else
            {
                return NotFound();
            }

            return Ok(items);
        }

        // PUT api/TicketPrices/5
        public async Task<IHttpActionResult> PutTicketPrice(int id, TicketPrice ticketPrice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ticketPrice.TicketPriceID)
            {
                return BadRequest();
            }

            await repository.UpdateAsync(ticketPrice, ticketPrice.TicketPriceID);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/TicketPrices
        [ResponseType(typeof(TicketPrice))]
        public async Task<IHttpActionResult> PostTicketPrice(TicketPrice ticketPrice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await repository.AddAsync(ticketPrice);

            return CreatedAtRoute("DefaultApi", new { id = ticketPrice.TicketPriceID }, ticketPrice);
        }

        // DELETE api/TicketPrices/5
        [ResponseType(typeof(TicketPrice))]
        public async Task<IHttpActionResult> DeleteTicketPrice(int id)
        {
            TicketPrice ticketPrice = await repository.RetrieveAsync(id);
            if (ticketPrice == null)
            {
                return NotFound();
            }

            await repository.DeleteAsync(ticketPrice);

            return Ok(ticketPrice);
        }

    }



}