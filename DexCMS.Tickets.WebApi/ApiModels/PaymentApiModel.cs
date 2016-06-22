using System;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class PaymentApiModel
    {
        public int PaymentID { get; set; }
        public int OrderID { get; set; }
        public DateTime? PaidOn { get; set; }
        public int PaymentType { get; set; }
        public string PaymentTypeName { get; set; }
        public decimal? GrossPaid { get; set; }
        public decimal? PaymentFee { get; set; }
        public decimal? NetPaid { get; set; }
    }
}
