using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DexCMS.Core.Contexts;
using DexCMS.Core.Repositories;
using DexCMS.Tickets.Contexts;
using DexCMS.Tickets.Tickets.Interfaces;
using DexCMS.Tickets.Tickets.Models;

namespace DexCMS.Tickets.Repositories.Tickets
{
    public class TicketOptionChoiceRepository : AbstractRepository<TicketOptionChoice>, ITicketOptionChoiceRepository
    {
        public override IDexCMSContext GetContext()
        {
            return _ctx;
        }

        private IDexCMSTicketsContext _ctx { get; set; }

        public TicketOptionChoiceRepository(IDexCMSTicketsContext ctx)
        {
            _ctx = ctx;
        }

        public async override Task<int> AddAsync(TicketOptionChoice item)
        {
            await base.AddAsync(item);
            return await this.UpdateAsync(item, item.TicketOptionChoiceID);
        }


        public override Task<int> UpdateAsync(TicketOptionChoice item, int id)
        {
            if (item.cbEventAges != null)
            {
                _ctx.Entry(item).State = EntityState.Modified;
                _ctx.Entry(item).Collection(x => x.EventAgeGroups).Load();
                item.EventAgeGroups.Clear();

                BuildEventAgeGroups(item);
                _ctx.SaveChanges();
            }

            return base.UpdateAsync(item, id);
        }

        private void BuildEventAgeGroups(TicketOptionChoice item)
        {
            if (item.cbEventAges != null && item.cbEventAges.Length > 0)
            {
                var ages = _ctx.EventAgeGroups.ToList();
                foreach (var cb in item.cbEventAges)
                {
                    var eventAge = ages.Where(x => x.EventAgeGroupID == cb).Single();
                    item.EventAgeGroups.Add(eventAge);
                }
            }
        }
    }
}
