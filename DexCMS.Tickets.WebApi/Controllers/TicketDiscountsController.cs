using System;
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
    public class TicketDiscountsController : ApiController
    {
        private ITicketDiscountRepository repository;

        public TicketDiscountsController(ITicketDiscountRepository repo)
        {
            repository = repo;
        }

        // GET api/TicketDiscounts
        public List<TicketDiscountApiModel> GetTicketDiscounts()
        {
            var items = repository.Items.Select(x => new TicketDiscountApiModel
            {
                TicketDiscountID = x.TicketDiscountID,
                Code = x.Code,
                CutoffDate = x.CutoffDate,
                Description = x.Description,
                EventID = x.EventID,
                IsActive = x.IsActive,
                MaximumAvailable = x.MaximumAvailable,
                Name = x.Name,
                SecurityConfirmationNumber = x.SecurityConfirmationNumber
            }).ToList();

            return items;
        }

        // GET api/TicketDiscounts/5
        [ResponseType(typeof(TicketDiscount))]
        public async Task<IHttpActionResult> GetTicketDiscount(int id)
        {
            TicketDiscount ticketDiscount = await repository.RetrieveAsync(id);
            if (ticketDiscount == null)
            {
                return NotFound();
            }

            TicketDiscountApiModel model = new TicketDiscountApiModel()
            {
                TicketDiscountID = ticketDiscount.TicketDiscountID,
                AgeGroups = ticketDiscount.EventAgeGroups.Select(x => new TicketDiscountAgeApiModel
                {
                    Name = x.Name,
                    MaximumAge = x.MaximumAge,
                    MinimumAge = x.MinimumAge
                }).ToList(),
                cbEventAges = ticketDiscount.EventAgeGroups.Select(x => x.EventAgeGroupID).ToList(),
                Code = ticketDiscount.Code,
                CutoffDate = ticketDiscount.CutoffDate,
                Description = ticketDiscount.Description,
                EventID = ticketDiscount.EventID,
                IsActive = ticketDiscount.IsActive,
                MaximumAvailable = ticketDiscount.MaximumAvailable,
                Name = ticketDiscount.Name,
                TicketAreaDiscountCount = ticketDiscount.TicketAreaDiscounts.Count,
                TicketOptionDiscountCount = ticketDiscount.TicketOptionDiscounts.Count,
                TicketReservations = ticketDiscount.TicketSeats.Count(x => x.TicketSeatStatus == TicketSeatStatus.Reserved),
                TicketsClaimed = ticketDiscount.TicketSeats.Count(x => x.TicketSeatStatus == TicketSeatStatus.Assigned || x.TicketSeatStatus == TicketSeatStatus.Complete),
                TotalReservations = ticketDiscount.TicketSeats.Count(),
                SecurityConfirmationNumber = ticketDiscount.SecurityConfirmationNumber
            };

            return Ok(model);
        }

        [ResponseType(typeof(List<TicketDiscountApiModel>))]
        public IHttpActionResult GetTicketDiscount(string bytype, int id)
        {
            List<TicketDiscountApiModel> items = new List<TicketDiscountApiModel>();

            if (bytype == "byevent")
            {
                items = repository.Items.Where(x => x.EventID == id).Select(z => new TicketDiscountApiModel
                {
                    TicketDiscountID = z.TicketDiscountID,
                    AgeGroups = z.EventAgeGroups.Select(x => new TicketDiscountAgeApiModel
                    {
                        Name = x.Name,
                        MaximumAge = x.MaximumAge,
                        MinimumAge = x.MinimumAge
                    }).ToList(),
                    cbEventAges = z.EventAgeGroups.Select(x => x.EventAgeGroupID).ToList(),
                    Code = z.Code,
                    CutoffDate = z.CutoffDate,
                    Description = z.Description,
                    EventID = z.EventID,
                    IsActive = z.IsActive,
                    MaximumAvailable = z.MaximumAvailable,
                    Name = z.Name,
                    TicketAreaDiscountCount = z.TicketAreaDiscounts.Count,
                    TicketOptionDiscountCount = z.TicketOptionDiscounts.Count,
                    TicketReservations = z.TicketSeats.Count(x => x.TicketSeatStatus == TicketSeatStatus.Reserved),
                    TicketsClaimed = z.TicketSeats.Count(x => x.TicketSeatStatus == TicketSeatStatus.Assigned || x.TicketSeatStatus == TicketSeatStatus.Complete),
                    SecurityConfirmationNumber = z.SecurityConfirmationNumber
                }).ToList();
            }
            else
            {
                return NotFound();
            }

            return Ok(items);
        }


        // PUT api/TicketDiscounts/5
        public async Task<IHttpActionResult> PutTicketDiscount(int id, TicketDiscount ticketDiscount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ticketDiscount.TicketDiscountID)
            {
                return BadRequest();
            }

            await repository.UpdateAsync(ticketDiscount, ticketDiscount.TicketDiscountID);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/TicketDiscounts
        [ResponseType(typeof(TicketDiscount))]
        public async Task<IHttpActionResult> PostTicketDiscount(TicketDiscount ticketDiscount)
        {
            ticketDiscount.SecurityConfirmationNumber = Guid.NewGuid().ToString();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await repository.AddAsync(ticketDiscount);

            return CreatedAtRoute("DefaultApi", new { id = ticketDiscount.TicketDiscountID }, ticketDiscount);
        }

        // DELETE api/TicketDiscounts/5
        [ResponseType(typeof(TicketDiscount))]
        public async Task<IHttpActionResult> DeleteTicketDiscount(int id)
        {
            TicketDiscount ticketDiscount = await repository.RetrieveAsync(id);
            if (ticketDiscount == null)
            {
                return NotFound();
            }

            await repository.DeleteAsync(ticketDiscount);

            return Ok(ticketDiscount);
        }

    }



}