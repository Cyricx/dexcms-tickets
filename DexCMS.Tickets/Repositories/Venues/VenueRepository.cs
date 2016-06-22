using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DexCMS.Core.Infrastructure.Repositories;
using DexCMS.Tickets.Venues.Models;
using DexCMS.Tickets.Venues.Interfaces;
using System.Data.Entity;
using DexCMS.Tickets.Contexts;
using DexCMS.Core.Infrastructure.Contexts;

namespace DexCMS.Tickets.Repositories.Venues
{
    public class VenueRepository : AbstractRepository<Venue>, IVenueRepository
    {
        public override IDexCMSContext GetContext()
        {
            return _ctx;
        }

        private IDexCMSTicketsContext _ctx { get; set; }

        public VenueRepository(IDexCMSTicketsContext ctx)
        {
            _ctx = ctx;
        }

        public override Task<int> UpdateAsync(Venue item, int id)
        {
            if (item.VenueAreas != null && item.VenueAreas.Count > 0)
            {
                List<VenueArea> venueAreas = item.VenueAreas.ToList();
                venueAreas.ForEach(x => x.VenueID = item.VenueID);
                SaveAreas(venueAreas);
            }
            //TODO: anything?
            //item.State = ctx.States.Find(item.StateID);
            _ctx.Entry(item).State = EntityState.Modified;

            return _ctx.SaveChangesAsync();
        }
        public override Task<int> AddAsync(Venue item)
        {
            _ctx.Venues.Add(item);
            _ctx.SaveChangesAsync();
            if (item.VenueAreas != null && item.VenueAreas.Count > 0)
            {
                List<VenueArea> venueAreas = item.VenueAreas.ToList();
                venueAreas.ForEach(x => x.VenueID = item.VenueID);
                SaveAreas(venueAreas);
            }
            return _ctx.SaveChangesAsync();
        }
        public override Task<int> DeleteAsync(Venue item)
        {
            if (item != null)
            {
                if (item.VenueAreas != null && item.VenueAreas.Count > 0)
                {
                    RemoveAreas(item.VenueAreas.ToList());
                }
                _ctx.Venues.Remove(item);
                return _ctx.SaveChangesAsync();
            }
            else
            {
                return null;
            }
        }

        private void RemoveAreas(List<VenueArea> items)
        {
            foreach (VenueArea item in items)
            {
                if (item.VenueSections != null && item.VenueSections.Count > 0)
                {
                    RemoveSections(item.VenueSections.ToList());
                }
                _ctx.VenueAreas.Remove(item);
            }
        }
        private void RemoveSections(List<VenueSection> items)
        {
            foreach (VenueSection item in items)
            {
                if (item.VenueRows != null && item.VenueRows.Count > 0)
                {
                    RemoveRows(item.VenueRows.ToList());
                }
                _ctx.VenueSections.Remove(item);
            }
        }
        private void RemoveRows(List<VenueRow> items)
        {
            _ctx.VenueRows.RemoveRange(items);
        }
        private void SaveAreas(List<VenueArea> items)
        {
            foreach (var item in items)
            {


                if (item.VenueSections != null && item.VenueSections.Count > 0)
                {
                    List<VenueSection> venueSections = item.VenueSections.ToList();
                    venueSections.ForEach(x => x.VenueAreaID = item.VenueAreaID);
                    SaveSections(venueSections);
                }
                else
                {
                    //make sure there are not any to remove
                    DeleteSections(_ctx.VenueSections.Where(x => x.VenueAreaID == item.VenueAreaID).ToList());
                }
                SaveArea(item);
            }
        }

        private void SaveArea(VenueArea item)
        {
            //TODO: if changing an area is GA make sure any sections/rows are removed.
            if (item.VenueAreaID == 0)
            {
                _ctx.VenueAreas.Add(item);
            }
            else
            {
                _ctx.Entry(item).State = EntityState.Modified;
            }
            _ctx.SaveChanges();
        }

        private void SaveSections(List<VenueSection> items)
        {
            foreach (var item in items)
            {


                if (item.VenueRows != null && item.VenueRows.Count > 0)
                {
                    List<VenueRow> venueRows = item.VenueRows.ToList();
                    venueRows.ForEach(x => x.VenueSectionID = item.VenueSectionID);
                    SaveRows(venueRows);
                }
                SaveSection(item);
            }
        }

        private void SaveSection(VenueSection item)
        {
            if (item.VenueSectionID == 0)
            {
                _ctx.VenueSections.Add(item);
            }
            else
            {
                _ctx.Entry(item).State = EntityState.Modified;
            }
            _ctx.SaveChanges();
        }

        private void DeleteSections(List<VenueSection> items)
        {
            if (items.Count > 0)
            {
                foreach (var item in items)
                {
                    if (item.VenueRows.Count > 0)
                    {
                        _ctx.VenueRows.RemoveRange(item.VenueRows);
                    }
                }
                _ctx.VenueSections.RemoveRange(items);
            }
        }

        private void SaveRows(List<VenueRow> items)
        {
            foreach (var item in items)
            {
                SaveRow(item);
            }
        }

        private void SaveRow(VenueRow item)
        {
            if (item.VenueRowID == 0)
            {
                _ctx.VenueRows.Add(item);
            }
            else
            {
                _ctx.Entry(item).State = EntityState.Modified;
            }
            _ctx.SaveChanges();
        }


    }
}
