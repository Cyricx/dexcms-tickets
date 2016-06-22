using DexCMS.Core.Infrastructure.Repositories;
using DexCMS.Tickets.Venues.Models;
using DexCMS.Tickets.Venues.Interfaces;
using DexCMS.Tickets.Contexts;
using DexCMS.Core.Infrastructure.Contexts;

namespace DexCMS.Tickets.Repositories.Venues
{
    public class VenueRowRepository : AbstractRepository<VenueRow>, IVenueRowRepository
    {
        public override IDexCMSContext GetContext()
        {
            return _ctx;
        }

        private IDexCMSTicketsContext _ctx { get; set; }

        public VenueRowRepository(IDexCMSTicketsContext ctx)
        {
            _ctx = ctx;
        }
    }
}
