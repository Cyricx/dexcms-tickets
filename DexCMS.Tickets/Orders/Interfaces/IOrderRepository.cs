using System.Linq;
using System.Threading.Tasks;
using DexCMS.Core.Infrastructure.Interfaces;
using DexCMS.Tickets.Orders.Models;

namespace DexCMS.Tickets.Orders.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        IQueryable<Order> RetrieveUserOrders(string userName);
    }
}
