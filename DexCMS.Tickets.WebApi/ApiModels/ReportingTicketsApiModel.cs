using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class ReportingTicketApiModel
    {
        public int OrderID { get; set; }
        public int TicketID { get; set; }
        public string UserName { get; set; }
        public string PreferredName { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string TicketAreaName { get; set; }
        public string TicketSectionName { get; set; }
        public string TicketRowDesignation { get; set; }
        public string TicketDiscountName { get; set; }
        public string TicketDiscountCode { get; set; }
        public decimal TicketTotalPrice { get; set; }
        public decimal OrderTotal { get; set; }
        public List<ReportingTicketOptionApiModel> Options { get; set; }
    }

    public class ReportingTicketOptionApiModel
    {
        public string OptionName { get; set; }
        public string OptionChoiceName { get; set; }
    }
}
