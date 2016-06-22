using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DexCMS.Tickets.Tickets.Models
{
    public partial class TicketOptionDiscount
    {
        [Key, Column(Order = 0)]
        public int TicketDiscountID { get; set; }

        [Key, Column(Order = 1)]
        public int TicketOptionID { get; set; }

        public AdjustmentType AdjustmentType { get; set; }

        [Required]
        public decimal AdjustmentAmount { get; set; }

        public virtual TicketDiscount TicketDiscount { get; set; }

        public virtual TicketOption TicketOption { get; set; }
    }
}
