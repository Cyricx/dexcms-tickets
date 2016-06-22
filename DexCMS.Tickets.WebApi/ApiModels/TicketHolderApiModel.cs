using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class TicketHolderApiModel
    {
        public int OrderID { get; set; }
        public List<TicketSeatHolderApiModel> TicketHolders { get; set; }
    }

    public class TicketSeatHolderApiModel
    {
        public int TicketID { get; set; }
        public int TicketSeatID { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string AgeGroup { get; set; }
        public string DiscountName { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public bool HasArrived { get; set; }
    }
}
