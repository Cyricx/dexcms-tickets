using DexCMS.Core.Contexts;
using DexCMS.Core.Repositories;
using DexCMS.Tickets.Contexts;
using DexCMS.Tickets.Tickets.Interfaces;
using DexCMS.Tickets.Tickets.Models;

namespace DexCMS.Tickets.Repositories.Tickets
{
    public class TicketPriceRepository : AbstractRepository<TicketPrice>, ITicketPriceRepository
    {
        public override IDexCMSContext GetContext()
        {
            return _ctx;
        }

        private IDexCMSTicketsContext _ctx { get; set; }

        public TicketPriceRepository(IDexCMSTicketsContext ctx)
        {
            _ctx = ctx;
        }
    }
}
