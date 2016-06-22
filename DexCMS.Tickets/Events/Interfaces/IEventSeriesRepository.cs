using System.Collections.Generic;
using DexCMS.Core.Infrastructure.Interfaces;
using DexCMS.Tickets.Events.Models;

namespace DexCMS.Tickets.Events.Interfaces
{
    public interface IEventSeriesRepository : IRepository<EventSeries>
    {
        Event RetrievePublicSingle(string seriesSegment);
        List<Event> RetrieveByUrlSegment(string seriesSegment, bool? isActive = true, bool? isPublic = true);
        Event RetrieveByUrlSegment(string seriesSegment, string eventSegment, bool? isActive = true, bool? isPublic = true);
    }
}
