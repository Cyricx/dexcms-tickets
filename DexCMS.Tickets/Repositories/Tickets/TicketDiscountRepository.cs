using System.Linq;
using System.Threading.Tasks;
using DexCMS.Core.Infrastructure.Repositories;
using DexCMS.Tickets.Tickets.Models;
using DexCMS.Tickets.Tickets.Interfaces;
using System.Data.Entity;
using DexCMS.Tickets.Contexts;
using DexCMS.Core.Infrastructure.Contexts;

namespace DexCMS.Tickets.Repositories.Tickets
{
    public class TicketDiscountRepository : AbstractRepository<TicketDiscount>, ITicketDiscountRepository
    {
        public override IDexCMSContext GetContext()
        {
            return _ctx;
        }

        private IDexCMSTicketsContext _ctx { get; set; }

        public TicketDiscountRepository(IDexCMSTicketsContext ctx)
        {
            _ctx = ctx;
        }

        public async override Task<int> AddAsync(TicketDiscount item)
        {
            await base.AddAsync(item);
            return await this.UpdateAsync(item, item.TicketDiscountID);
        }

        public override Task<int> UpdateAsync(TicketDiscount item, int id)
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

        private void BuildEventAgeGroups(TicketDiscount item)
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
