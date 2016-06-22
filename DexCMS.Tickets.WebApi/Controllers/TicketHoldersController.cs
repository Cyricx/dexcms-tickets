using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DexCMS.Tickets.Orders.Interfaces;
using DexCMS.Tickets.Tickets.Interfaces;
using DexCMS.Tickets.Tickets.Models;
using DexCMS.Tickets.WebApi.ApiModels;

namespace DexCMS.Tickets.WebApi.Controllers
{
    [Authorize]
    public class TicketHoldersController : ApiController
    {
        private IOrderRepository orderRepository;
        private ITicketRepository ticketRepository;

        public TicketHoldersController(IOrderRepository orderRepo, ITicketRepository ticketRepo)
        {
            orderRepository = orderRepo;
            ticketRepository = ticketRepo;
        }

        [HttpGet]
        [ResponseType(typeof(TicketHolderApiModel))]
        public async Task<IHttpActionResult> Get(int id)
        {
            var order = await orderRepository.RetrieveAsync(id);
            if (order == null || order.UserName != User.Identity.Name)
            {
                return NotFound();
            }

            TicketHolderApiModel response = new TicketHolderApiModel
            {
                OrderID = order.OrderID,
                TicketHolders = BuildTicketHolders(order.Tickets.ToList())
            };

            return Ok(response);
        }

        [HttpPut]
        [ResponseType(typeof(List<TicketHolderApiModel>))]
        public async Task<IHttpActionResult> Put(TicketHolderApiModel holderOrder, int id)
        {
            List<Ticket> updateTickets = new List<Ticket>();

            foreach (var item in holderOrder.TicketHolders)
            {
                updateTickets.Add(await UpdateTicketHolder(id, item));
            }

            return Ok(new TicketHolderApiModel
            {
                OrderID = id,
                TicketHolders = BuildTicketHolders(updateTickets)
            });
        }

        private async Task<Ticket> UpdateTicketHolder(int orderID, TicketSeatHolderApiModel item)
        {
            var ticket = await ticketRepository.RetrieveAsync(item.TicketID);
            if (ticket.OrderID != orderID)
            {
                return null;
            }

            ticket.FirstName = item.FirstName;
            ticket.MiddleInitial = item.MiddleInitial;
            ticket.LastName = item.LastName;
            //only update arrival if a cashier submitted this
            if (User.IsInRole("Cashier"))
            {
                ticket.ArrivalTime = item.ArrivalTime;
            }

            await ticketRepository.UpdateAsync(ticket, ticket.TicketID);

            return ticket;
        }

        private static List<TicketSeatHolderApiModel> BuildTicketHolders(List<Ticket> tickets)
        {
            var response = new List<TicketSeatHolderApiModel>();

            foreach (var x in tickets)
            {
                var ticket = new TicketSeatHolderApiModel
                {
                    AgeGroup = x.TicketPrice.EventAgeGroup.Name,
                    DiscountName = x.TicketDiscount?.Name,
                    FirstName = x.FirstName,
                    MiddleInitial = x.MiddleInitial,
                    LastName = x.LastName,
                    TicketID = x.TicketID,
                    TicketSeatID = x.TicketSeat.TicketSeatID,
                    Location = x.TicketSeat.TicketArea.Name,
                    HasArrived = x.ArrivalTime.HasValue,
                    ArrivalTime = x.ArrivalTime
                };

                if (x.TicketSeat.TicketRowID.HasValue)
                {
                    ticket.Location += " " +
                        x.TicketSeat.TicketRow.TicketSection.Name + " " +
                        x.TicketSeat.TicketRow.Designation + " " +
                        x.TicketSeat.SeatNumber;
                }

                response.Add(ticket);
            }

            return response;
        }
    }

}
