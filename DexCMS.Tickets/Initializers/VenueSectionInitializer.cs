using DexCMS.Core.Infrastructure.Extensions;
using DexCMS.Core.Infrastructure.Globals;
using DexCMS.Tickets.Contexts;
using DexCMS.Tickets.Initializers.Helpers;
using DexCMS.Tickets.Venues.Models;

namespace DexCMS.Tickets.Initializers
{
    class VenueSectionInitializer : DexCMSInitializer<IDexCMSTicketsContext>
    {
        private VenueAreasReference VenueAreas { get; set; }
        public VenueSectionInitializer(IDexCMSTicketsContext context) : base(context)
        {
            VenueAreas = new VenueAreasReference(context);
        }
        public override void Run(bool addDemoContent = true)
        {
            if (addDemoContent)
            {
                Context.VenueSections.AddIfNotExists(x => new { x.VenueAreaID, x.Name },
                    new VenueSection { Name = "L", VenueAreaID = VenueAreas.Balcony },
                    new VenueSection { Name = "M", VenueAreaID = VenueAreas.Balcony },
                    new VenueSection { Name = "R", VenueAreaID = VenueAreas.Balcony },
                    new VenueSection { Name = "L", VenueAreaID = VenueAreas.LowerSeating },
                    new VenueSection { Name = "M", VenueAreaID = VenueAreas.LowerSeating },
                    new VenueSection { Name = "R", VenueAreaID = VenueAreas.LowerSeating }
                );
                Context.SaveChanges();
            }
        }
    }
}
