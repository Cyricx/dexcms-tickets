using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DexCMS.Tickets.Tickets.Models;

namespace DexCMS.Tickets.Orders.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [StringLength(256)]
        public string UserName { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public DateTime EnteredOn { get; set; }

        public decimal OrderTotal { get; set; }

        public decimal? RefundAmount { get; set; }

        [StringLength(256)]
        public string RefundedBy { get; set; }

        public DateTime? RefundedOn { get; set; }

        [StringLength(256)]
        public string OverridedBy { get; set; }

        public DateTime? OverridedOn { get; set; }

        [StringLength(2000)]
        public string Notes { get; set; }

        [StringLength(500)]
        public string OverrideReason { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }

        [NotMapped]
        public List<OrderTicketReference> OrderTicketReferences { get; set; }

    }

    [NotMapped]
    public class OrderTicketReference
    {
        public int TicketSeatID { get; set; }
        public string PendingPurchaseConfirmation { get; set; }
        public int EventAgeGroupID { get; set; }
        public int? TicketDiscountID { get; set; }
        public string SecurityConfirmationNumber { get; set; }
        public List<int> TicketOptionChoices { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
