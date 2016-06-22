using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class CheckInApiModel
    {
        public int OrderID { get; set; }
        public int TicketID { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string PreferredName { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public string AgeGroup { get; set; }
        public string DiscountName { get; set; }
        public string PurchasedBy { get; set; }
        public string Seating { get; set; }
        public List<string> Options { get; set; }
        public bool HasArrived { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
    }
}
