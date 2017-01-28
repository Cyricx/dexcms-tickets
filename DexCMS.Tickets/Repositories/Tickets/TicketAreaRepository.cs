using DexCMS.Core.Repositories;
using DexCMS.Tickets.Tickets.Models;
using DexCMS.Tickets.Tickets.Interfaces;
using DexCMS.Tickets.Contexts;
using DexCMS.Core.Contexts;

namespace DexCMS.Tickets.Repositories.Tickets
{
    public class TicketAreaRepository : AbstractRepository<TicketArea>, ITicketAreaRepository
    {
        public override IDexCMSContext GetContext()
        {
            return _ctx;
        }

        private IDexCMSTicketsContext _ctx { get; set; }

        public TicketAreaRepository(IDexCMSTicketsContext ctx)
        {
            _ctx = ctx;
        }
    }
}
