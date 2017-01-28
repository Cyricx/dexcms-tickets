using DexCMS.Core.Repositories;
using DexCMS.Tickets.Venues.Models;
using DexCMS.Tickets.Venues.Interfaces;
using DexCMS.Tickets.Contexts;
using DexCMS.Core.Contexts;

namespace DexCMS.Tickets.Repositories.Venues
{
    public class VenueAreaRepository : AbstractRepository<VenueArea>, IVenueAreaRepository
    {
        public override IDexCMSContext GetContext()
        {
            return _ctx;
        }

        private IDexCMSTicketsContext _ctx { get; set; }

        public VenueAreaRepository(IDexCMSTicketsContext ctx)
        {
            _ctx = ctx;
        }
    }
}
