using DexCMS.Tickets.Contexts;
using System.Linq;

namespace DexCMS.Tickets.Initializers.Helpers
{
    public class VenueAreasReference
    {
        public int Balcony { get; set; }
        public int LowerSeating { get; set; }
        private VenuesReference Venues { get; set; }
        public VenueAreasReference(IDexCMSTicketsContext Context)
        {
            Venues = new VenuesReference(Context);
            Balcony = Context.VenueAreas.Where(x => x.Name == "Balcony" && x.VenueID == Venues.Example).Select(x => x.VenueAreaID).SingleOrDefault();
            LowerSeating = Context.VenueAreas.Where(x => x.Name == "Lower Seating" && x.VenueID == Venues.Example).Select(x => x.VenueAreaID).SingleOrDefault();
        }
    }
}
