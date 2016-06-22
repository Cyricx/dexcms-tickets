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
    public class TicketAssignmentsController: ApiController
    {
        private ITicketRepository ticketRepository;
        private ITicketSeatRepository ticketSeatRepository;

        public TicketAssignmentsController(ITicketRepository ticketRepo, ITicketSeatRepository ticketSeatRepo)
        {
            ticketRepository = ticketRepo;
            ticketSeatRepository = ticketSeatRepo;
        }


    }
}
