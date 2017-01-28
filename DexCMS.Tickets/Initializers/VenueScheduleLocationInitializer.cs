using DexCMS.Core.Infrastructure.Extensions;
using DexCMS.Core.Infrastructure.Globals;
using DexCMS.Tickets.Contexts;
using DexCMS.Tickets.Initializers.Helpers;
using DexCMS.Tickets.Venues.Models;

namespace DexCMS.Tickets.Initializers
{
    class VenueScheduleLocationInitializer : DexCMSInitializer<IDexCMSTicketsContext>
    {
        private VenuesReference Venues { get; set; }
        public VenueScheduleLocationInitializer(IDexCMSTicketsContext context) : base(context)
        {
            Venues = new VenuesReference(context);
        }
        public override void Run(bool addDemoContent = true)
        {
            if (addDemoContent)
            {
                Context.VenueScheduleLocations.AddIfNotExists(x => new { x.VenueID, x.Name },
                    new VenueScheduleLocation { Name = "Stage", IsActive = true, VenueID = Venues.Example }
                );
                Context.SaveChanges();
            }
        }
    }
}
