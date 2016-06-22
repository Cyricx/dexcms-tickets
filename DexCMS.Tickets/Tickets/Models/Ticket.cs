using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DexCMS.Tickets.Orders.Models;

namespace DexCMS.Tickets.Tickets.Models
{
    public class Ticket
    {
        
        [Key, ForeignKey("TicketSeat")]
        public int TicketID { get; set; }
        public virtual TicketSeat TicketSeat { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }
        
        [StringLength(20)]
        public string MiddleInitial { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }
        
        public string PreferredName { get; set; }

        public DateTime? ArrivalTime { get; set; }

        [Required]
        public int TicketPriceID { get; set; }
        public virtual TicketPrice TicketPrice { get; set; }

        public decimal TicketTotalPrice { get; set; }

        [Required]
        public int OrderID { get; set; }

        public virtual Order Order { get; set; }


        public int? TicketDiscountID { get; set; }
        public virtual TicketDiscount TicketDiscount { get; set; }

        public virtual ICollection<TicketOptionChoice> TicketOptionChoices { get; set; }

    }
}
