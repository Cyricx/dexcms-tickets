using DexCMS.Tickets.Contexts;
using System.Linq;

namespace DexCMS.Tickets.Initializers.Helpers
{
    public class VenuesReference
    {
        public int Example { get; set; }

        public VenuesReference(IDexCMSTicketsContext Context)
        {
            Example = Context.Venues.Where(x => x.Name == "Example Venue").Select(x => x.VenueID).SingleOrDefault();
        }
    }
}
