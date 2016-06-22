using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DexCMS.Core.Infrastructure.Repositories;
using DexCMS.Tickets.Orders.Models;
using DexCMS.Tickets.Orders.Interfaces;
using System.Data.Entity;
using DexCMS.Tickets.Contexts;
using DexCMS.Core.Infrastructure.Contexts;
using DexCMS.Tickets.Tickets.Models;

namespace DexCMS.Tickets.Repositories.Orders
{
    public class OrderRepository : AbstractRepository<Order>, IOrderRepository
    {
        public override IDexCMSContext GetContext()
        {
            return _ctx;
        }

        private IDexCMSTicketsContext _ctx { get; set; }

        public OrderRepository(IDexCMSTicketsContext ctx)
        {
            _ctx = ctx;
        }

        public IQueryable<Order> RetrieveUserOrders(string userName)
        {
            return _ctx.Orders.Where(x => x.UserName == userName);
        }

        public override Task<int> DeleteAsync(Order item)
        {
            var tickets = _ctx.Tickets.Where(x => x.OrderID == item.OrderID).ToList();
            var ticketSeats = tickets.Select(x => x.TicketSeat).ToList();
            foreach (var ticket in tickets)
            {
                _ctx.Entry(ticket).State = EntityState.Deleted;
            }
            foreach (var ticketSeat in ticketSeats)
            {
                _ctx.Entry(ticketSeat).State = EntityState.Modified;
                ticketSeat.TicketSeatStatus = ticketSeat.PreviousTicketSeatStatus;
            }
            _ctx.Entry(item).State = EntityState.Modified;

            item.Tickets.Clear();

            return base.DeleteAsync(item);
        }

        public override Task<int> AddAsync(Order item)
        {
            DateTime now = DateTime.Now;

            //build tickets
            item.Tickets = new List<Ticket>();
            foreach (OrderTicketReference ticketRef in item.OrderTicketReferences)
            {
                Ticket ticket = new Ticket
                {
                    TicketTotalPrice = ticketRef.TotalPrice,
                    TicketID = ticketRef.TicketSeatID
                };

                //retrieve seat
                TicketSeat seat = _ctx.TicketSeats.Find(ticketRef.TicketSeatID);

                //verify confirmation
                if (seat != null && seat.PendingPurchaseConfirmation == ticketRef.PendingPurchaseConfirmation)
                {
                    //retrieve the price
                    TicketPrice price = _ctx.TicketPrices.Where(x =>
                        x.TicketCutoff.OnSellDate <= now
                        && x.TicketCutoff.CutoffDate >= now
                        && x.EventAgeGroupID == ticketRef.EventAgeGroupID
                        && x.TicketAreaID == seat.TicketAreaID).SingleOrDefault();
                    if (price != null)
                    {
                        ticket.TicketPriceID = price.TicketPriceID;

                        //check for a discount
                        if (ticketRef.TicketDiscountID.HasValue)
                        {
                            TicketDiscount discount = _ctx.TicketDiscounts.Find(ticketRef.TicketDiscountID.Value);
                            if (discount != null && discount.SecurityConfirmationNumber == ticketRef.SecurityConfirmationNumber)
                            {
                                ticket.TicketDiscountID = discount.TicketDiscountID;
                            }
                        }

                        //process options
                        if (ticketRef.TicketOptionChoices != null && ticketRef.TicketOptionChoices.Count > 0)
                        {
                            ticket.TicketOptionChoices = new List<TicketOptionChoice>();

                            foreach (var choiceID in ticketRef.TicketOptionChoices)
                            {
                                TicketOptionChoice choice = _ctx.TicketOptionChoices.Find(choiceID);
                                if (choice != null)
                                {
                                    ticket.TicketOptionChoices.Add(choice);
                                }
                            }

                        }
                        item.Tickets.Add(ticket);
                        seat.PreviousTicketSeatStatus = seat.TicketSeatStatus;
                        seat.TicketSeatStatus = TicketSeatStatus.Assigned;
                        _ctx.Entry(seat).State = EntityState.Modified;

                    }
                }
            }

            //set total
            item.OrderTotal = item.Tickets.Sum(x => x.TicketTotalPrice);

            return base.AddAsync(item);
        }
    }
}