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
    public class TicketOptionDiscountsController : ApiController
    {
        private ITicketOptionDiscountRepository repository;

        public TicketOptionDiscountsController(ITicketOptionDiscountRepository repo)
        {
            repository = repo;
        }

        // GET api/TicketOptionDiscounts
        public List<TicketOptionDiscountApiModel> GetTicketOptionDiscounts()
        {
            var items = repository.Items.Select(x => new TicketOptionDiscountApiModel
            {
                TicketDiscountID = x.TicketDiscountID,
                TicketOptionID = x.TicketOptionID,
                AdjustmentAmount = x.AdjustmentAmount,
                AdjustmentType = x.AdjustmentType
            }).ToList();

            return items;
        }

        // GET api/TicketOptionDiscounts/1/2
        //DISCOUNTID then OPTIONID
        [ResponseType(typeof(TicketOptionDiscount))]
        public async Task<IHttpActionResult> GetTicketOptionDiscount(int id, int secondKey)
        {
            TicketOptionDiscount ticketOptionDiscount = await repository.RetrieveAsync(new int[] { id, secondKey });
            if (ticketOptionDiscount == null)
            {
                return NotFound();
            }

            TicketOptionDiscountApiModel model = new TicketOptionDiscountApiModel()
            {
                TicketDiscountID = ticketOptionDiscount.TicketDiscountID,
                TicketOptionID = ticketOptionDiscount.TicketOptionID,
                AdjustmentType = ticketOptionDiscount.AdjustmentType,
                AdjustmentAmount = ticketOptionDiscount.AdjustmentAmount
            };

            return Ok(model);
        }

        [ResponseType(typeof(List<TicketOptionDiscountApiModel>))]
        public IHttpActionResult GetTicketOptionDiscount(string bytype, int id)
        {
            List<TicketOptionDiscountApiModel> items = new List<TicketOptionDiscountApiModel>();

            if (bytype == "bydiscount")
            {
                items = repository.Items.Where(x => x.TicketDiscountID == id).Select(x => new TicketOptionDiscountApiModel
                {
                    TicketDiscountID = x.TicketDiscountID,
                    TicketOptionID = x.TicketOptionID,
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


        // PUT api/TicketOptionDiscounts/5
        public async Task<IHttpActionResult> PutTicketOptionDiscount(int id, int secondKey, TicketOptionDiscount ticketOptionDiscount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ticketOptionDiscount.TicketDiscountID && secondKey != ticketOptionDiscount.TicketOptionID)
            {
                return BadRequest();
            }

            await repository.UpdateAsync(ticketOptionDiscount, 
                new[] { ticketOptionDiscount.TicketDiscountID, ticketOptionDiscount.TicketOptionID });

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/TicketOptionDiscounts
        [ResponseType(typeof(TicketOptionDiscount))]
        public async Task<IHttpActionResult> PostTicketOptionDiscount(TicketOptionDiscount ticketOptionDiscount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await repository.AddAsync(ticketOptionDiscount);

            return CreatedAtRoute("DefaultApi", 
                new { id = ticketOptionDiscount.TicketDiscountID, secondKey = ticketOptionDiscount.TicketOptionID }, ticketOptionDiscount);
        }

        // DELETE api/TicketOptionDiscounts/5
        [ResponseType(typeof(TicketOptionDiscount))]
        public async Task<IHttpActionResult> DeleteTicketOptionDiscount(int id, int secondKey)
        {
            TicketOptionDiscount ticketOptionDiscount = await repository.RetrieveAsync(new int[] { id, secondKey });
            if (ticketOptionDiscount == null)
            {
                return NotFound();
            }

            await repository.DeleteAsync(ticketOptionDiscount);

            return Ok(ticketOptionDiscount);
        }

    }

}