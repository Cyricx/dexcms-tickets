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
    public class EventAgeGroupRepository : AbstractRepository<EventAgeGroup>, IEventAgeGroupRepository
    {
        public override IDexCMSContext GetContext()
        {
            return _ctx;
        }

        private IDexCMSTicketsContext _ctx { get; set; }

        public EventAgeGroupRepository(IDexCMSTicketsContext ctx)
        {
            _ctx = ctx;
        }

        public override Task<int> AddAsync(EventAgeGroup item)
        {
            if (NoConflicts(item))
            {
                return base.AddAsync(item);
            }
            else
            {
                throw new ApplicationException("The new age group conflicts with existing ages.");
            }
        }

        public override Task<int> UpdateAsync(EventAgeGroup item, int id)
        {
            if (NoConflicts(item, id))
            {
                return base.UpdateAsync(item, id);
            }
            else
            {
                throw new ApplicationException("The new age group conflicts with existing ages.");
            }
        }

        private bool NoConflicts(EventAgeGroup item, int? id = null)
        {
            bool isValid = true;

            //retrieve ages from save event
            List<EventAgeGroup> ageGroups = new List<EventAgeGroup>();

            if (id.HasValue)
            {
                ageGroups = _ctx.EventAgeGroups.Where(x => x.EventID == item.EventID
                                && x.EventAgeGroupID != id).ToList();
            }
            else
            {
                ageGroups = _ctx.EventAgeGroups.Where(x => x.EventID == item.EventID).ToList();
            }

            foreach (EventAgeGroup ageGroup in ageGroups)
            {
                //only test if still valid
                if (isValid)
                {
                    //if the min is inbetween a previous groups ages
                    if (item.MinimumAge >= ageGroup.MinimumAge && (!ageGroup.MaximumAge.HasValue || (item.MinimumAge <= ageGroup.MaximumAge)))
                    {
                        isValid = false;
                    }
                    //if the max is inbetween a previous groups ages
                    else if (item.MaximumAge.HasValue && item.MaximumAge >= ageGroup.MinimumAge && item.MaximumAge <= ageGroup.MaximumAge)
                    {
                        isValid = false;
                    }
                    //make sure we are not wrapping a group
                    else if (item.MinimumAge < ageGroup.MinimumAge && item.MaximumAge > ageGroup.MinimumAge)
                    {
                        isValid = false;
                    }
                }

            }



            return isValid;
        }

    }
}
