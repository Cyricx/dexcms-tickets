using System.Collections.Generic;
using System.Threading.Tasks;
using DexCMS.Core.Infrastructure.Interfaces;
using DexCMS.Tickets.Events.Models;
using DexCMS.Tickets.Schedules.Models;
using DexCMS.Tickets.Venues.Models;

namespace DexCMS.Tickets.Events.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
        Event RetrieveByUrlSegment(string eventSegment, bool? isPublic = true);

        List<ScheduleType> GetScheduleTypes();
        List<ScheduleStatus> GetScheduleStatuses();
        List<VenueScheduleLocation> GetVenueScheduleLocations(int venueID);
    }
}
