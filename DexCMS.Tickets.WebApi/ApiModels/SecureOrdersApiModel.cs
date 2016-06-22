using System;
using System.Collections.Generic;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class OrderCreateResponseModel
    {
        public int OrderID { get; set; }
    }
    public class SecureOrderApiModel
    {
        public int OrderID { get; set; }
        public string UserName { get; set; }
        public string OrderStatus { get; set; }
        public DateTime EnteredOn { get; set; }
        public decimal OrderTotal { get; set; }
        public List<SecureTicketSeatApiModel> Tickets { get; set; }
        public OrderPaymentApiType PaymentType { get; set; }
        public string PayerID { get; set; }
        public string PaymentID { get; set; }
        public string Token { get; set; }
        public List<TicketsDetail> TicketsDetails { get; set; }
        public bool IsUpdated { get; set; }
    }

    public enum OrderPaymentApiType
    {
        PaypalStart,
        PaypalComplete,
        CreditCard
    }

    public class TicketsDetail
    {
        public string EventName { get; set; }
        public int TicketCount { get; set; }
    }
}
