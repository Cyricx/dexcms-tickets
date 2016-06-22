using DexCMS.Core.Infrastructure.Repositories;
using DexCMS.Tickets.Schedules.Models;
using DexCMS.Tickets.Schedules.Interfaces;
using DexCMS.Tickets.Contexts;
using DexCMS.Core.Infrastructure.Contexts;

namespace DexCMS.Tickets.Repositories.Schedules
{
    public class ScheduleTypeRepository : AbstractRepository<ScheduleType>, IScheduleTypeRepository
    {
        public override IDexCMSContext GetContext()
        {
            return _ctx;
        }

        private IDexCMSTicketsContext _ctx { get; set; }

        public ScheduleTypeRepository(IDexCMSTicketsContext ctx)
        {
            _ctx = ctx;
        }

    }
}
