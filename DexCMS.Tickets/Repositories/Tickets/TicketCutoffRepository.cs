using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DexCMS.Core.Infrastructure.Repositories;
using DexCMS.Tickets.Tickets.Models;
using DexCMS.Tickets.Tickets.Interfaces;
using DexCMS.Tickets.Contexts;
using DexCMS.Core.Infrastructure.Contexts;

namespace DexCMS.Tickets.Repositories.Tickets
{
    public class TicketCutoffRepository : AbstractRepository<TicketCutoff>, ITicketCutoffRepository
    {
        public override IDexCMSContext GetContext()
        {
            return _ctx;
        }

        private IDexCMSTicketsContext _ctx { get; set; }

        public TicketCutoffRepository(IDexCMSTicketsContext ctx)
        {
            _ctx = ctx;
        }

        public override Task<int> AddAsync(TicketCutoff item)
        {
            if (NoDateConflicts(item))
            {
                return base.AddAsync(item);
            }
            else
            {
                throw new ApplicationException("A ticket cutoff dates conflict with existing dates.");
            }

        }

        public override Task<int> UpdateAsync(TicketCutoff item, int id)
        {
            if (NoDateConflicts(item, id))
            {
                return base.UpdateAsync(item, id);
            }
            else
            {
                throw new ApplicationException("A ticket cutoff dates conflict with existing dates.");
            }
        }

        private bool NoDateConflicts(TicketCutoff item, int? id = null)
        {
            //retrieve items from same event
            List<TicketCutoff> cutoffs = new List<TicketCutoff>();

            if (id.HasValue)
            {
                cutoffs = _ctx.TicketCutoffs.Where(x => x.EventID == item.EventID
                                        && x.TicketCutoffID != id).ToList();

            }
            else
            {
                cutoffs = _ctx.TicketCutoffs.Where(x => x.EventID == item.EventID).ToList();
            }

            bool isValid = true;

            foreach (TicketCutoff cutoff in cutoffs)
            {
                //only test if is still valid
                if (isValid)
                {
                    //if the sell date is inbetween a previous item's dates
                    if (item.OnSellDate >= cutoff.OnSellDate && item.OnSellDate <= cutoff.CutoffDate)
                    {
                        isValid = false;
                    }
                    //if the cutoff date is between a previous item's dates
                    else if (item.CutoffDate >= cutoff.OnSellDate && item.CutoffDate <= cutoff.CutoffDate)
                    {
                        isValid = false;
                    }
                    //make sure it comes before or after
                    else if (item.OnSellDate < cutoff.OnSellDate & item.CutoffDate > cutoff.OnSellDate)
                    {
                        isValid = false;
                    }
                }
            }

            return isValid;

        }
    }
}
