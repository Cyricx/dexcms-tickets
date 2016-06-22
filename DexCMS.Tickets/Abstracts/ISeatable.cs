using System.Collections.Generic;
using DexCMS.Tickets.Tickets.Models;

namespace DexCMS.Tickets.Abstracts
{
    public interface ISeatable
    {
        ICollection<TicketSeat> TicketSeats { get; set; }
    }
}
