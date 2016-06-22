using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using DexCMS.Tickets.Events.Interfaces;
using DexCMS.Tickets.Tickets.Interfaces;
using DexCMS.Tickets.Tickets.Models;
using DexCMS.Tickets.WebApi.ApiModels;

namespace DexCMS.Tickets.WebApi.Controllers
{
    [Authorize(Roles="Admin")]
    public class TicketsController: ApiController
    {
        private IEventRepository eventRepository;
        private ITicketRepository ticketRepository;

        public TicketsController(IEventRepository eventRepo, ITicketRepository ticketRepo)
        {
            eventRepository = eventRepo;
            ticketRepository = ticketRepo;
        }

        public List<EventApiModel> Get()
        {
            List<EventApiModel> items = new List<EventApiModel>();

            foreach (var evt in eventRepository.Items.ToList())
            {
                EventApiModel item = new EventApiModel()
                {
                    EventID = evt.EventID,
                    PageContentHeading = evt.PageContent.Heading,
                    EventSeriesName = evt.EventSeries.SeriesName,
                    AvailableCount = 0,
                    DisabledCount = 0,
                    ReservedCount = 0,
                    AssignedCount = 0,
                    CompleteCount = 0
                };

                foreach (var area in evt.TicketAreas.ToList())
                {
                    item.AvailableCount += area.TicketSeats.Count(x => x.TicketSeatStatus == TicketSeatStatus.Available);
                    item.DisabledCount += area.TicketSeats.Count(x => x.TicketSeatStatus == TicketSeatStatus.Disabled);
                    item.ReservedCount += area.TicketSeats.Count(x => x.TicketSeatStatus == TicketSeatStatus.Reserved);
                    item.AssignedCount += area.TicketSeats.Count(x => x.TicketSeatStatus == TicketSeatStatus.Assigned);
                    item.CompleteCount += area.TicketSeats.Count(x => x.TicketSeatStatus == TicketSeatStatus.Complete);
                }

                items.Add(item);
            }

            return items;
        }
    }
}
