using System;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using DexCMS.Tickets.Events.Interfaces;
using DexCMS.Tickets.Events.Models;
using DexCMS.Tickets.Tickets.Interfaces;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Description;
using DexCMS.Tickets.WebApi.ApiModels;
using DexCMS.Tickets.Tickets.Models;
using System.Threading.Tasks;
using DexCMS.Core.Infrastructure.Globals;

namespace DexCMS.Tickets.WebApi.Controllers
{
    public class ReportingTicketsController : ApiController
    {
        private IEventRepository eventRepository;
        private IEventSeriesRepository seriesRepository;
        private ITicketRepository ticketsRepository;
        private ApplicationUserManager _userManager;

        private DateTime cstTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Central Standard Time");

        public ReportingTicketsController(IEventRepository eventRepo, 
            IEventSeriesRepository seriesRepo, 
            ITicketRepository ticketsRepo)
        {
            eventRepository = eventRepo;
            seriesRepository = seriesRepo;
            ticketsRepository = ticketsRepo;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpGet]
        [ResponseType(typeof(List<CheckInApiModel>))]
        public IHttpActionResult ReportingTickets(int id)
        {
            var users = UserManager.Users.ToList();

            var tickets = ticketsRepository.Items.Where(x => x.TicketSeat.TicketArea.EventID == id).Select(x
                => new ReportingTicketApiModel
                {
                    OrderID = x.OrderID,
                    TicketID = x.TicketID,
                    UserName = x.Order.UserName,
                    FirstName = x.FirstName,
                    MiddleInitial = x.MiddleInitial,
                    LastName = x.LastName,
                    TicketAreaName = x.TicketSeat.TicketArea.Name,
                    TicketSectionName = x.TicketSeat.TicketRowID.HasValue ? x.TicketSeat.TicketRow.TicketSection.Name : null,
                    TicketRowDesignation = x.TicketSeat.TicketRowID.HasValue ? x.TicketSeat.TicketRow.Designation : null,
                    TicketDiscountName = x.TicketDiscountID.HasValue ? x.TicketDiscount.Name : null,
                    TicketDiscountCode  = x.TicketDiscountID.HasValue ? x.TicketDiscount.Code : null,
                    TicketTotalPrice = x.TicketTotalPrice,
                    OrderTotal = x.Order.OrderTotal,
                    ArrivalTime = x.ArrivalTime,
                    Options = x.TicketOptionChoices.Select(y => new ReportingTicketOptionApiModel
                    {
                        OptionName = y.TicketOption.Name,
                        OptionChoiceName = y.Name
                    }).ToList()
                }).ToList();

            foreach (var item in tickets)
            {
                var user = users.Where(x => x.Email == item.UserName).SingleOrDefault();
                if (user != null)
                {
                    item.PreferredName = user.PreferredName;
                }
            }

            return Ok(tickets);
        }

        private Event RetrieveEvent(string segment)
        {
            //check for series event
            var evt = seriesRepository.RetrievePublicSingle(segment);

            if (evt == null)
            {
                //check for regular event
                evt = eventRepository.RetrieveByUrlSegment(segment);
            }

            return evt;
        }

    }


}