using DexCMS.Tickets.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DexCMS.Tickets.Initializers.Helpers
{
    public class VenuesReference
    {
        public int Example { get; set; }

        public VenuesReference(IDexCMSTicketsContext Context)
        {
            Example = Context.Venues.Where(x => x.Name == "Example Venue").Select(x => x.VenueID).Single();
        }
    }
}
