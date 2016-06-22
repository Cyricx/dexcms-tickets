using System;
using System.Collections.Generic;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class TicketCutoffApiModel
    {
        public int TicketCutoffID { get; set; }
        public int EventID { get; set; }
        public string Name { get; set; }

        public DateTime OnSellDate { get; set; }
        public DateTime CutoffDate { get; set; }

        public List<TicketCutoffAreaApiModel> TicketAreas { get; set; }

        public int TicketPricesCount { get; set; }
    }

    public class TicketCutoffAreaApiModel
    {
        public int TicketAreaID { get; set; }
        public string Name { get; set; }
        public List<TicketCutoffPriceModel> TicketPrices { get; set; }
    }

    public class TicketCutoffPriceModel
    {
        public int? TicketPriceID { get; set; }
        public int EventAgeGroupID { get; set; }
        public string Name { get; set; }
        public int? MinimumAge { get; set; }
        public int? MaximumAge { get; set; }
        public int TicketCutoffID { get; set; }
        public int TicketAreaID { get; set; }
        public decimal? BasePrice { get; set; }
    }
}
