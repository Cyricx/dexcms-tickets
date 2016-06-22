using DexCMS.Core.Infrastructure.Repositories;
using DexCMS.Tickets.Venues.Models;
using DexCMS.Tickets.Venues.Interfaces;
using DexCMS.Tickets.Contexts;
using DexCMS.Core.Infrastructure.Contexts;

namespace DexCMS.Tickets.Repositories.Venues
{
    public class VenueScheduleLocationRepository : AbstractRepository<VenueScheduleLocation>, IVenueScheduleLocationRepository
    {
        public override IDexCMSContext GetContext()
        {
            return _ctx;
        }

        private IDexCMSTicketsContext _ctx { get; set; }

        public VenueScheduleLocationRepository(IDexCMSTicketsContext ctx)
        {
            _ctx = ctx;
        }
    }
}
