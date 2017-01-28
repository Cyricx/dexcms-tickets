using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DexCMS.Core.Repositories;
using DexCMS.Tickets.Events.Models;
using DexCMS.Tickets.Events.Interfaces;
using DexCMS.Tickets.Contexts;
using DexCMS.Core.Contexts;

namespace DexCMS.Tickets.Repositories.Events
{
    public class EventSeriesRepository : AbstractRepository<EventSeries>, IEventSeriesRepository
    {
        public override IDexCMSContext GetContext()
        {
            return _ctx;
        }

        private IDexCMSTicketsContext _ctx { get; set; }

        public EventSeriesRepository(IDexCMSTicketsContext ctx)
        {
            _ctx = ctx;
        }

        public override Task<int> AddAsync(EventSeries item)
        {
            CheckUrlSegmentConflict(item);
            return base.AddAsync(item);
        }
        public override Task<int> UpdateAsync(EventSeries item, int id)
        {
            CheckUrlSegmentConflict(item, id);
            return base.UpdateAsync(item, id);
        }

        public void CheckUrlSegmentConflict(EventSeries item, int? id = null)
        {
                EventSeries series = _ctx.EventSeries
                    .Where(x => x.SeriesUrlSegment == item.SeriesUrlSegment
                    && (!id.HasValue || id.Value != x.EventSeriesID)).SingleOrDefault();
                if (series != null)
                {
                    throw new ApplicationException(
                        string.Format(
                        "The series '{0}' is already using that url segment.",
                        series.SeriesName));
                }
            //make sure it doesn't conflict with an event either
            Event eventConflict = _ctx.Events
                .Where(x => x.EventUrlSegment == item.SeriesUrlSegment).SingleOrDefault();
                if (eventConflict != null)
                {
                    throw new ApplicationException(
                        string.Format(
                        "The event '{0}' is already using that url segment.",
                        eventConflict.PageContent.Heading));
                }
        }

        public List<Event> RetrieveByUrlSegment(string seriesSegment, bool? isActive = true, bool? isPublic = true)
        {
            EventSeries series = Items.Where(x => x.SeriesUrlSegment == seriesSegment &&
                    (!isActive.HasValue || x.IsActive == isActive)).SingleOrDefault();
            if (series == null)
            {
                return null;
            }

            return series.Events.Where(x => (!isPublic.HasValue && x.IsPublic == isPublic)).ToList();
        }

        public Event RetrieveByUrlSegment(string seriesSegment, string eventSegment, bool? isActive = true, bool? isPublic = true)
        {
            EventSeries series = Items.Where(x => x.SeriesUrlSegment == seriesSegment &&
                    (!isActive.HasValue || x.IsActive == isActive)).SingleOrDefault();
            if (series == null)
            {
                return null;
            }
            return series.Events.Where(x => x.EventUrlSegment == eventSegment &&
                    (!isPublic.HasValue && x.IsPublic == isPublic)).SingleOrDefault();
        }

        public Event RetrievePublicSingle(string seriesSegment)
        {
            EventSeries series = Items.Where(x => x.SeriesUrlSegment == seriesSegment &&
                        x.IsActive).SingleOrDefault();
            if (series == null)
            {
                return null;
            }

            return series.Events.Where(x => x.IsPublic).SingleOrDefault();
        }
    }
}