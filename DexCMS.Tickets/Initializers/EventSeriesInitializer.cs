using DexCMS.Core.Extensions;
using DexCMS.Core.Globals;
using DexCMS.Core.Initializers.Helpers;
using DexCMS.Tickets.Contexts;
using DexCMS.Tickets.Events.Models;
using DexCMS.Tickets.Venues.Models;

namespace DexCMS.Tickets.Initializers
{
    class EventSeriesInitializer : DexCMSInitializer<IDexCMSTicketsContext>
    {

        public EventSeriesInitializer(IDexCMSTicketsContext context) : base(context) {
        }

        public override void Run(bool addDemoContent = true)
        {
            if (addDemoContent)
            {
                Context.EventSeries.AddIfNotExists(x => x.SeriesName,
                    new EventSeries { SeriesName = "Summer Rockin", AllowMultiplePublic = false, IsActive = true, SeriesUrlSegment = "summer-rockin" }
                );
                Context.SaveChanges();
            }
        }
    }
}
