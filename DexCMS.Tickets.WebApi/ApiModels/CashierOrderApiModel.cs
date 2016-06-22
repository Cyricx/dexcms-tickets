using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DexCMS.Tickets.Orders.Models;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class CashierOrderApiModel
    {
        public CashierOrderType CashierOrderType { get; set; }
        public SecureTicketSeatApiModel[] Tickets { get; set; }
        public string InvoiceEmail { get; set; }
        public string InvoiceID { get; set; }
        public int OrderID { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime EnteredOn { get; set; }
        public decimal OrderTotal { get; set; }
        public bool IsChangeToCash { get; set; }
        public string Notes { get; set; }
        public PaymentType PaymentType { get; set; }
        public decimal? GrossPaid { get; set; }
        public string PaymentDetails { get; set; }
        public int? PaymentID { get; set; }
    }

    public enum CashierOrderType
    {
        CashOrCheck,
        Paypal
    }

    public class InvoiceDetails
    {
        public string id { get; set; }
        public string email { get; set; }
    }
}
