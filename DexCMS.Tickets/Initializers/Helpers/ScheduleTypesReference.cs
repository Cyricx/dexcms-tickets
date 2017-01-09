using DexCMS.Tickets.Contexts;
using System.Linq;

namespace DexCMS.Tickets.Initializers.Helpers
{
    public class ScheduleTypesReference
    {
        public int Concert { get; set; }
        public int Drawing { get; set; }

        public ScheduleTypesReference(IDexCMSTicketsContext Context)
        {
            Concert = Context.ScheduleTypes.Where(x => x.Name == "Concert").Select(x => x.ScheduleTypeID).Single();
            Drawing = Context.ScheduleTypes.Where(x => x.Name == "Drawing").Select(x => x.ScheduleTypeID).Single();
        }
    }
}
