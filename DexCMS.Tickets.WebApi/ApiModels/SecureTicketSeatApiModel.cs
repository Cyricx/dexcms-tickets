using System.Collections.Generic;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class SecureTicketSeatApiModel
    {
        public string ConfirmationNumber { get; set; }
        public int TicketSeatID { get; set; }
        public int TicketPriceID { get; set; }
        public int AgeID { get; set; }
        public decimal BasePrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string EventName { get; set; }
        public int? TicketDiscountID { get; set; }
        public string DiscountConfirmationNumber { get; set; }
        public Dictionary<int, int> Options { get; set; }
        public bool? IsValid { get; set; }
    }

}
