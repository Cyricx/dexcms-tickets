using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class OrderApiModel
    {
        public int OrderID { get; set; }
        public string UserName { get; set; }
        public int OrderStatus { get; set; }
        public string OrderStatusName { get; set; }
        public DateTime EnteredOn { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal? RefundAmount { get; set; }
        public DateTime? RefundedOn { get; set; }
        public string OverridedBy { get; set; }
        public DateTime? OverridedOn { get; set; }
        public string OverideReason { get; set; }
        public int? TicketCount { get; set; }
        public List<TicketApiModel> Tickets { get; set; }
        public List<PaymentApiModel> Payments { get; set; }
    }
}
