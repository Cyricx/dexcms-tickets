﻿using DexCMS.Core.Infrastructure.Interfaces;
using DexCMS.Tickets.Tickets.Models;

namespace DexCMS.Tickets.Tickets.Interfaces
{
    public interface ITicketRepository : IRepository<Ticket>
    {
    }
}