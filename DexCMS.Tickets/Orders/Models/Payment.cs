using System;
using System.ComponentModel.DataAnnotations;

namespace DexCMS.Tickets.Orders.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }


        [Required]
        public int OrderID { get; set; }

        public virtual Order Order { get; set; }

        public DateTime? PaidOn { get; set; }

        public PaymentType PaymentType { get; set; }

        public string PaymentDetails { get; set; }

        public decimal? GrossPaid { get; set; }
        public decimal? PaymentFee { get; set; }
        public decimal? NetPaid { get; set; }
    }
}
