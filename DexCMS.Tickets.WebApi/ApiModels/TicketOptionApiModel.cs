using System;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class TicketOptionApiModel
    {
        public int TicketOptionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public DateTime CutoffDate { get; set; }
        public int EventID { get; set; }
        public int TicketOptionChoiceCount { get; set; }
        public bool IsRequired { get; set; }
    }
}
