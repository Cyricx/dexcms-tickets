using DexCMS.Core.Infrastructure.Globals;
using DexCMS.Tickets.Contexts;

namespace DexCMS.Tickets.Initializers
{
    public class TicketsInitializer: DexCMSInitializer<IDexCMSTicketsContext>
    {
        public TicketsInitializer(IDexCMSTicketsContext context) : base(context) { }

        public override void Run()
        {
            (new VenueInitializer(Context)).Run();
            (new VenueAreaInitializer(Context)).Run();
            (new VenueSectionInitializer(Context)).Run();
            (new VenueRowInitializer(Context)).Run();
            (new VenueScheduleLocationInitializer(Context)).Run();
            (new ScheduleStatusInitializer(Context)).Run();
            (new ScheduleTypeInitializer(Context)).Run();
        }
    }
}
