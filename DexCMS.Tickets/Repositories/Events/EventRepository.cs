using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DexCMS.Core.Infrastructure.Repositories;
using DexCMS.Tickets.Events.Models;
using DexCMS.Tickets.Events.Interfaces;
using System.Data.Entity;
using DexCMS.Tickets.Contexts;
using DexCMS.Core.Infrastructure.Contexts;
using DexCMS.Tickets.Schedules.Models;
using DexCMS.Tickets.Venues.Models;

namespace DexCMS.Tickets.Repositories.Events
{
    public class EventRepository : AbstractRepository<Event>, IEventRepository
    {
        public override IDexCMSContext GetContext()
        {
            return _ctx;
        }

        private IDexCMSTicketsContext _ctx { get; set; }

        public EventRepository(IDexCMSTicketsContext ctx)
        {
            _ctx = ctx;
        }

        public override Task<int> DeleteAsync(Event item)
        {
            if (item != null)
            {
                //delete content blocks
                if (item.PageContent.ContentBlocks.Count > 0)
                {
                    _ctx.ContentBlocks.RemoveRange(item.PageContent.ContentBlocks);
                }

                //delete content scripts
                if (item.PageContent.ContentScripts.Count > 0)
                {
                    _ctx.ContentScripts.RemoveRange(item.PageContent.ContentScripts);
                }

                //delete content styles
                if (item.PageContent.ContentStyles.Count > 0)
                {
                    _ctx.ContentStyles.RemoveRange(item.PageContent.ContentStyles);
                }

                //delete content images
                if (item.PageContent.PageContentImages.Count > 0)
                {
                    _ctx.PageContentImages.RemoveRange(item.PageContent.PageContentImages);
                }

                _ctx.PageContents.Remove(item.PageContent);
            }

            return base.DeleteAsync(item);
        }

        public override Task<int> UpdateAsync(Event item, int id)
        {
            CheckPublicConflicts(item, id);
            CheckUrlConflicts(item, id);
            _ctx.Entry(item.PageContent).State = EntityState.Modified;
            return base.UpdateAsync(item, id);
        }

        public override Task<int> AddAsync(Event item)
        {
            CheckPublicConflicts(item);
            CheckUrlConflicts(item);
            return base.AddAsync(item);

        }

        private void CheckPublicConflicts(Event item, int? id = null)
        {
            if (item.IsPublic && item.EventSeriesID.HasValue)
            {
                //make sure the series either allows multiple public, or that no other
                //events in the series are currently public
                EventSeries series = _ctx.EventSeries.Find(item.EventSeriesID);
                if (series != null && !series.AllowMultiplePublic)
                {
                    //need to check for other events to be public 
                    List<Event> conflictEvents = new List<Event>();
                    if (id.HasValue)
                    {
                        conflictEvents = series.Events.Where(x => x.IsPublic && x.EventID != id).ToList();
                    }
                    else
                    {
                        conflictEvents = series.Events.Where(x => x.IsPublic).ToList();
                    }

                    if (conflictEvents.Count() > 0)
                    {
                        
                        throw new ApplicationException("An event is already public for this series.");
                    }
                }

            }
        }

        private void CheckUrlConflicts(Event item, int? id = null)
        {
            if (!item.EventSeriesID.HasValue)
            {
                Event eventConflict = _ctx.Events
                    .Where(x => x.EventUrlSegment == item.EventUrlSegment
                    && (!id.HasValue || id.Value != x.EventID)).SingleOrDefault();
                if (eventConflict != null)
                {
                    throw new ApplicationException("The event '{0}' is already using that url segment.");
                }
                //make sure it doesn't conflict with a series either
                EventSeries series = _ctx.EventSeries
                    .Where(x => x.SeriesUrlSegment == item.EventUrlSegment).SingleOrDefault();
                if (series != null)
                {
                    throw new ApplicationException(
                        string.Format(
                        "The series '{0}' is already using that url segment.",
                        series.SeriesName));
                }
            }
        }

        public Event RetrieveByUrlSegment(string eventSegment, bool? isPublic = true)
        {
            return Items.Where(x => x.EventUrlSegment == eventSegment && (!isPublic.HasValue ||
                    x.IsPublic == isPublic)).SingleOrDefault();
        }

        public List<ScheduleType> GetScheduleTypes()
        {
            return _ctx.ScheduleTypes.Where(x => x.IsActive).ToList();
        }

        public List<ScheduleStatus> GetScheduleStatuses()
        {
            return _ctx.ScheduleStatuses.Where(x => x.IsActive).ToList();
        }

        public List<VenueScheduleLocation> GetVenueScheduleLocations(int venueID)
        {
            return _ctx.VenueScheduleLocations.Where(x => x.VenueID == venueID && x.IsActive).ToList();
        }
    }

}
