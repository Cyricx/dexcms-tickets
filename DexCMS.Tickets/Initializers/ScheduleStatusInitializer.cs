using DexCMS.Core.Infrastructure.Extensions;
using DexCMS.Core.Infrastructure.Globals;
using DexCMS.Tickets.Contexts;
using DexCMS.Tickets.Schedules.Models;

namespace DexCMS.Tickets.Initializers
{
    class ScheduleStatusInitializer : DexCMSInitializer<IDexCMSTicketsContext>
    {

        public ScheduleStatusInitializer(IDexCMSTicketsContext context) : base(context) {
        }

        public override void Run(bool addDemoContent = true)
        {
            Context.ScheduleStatuses.AddIfNotExists(x => x.Name,
                new ScheduleStatus { Name = "Tentative", IsActive = true },
                new ScheduleStatus { Name = "Confirmed", IsActive = true }
            );
            Context.SaveChanges();
        }
    }
}
