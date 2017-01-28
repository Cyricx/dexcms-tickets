using DexCMS.Core.Infrastructure.Extensions;
using DexCMS.Core.Infrastructure.Globals;
using DexCMS.Tickets.Contexts;
using DexCMS.Tickets.Initializers.Helpers;
using DexCMS.Tickets.Venues.Models;

namespace DexCMS.Tickets.Initializers
{
    class VenueAreaInitializer : DexCMSInitializer<IDexCMSTicketsContext>
    {
        private VenuesReference Venues { get; set; }
        public VenueAreaInitializer(IDexCMSTicketsContext context) : base(context)
        {
            Venues = new VenuesReference(context);
        }
        public override void Run(bool addDemoContent = true)
        {
            if (addDemoContent)
            {
                Context.VenueAreas.AddIfNotExists(x => new { x.VenueID, x.Name },
                    new VenueArea { Name = "Balcony", IsGA = false, VenueID = Venues.Example },
                    new VenueArea { Name = "Lower Seating", IsGA = false, VenueID = Venues.Example },
                    new VenueArea { Name = "Floor", IsGA = true, VenueID = Venues.Example, GASeatCount = 20 }
                );
                Context.SaveChanges();
            }
        }
    }
}
