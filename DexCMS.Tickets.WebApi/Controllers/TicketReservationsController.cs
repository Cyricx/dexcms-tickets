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
    public class TicketReservationsController : ApiController
    {
        private ITicketDiscountRepository discountRepository;
        private ITicketSeatRepository seatRepository;

        public TicketReservationsController(ITicketDiscountRepository discountRepo, ITicketSeatRepository seatRepo)
        {
            discountRepository = discountRepo;
            seatRepository = seatRepo;
        }

        // GET api/TicketReservations/5
        [ResponseType(typeof(TicketReservationApiModel))]
        public async Task<IHttpActionResult> GetTicketReservation(int id)
        {
            TicketDiscount ticketDiscount = await discountRepository.RetrieveAsync(id);
            if (ticketDiscount == null)
            {
                return NotFound();
            }

            TicketReservationApiModel model = new TicketReservationApiModel()
            {
                TicketDiscountID = ticketDiscount.TicketDiscountID,
                Name = ticketDiscount.Name,
                MaximumAvailable = ticketDiscount.MaximumAvailable,
                TotalReservations = ticketDiscount.TicketSeats.Count(),
                TicketAreas = ticketDiscount.Event.TicketAreas.Select(a => new ReservationAreaApiModel
                {
                    TicketAreaID = a.TicketAreaID,
                    Name = a.Name,
                    IsGA = a.IsGA,
                    MaxCapacity = a.TicketSeats.Count,
                    Assigned = a.TicketSeats.Count(ts => (ts.TicketSeatStatus == TicketSeatStatus.Assigned || ts.TicketSeatStatus == TicketSeatStatus.Complete) && ts.TicketDiscountID != id),
                    DiscountAssigned = a.TicketSeats.Count(ts => (ts.TicketSeatStatus == TicketSeatStatus.Assigned || ts.TicketSeatStatus == TicketSeatStatus.Complete) && ts.TicketDiscountID == id),
                    UnclaimedReservations = a.TicketSeats.Count(ts => ts.TicketSeatStatus == TicketSeatStatus.Reserved && ts.TicketDiscountID != id),
                    DiscountReservations = a.TicketSeats.Count(ts => ts.TicketSeatStatus == TicketSeatStatus.Reserved 
                            && ts.TicketDiscountID == id && (!ts.PendingPurchaseExpiration.HasValue || ts.PendingPurchaseExpiration < DateTime.Now)),
                    PendingDiscount = a.TicketSeats.Count(ts => ts.TicketSeatStatus == TicketSeatStatus.Reserved
                            && ts.TicketDiscountID == id && (ts.PendingPurchaseExpiration.HasValue && ts.PendingPurchaseExpiration >= DateTime.Now)),
                    Unavailable = a.TicketSeats.Count(ts => ts.TicketSeatStatus == TicketSeatStatus.Disabled),
                    Available = a.TicketSeats.Count(ts => ts.TicketSeatStatus == TicketSeatStatus.Available && 
                        (!ts.PendingPurchaseExpiration.HasValue || ts.PendingPurchaseExpiration < DateTime.Now)),
                    PendingPurchase = a.TicketSeats.Count(ts => ts.TicketSeatStatus == TicketSeatStatus.Available &&
                        (ts.PendingPurchaseExpiration.HasValue && ts.PendingPurchaseExpiration >= DateTime.Now)),
                    TicketSections = a.TicketSections.Select(s => new ReservationSectionApiModel
                    {
                        Name = s.Name,
                        TicketSectionID = s.TicketSectionID,
                        TicketRows = s.TicketRows.Select(r => new ReservationRowApiModel
                        {
                            Designation = r.Designation,
                            TicketRowID = r.TicketRowID,
                            MaxCapacity = r.TicketSeats.Count,
                            Assigned = r.TicketSeats.Count(ts => (ts.TicketSeatStatus == TicketSeatStatus.Assigned || ts.TicketSeatStatus == TicketSeatStatus.Complete) && ts.TicketDiscountID != id),
                            DiscountAssigned = r.TicketSeats.Count(ts => (ts.TicketSeatStatus == TicketSeatStatus.Assigned || ts.TicketSeatStatus == TicketSeatStatus.Complete) 
                                && ts.TicketDiscountID == id && (!ts.PendingPurchaseExpiration.HasValue || ts.PendingPurchaseExpiration < DateTime.Now)),
                            PendingDiscount = r.TicketSeats.Count(ts => ts.TicketSeatStatus == TicketSeatStatus.Reserved
                                    && ts.TicketDiscountID == id && (ts.PendingPurchaseExpiration.HasValue && ts.PendingPurchaseExpiration >= DateTime.Now)),
                            UnclaimedReservations = r.TicketSeats.Count(ts => ts.TicketSeatStatus == TicketSeatStatus.Reserved && ts.TicketDiscountID != id),
                            DiscountReservations = r.TicketSeats.Count(ts => ts.TicketSeatStatus == TicketSeatStatus.Reserved && ts.TicketDiscountID == id),
                            Unavailable = r.TicketSeats.Count(ts => ts.TicketSeatStatus == TicketSeatStatus.Disabled),
                            Available = r.TicketSeats.Count(ts => ts.TicketSeatStatus == TicketSeatStatus.Available &&
                                (!ts.PendingPurchaseExpiration.HasValue || ts.PendingPurchaseExpiration < DateTime.Now)),
                            PendingPurchase = r.TicketSeats.Count(ts => ts.TicketSeatStatus == TicketSeatStatus.Available &&
                                (ts.PendingPurchaseExpiration.HasValue && ts.PendingPurchaseExpiration >= DateTime.Now)),
                        }).ToList()
                    }).ToList()
                }).ToList()
            };

            return Ok(model);
        }
        // POST api/TicketReservations
        [ResponseType(typeof(ReservationSeatApiModel))]
        public async Task<IHttpActionResult> PostTicketReservation(ReservationSeatApiModel reservationSeats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //first load modified
            List<TicketSeat> seatsToChange = new List<TicketSeat>();
            var ALLSEATS = from s in seatRepository.Items
                                              where (reservationSeats.TicketRowID.HasValue && s.TicketRowID == reservationSeats.TicketRowID)
                                              || (reservationSeats.TicketAreaID.HasValue && s.TicketAreaID == reservationSeats.TicketAreaID)
                                              select s;


            var currentSeats = ALLSEATS.Where(x => 
                x.TicketSeatStatus == TicketSeatStatus.Disabled || 
                    (x.TicketSeatStatus == TicketSeatStatus.Reserved 
                    && x.TicketDiscountID == reservationSeats.TicketDiscountID)).ToList();
            seatsToChange.AddRange(currentSeats);

            int difference = (reservationSeats.Unavailable + reservationSeats.DiscountReservations) - currentSeats.Count;
            //now, find out if we need more
            if (difference > 0)
            {
                //find and get more seats
                var seatsToAdd = ALLSEATS.Where(x => x.TicketSeatStatus == TicketSeatStatus.Available && 
                    (!x.PendingPurchaseExpiration.HasValue || x.PendingPurchaseExpiration < DateTime.Now)).Take(difference).ToList();
                currentSeats.AddRange(seatsToAdd);
            }

            //set ALL to available and NO discount
            foreach (var item in currentSeats)
            {
                item.TicketDiscountID = null;
                item.TicketSeatStatus = TicketSeatStatus.Available;
                if (reservationSeats.Unavailable > 0)
                {
                    item.TicketSeatStatus = TicketSeatStatus.Disabled;
                    reservationSeats.Unavailable--;
                }
                else if (reservationSeats.DiscountReservations > 0)
                {
                    item.TicketDiscountID = reservationSeats.TicketDiscountID;
                    item.TicketSeatStatus = TicketSeatStatus.Reserved;
                    reservationSeats.DiscountReservations--;
                }

                await seatRepository.UpdateAsync(item, item.TicketSeatID);

            }

            return StatusCode(HttpStatusCode.NoContent);
        }


    }



}