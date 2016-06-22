using System.Collections.Generic;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class TicketOptionChoiceApiModel
    {
        public int TicketOptionChoiceID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MarkupPrice { get; set; }
        public int? MaximumAvailable { get; set; }
        public int TicketOptionID { get; set; }
        public string TicketOptionName { get; set; }
        public List<int> cbEventAges { get; set; }
        public List<TicketChoiceAgeApiModel> AgeGroups { get; set; }
        public int TicketCount { get; set; }
    }

    public class TicketChoiceAgeApiModel
    {
        public int? MinimumAge { get; set; }
        public int? MaximumAge { get; set; }
        public string Name { get; set; }
    }
}
