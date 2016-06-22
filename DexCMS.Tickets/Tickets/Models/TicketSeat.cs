using System;
using System.ComponentModel.DataAnnotations;

namespace DexCMS.Tickets.Tickets.Models
{
    public class TicketSeat
    {
        
        [Key]
        public int TicketSeatID { get; set; }

        [Required]
        public int SeatNumber { get; set; }

        //Location
        [Required]
        public int TicketAreaID { get; set; }
        public virtual TicketArea TicketArea { get; set; }

        public int? TicketRowID { get; set; }
        public virtual TicketRow TicketRow { get; set; }

        public TicketSeatStatus TicketSeatStatus { get; set; }
        public TicketSeatStatus PreviousTicketSeatStatus { get; set; }

        [MaxLength(256)]
        public string PendingPurchaseConfirmation { get; set; }
        public DateTime? PendingPurchaseExpiration { get; set; }

        public int? TicketDiscountID { get; set; }
        public virtual TicketDiscount TicketDiscount { get; set; }

    }

    public enum TicketSeatStatus
    {
        Available,
        Assigned,
        Disabled,
        Reserved,
        Complete
    }
}
