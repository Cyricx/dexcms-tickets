using DexCMS.Core.Extensions;
using DexCMS.Core.Globals;
using DexCMS.Tickets.Contexts;
using DexCMS.Tickets.Initializers.Helpers;
using DexCMS.Tickets.Venues.Models;

namespace DexCMS.Tickets.Initializers
{
    class VenueRowInitializer : DexCMSInitializer<IDexCMSTicketsContext>
    {
        private VenueSectionsReference VenueSections { get; set; }
        public VenueRowInitializer(IDexCMSTicketsContext context) : base(context)
        {
            VenueSections = new VenueSectionsReference(context);
        }
        public override void Run(bool addDemoContent = true)
        {
            if (addDemoContent)
            {
                Context.VenueRows.AddIfNotExists(x => new { x.VenueSectionID, x.Designation },
                    new VenueRow { Designation = "A", SeatCount = 10, VenueSectionID = VenueSections.BalconyL },
                    new VenueRow { Designation = "B", SeatCount = 10, VenueSectionID = VenueSections.BalconyL },
                    new VenueRow { Designation = "A", SeatCount = 10, VenueSectionID = VenueSections.BalconyM },
                    new VenueRow { Designation = "B", SeatCount = 10, VenueSectionID = VenueSections.BalconyM },
                    new VenueRow { Designation = "A", SeatCount = 10, VenueSectionID = VenueSections.BalconyR },
                    new VenueRow { Designation = "B", SeatCount = 10, VenueSectionID = VenueSections.BalconyR },
                    new VenueRow { Designation = "A", SeatCount = 10, VenueSectionID = VenueSections.LowerSeatingL },
                    new VenueRow { Designation = "B", SeatCount = 10, VenueSectionID = VenueSections.LowerSeatingL },
                    new VenueRow { Designation = "A", SeatCount = 10, VenueSectionID = VenueSections.LowerSeatingM },
                    new VenueRow { Designation = "B", SeatCount = 10, VenueSectionID = VenueSections.LowerSeatingM },
                    new VenueRow { Designation = "A", SeatCount = 10, VenueSectionID = VenueSections.LowerSeatingR },
                    new VenueRow { Designation = "B", SeatCount = 10, VenueSectionID = VenueSections.LowerSeatingR }
                );
                Context.SaveChanges();
            }
        }
    }
}
