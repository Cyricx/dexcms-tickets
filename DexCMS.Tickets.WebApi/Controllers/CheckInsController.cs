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
using DexCMS.Core.Globals;

namespace DexCMS.Tickets.WebApi.Controllers
{
    public class CheckInsController : ApiController
    {
        private IEventRepository eventRepository;
        private IEventSeriesRepository seriesRepository;
        private ITicketRepository ticketsRepository;
        private ApplicationUserManager _userManager;


        private int RegistrationExpirationMinutes = int.Parse(WebConfigurationManager.AppSettings["RegistrationExpirationMinutes"]);
        private DateTime cstTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Central Standard Time");

        public CheckInsController(IEventRepository eventRepo, 
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
        public IHttpActionResult CheckIns(string segment)
        {
            Event evt = RetrieveEvent(segment);
            if (evt == null)
            {
                return NotFound();
            }

            var users = UserManager.Users.ToDictionary(x => x.UserName.ToLower());

            var tickets = ticketsRepository.Items.Where(x => x.TicketSeat.TicketArea.EventID == evt.EventID).Select(x
                => new CheckInApiModel
                {
                    TicketID = x.TicketID,
                    FirstName = x.FirstName,
                    MiddleInitial = x.MiddleInitial,
                    LastName = x.LastName,
                    PreferredName = x.PreferredName,
                    HasArrived = x.ArrivalTime.HasValue,
                    ArrivalTime = x.ArrivalTime,
                    AgeGroup = x.TicketPrice.EventAgeGroup.Name,
                    DiscountName = x.TicketDiscountID.HasValue ? x.TicketDiscount.Name : null,
                    PurchasedBy = x.Order.UserName,
                    Seating = x.TicketSeat.TicketArea.Name + " " +
                        (x.TicketSeat.TicketRowID.HasValue ?
                        x.TicketSeat.TicketRow.TicketSection.Name + " " +
                        x.TicketSeat.TicketRow.Designation : ""),
                    Options = x.TicketOptionChoices.Select(y => y.TicketOption.Name + " " + y.Name).ToList(),
                    OrderID = x.OrderID,
                    OrderStatus = x.Order.OrderStatus.ToString(),
                    PaymentStatus = x.Order.Payments.FirstOrDefault().PaymentType.ToString()
                }).ToList();

            foreach (var item in tickets)
            {
                item.PurchasedBy = users[item.PurchasedBy.ToLower()].FirstName + " " + users[item.PurchasedBy.ToLower()].LastName;
            }

            return Ok(tickets);
        }

        [HttpPut]
        [ResponseType(typeof(CheckInApiModel))]
        public async Task<IHttpActionResult> CheckIns(string segment, int id, CheckInApiModel model)
        {
            var ticket = await ticketsRepository.RetrieveAsync(id);

            if (ticket == null || ticket.TicketID != model.TicketID)
            {
                return NotFound();
            }

            ticket.FirstName = model.FirstName;
            ticket.MiddleInitial = model.MiddleInitial;
            ticket.LastName = model.LastName;
            ticket.ArrivalTime = model.ArrivalTime;
            ticket.PreferredName = model.PreferredName;

            await ticketsRepository.UpdateAsync(ticket, ticket.TicketID);

            return Ok(model);
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