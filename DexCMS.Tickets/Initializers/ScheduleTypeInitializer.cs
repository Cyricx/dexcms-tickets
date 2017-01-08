using DexCMS.Core.Infrastructure.Extensions;
using DexCMS.Core.Infrastructure.Globals;
using DexCMS.Tickets.Contexts;
using DexCMS.Tickets.Schedules.Models;

namespace DexCMS.Tickets.Initializers
{
    class ScheduleTypeInitializer : DexCMSInitializer<IDexCMSTicketsContext>
    {

        public ScheduleTypeInitializer(IDexCMSTicketsContext context) : base(context) {
        }

        public override void Run()
        {
            Context.ScheduleTypes.AddIfNotExists(x => x.Name,
                new ScheduleType { Name = "Concert", IsActive = true },
                new ScheduleType { Name = "Drawing", IsActive = true }
            );
            Context.SaveChanges();
        }
    }
}
