﻿using DexCMS.Core.Infrastructure.Repositories;
using DexCMS.Tickets.Tickets.Models;
using DexCMS.Tickets.Tickets.Interfaces;
using DexCMS.Tickets.Contexts;
using DexCMS.Core.Infrastructure.Contexts;

namespace DexCMS.Tickets.Repositories.Tickets
{
    public class TicketSeatRepository : AbstractRepository<TicketSeat>, ITicketSeatRepository
    {
        public override IDexCMSContext GetContext()
        {
            return _ctx;
        }

        private IDexCMSTicketsContext _ctx { get; set; }

        public TicketSeatRepository(IDexCMSTicketsContext ctx)
        {
            _ctx = ctx;
        }
    }
}
