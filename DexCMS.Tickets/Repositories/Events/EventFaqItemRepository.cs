using DexCMS.Core.Repositories;
using DexCMS.Tickets.Events.Models;
using DexCMS.Tickets.Events.Interfaces;
using DexCMS.Tickets.Contexts;
using DexCMS.Core.Contexts;

namespace DexCMS.Tickets.Repositories.Events
{
    public class EventFaqItemRepository : AbstractRepository<EventFaqItem>, IEventFaqItemRepository
    {
        public override IDexCMSContext GetContext()
        {
            return _ctx;
        }

        private IDexCMSTicketsContext _ctx { get; set; }

        public EventFaqItemRepository(IDexCMSTicketsContext ctx)
        {
            _ctx = ctx;
        }
    }
}