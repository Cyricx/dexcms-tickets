using DexCMS.Core.Repositories;
using DexCMS.Tickets.Schedules.Models;
using DexCMS.Tickets.Schedules.Interfaces;
using DexCMS.Tickets.Contexts;
using DexCMS.Core.Contexts;

namespace DexCMS.Tickets.Repositories.Schedules
{
    public class ScheduleItemRepository : AbstractRepository<ScheduleItem>, IScheduleItemRepository
    {
        public override IDexCMSContext GetContext()
        {
            return _ctx;
        }

        private IDexCMSTicketsContext _ctx { get; set; }

        public ScheduleItemRepository(IDexCMSTicketsContext ctx)
        {
            _ctx = ctx;
        }

    }
}
