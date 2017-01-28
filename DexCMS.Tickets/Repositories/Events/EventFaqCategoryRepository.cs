using DexCMS.Core.Repositories;
using DexCMS.Tickets.Events.Models;
using DexCMS.Tickets.Events.Interfaces;
using DexCMS.Tickets.Contexts;
using DexCMS.Core.Contexts;

namespace DexCMS.Tickets.Repositories.Events
{
    public class EventFaqCategoryRepository : AbstractRepository<EventFaqCategory>, IEventFaqCategoryRepository
    {
        public override IDexCMSContext GetContext()
        {
            return _ctx;
        }

        private IDexCMSTicketsContext _ctx { get; set; }

        public EventFaqCategoryRepository(IDexCMSTicketsContext ctx)
        {
            _ctx = ctx;
        }
    }
}