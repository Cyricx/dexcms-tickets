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
    public class TicketAreaDiscountsController : ApiController
    {
        private ITicketAreaDiscountRepository repository;

        public TicketAreaDiscountsController(ITicketAreaDiscountRepository repo)
        {
            repository = repo;
        }

        // GET api/TicketAreaDiscounts
        public List<TicketAreaDiscountApiModel> GetTicketAreaDiscounts()
        {
            var items = repository.Items.Select(x => new TicketAreaDiscountApiModel
            {
                TicketDiscountID = x.TicketDiscountID,
                TicketAreaID = x.TicketAreaID,
                AdjustmentAmount = x.AdjustmentAmount,
                AdjustmentType = x.AdjustmentType
            }).ToList();

            return items;
        }

        // GET api/TicketAreaDiscounts/1/2
        //DISCOUNTID AREAID
        [ResponseType(typeof(TicketAreaDiscount))]
        public async Task<IHttpActionResult> GetTicketAreaDiscount(int id, int secondKey)
        {
            TicketAreaDiscount ticketAreaDiscount = await repository.RetrieveAsync(new int[] { id, secondKey });
            if (ticketAreaDiscount == null)
            {
                return NotFound();
            }

            TicketAreaDiscountApiModel model = new TicketAreaDiscountApiModel()
            {
                TicketDiscountID = ticketAreaDiscount.TicketDiscountID,
                TicketAreaID = ticketAreaDiscount.TicketAreaID,
                AdjustmentAmount = ticketAreaDiscount.AdjustmentAmount,
                AdjustmentType = ticketAreaDiscount.AdjustmentType
            };

            return Ok(model);
        }

        [ResponseType(typeof(List<TicketAreaDiscountApiModel>))]
        public IHttpActionResult GetTicketAreaDiscount(string bytype, int id)
        {
            List<TicketAreaDiscountApiModel> items = new List<TicketAreaDiscountApiModel>();

            if (bytype == "bydiscount")
            {
                items = repository.Items.Where(x => x.TicketDiscountID == id).Select(x => new TicketAreaDiscountApiModel
                {
                    TicketDiscountID = x.TicketDiscountID,
                    TicketAreaID = x.TicketAreaID,
                    AdjustmentType = x.AdjustmentType,
                    AdjustmentAmount = x.AdjustmentAmount
                }).ToList();
            }
            else
            {
                return NotFound();
            }

            return Ok(items);
        }


        // PUT api/TicketAreaDiscounts/5
        public async Task<IHttpActionResult> PutTicketAreaDiscount(int id, int secondKey, TicketAreaDiscount ticketAreaDiscount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ticketAreaDiscount.TicketDiscountID && secondKey != ticketAreaDiscount.TicketAreaID)
            {
                return BadRequest();
            }

            await repository.UpdateAsync(ticketAreaDiscount, new int[] { ticketAreaDiscount.TicketDiscountID, ticketAreaDiscount.TicketAreaID });

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/TicketAreaDiscounts
        [ResponseType(typeof(TicketAreaDiscount))]
        public async Task<IHttpActionResult> PostTicketAreaDiscount(TicketAreaDiscount ticketAreaDiscount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await repository.AddAsync(ticketAreaDiscount);

            return CreatedAtRoute("DefaultApi", new { id = ticketAreaDiscount.TicketDiscountID, secondKey = ticketAreaDiscount.TicketAreaID }, ticketAreaDiscount);
        }

        // DELETE api/TicketAreaDiscounts/5
        [ResponseType(typeof(TicketAreaDiscount))]
        public async Task<IHttpActionResult> DeleteTicketAreaDiscount(int id, int secondKey)
        {
            TicketAreaDiscount ticketAreaDiscount = await repository.RetrieveAsync(new int[] { id, secondKey });
            if (ticketAreaDiscount == null)
            {
                return NotFound();
            }

            await repository.DeleteAsync(ticketAreaDiscount);

            return Ok(ticketAreaDiscount);
        }

    }


}