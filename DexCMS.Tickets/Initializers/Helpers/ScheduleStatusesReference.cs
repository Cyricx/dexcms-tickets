using DexCMS.Tickets.Contexts;
using System.Linq;

namespace DexCMS.Tickets.Initializers.Helpers
{
    public class ScheduleStatusesReference
    {
        public int Tentative { get; set; }
        public int Confirmed { get; set; }

        public ScheduleStatusesReference(IDexCMSTicketsContext Context)
        {
            Tentative = Context.ScheduleStatuses.Where(x => x.Name == "Tentative").Select(x => x.ScheduleStatusID).Single();
            Confirmed = Context.ScheduleStatuses.Where(x => x.Name == "Confirmed").Select(x => x.ScheduleStatusID).Single();
        }
    }
}
