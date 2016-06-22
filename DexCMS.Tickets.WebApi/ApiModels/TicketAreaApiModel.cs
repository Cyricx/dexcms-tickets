using System.Collections.Generic;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class TicketAreaApiModel : SeatableApiModel
    {
        public int? TicketAreaID { get; set; }
        public string Name { get; set; }
        public bool IsGA { get; set; }
        public List<TicketSectionApiModel> TicketSections { get; set; }
        public int EventID { get; set; }
    }
}
