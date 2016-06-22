namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class TicketPriceApiModel
    {
        public int TicketPriceID { get; set; }
        public int TicketCutoffID { get; set; }
        public int TicketAreaID { get; set; }
        public decimal BasePrice { get; set; }
        public int EventAgeGroupID { get; set; }
    }
}
