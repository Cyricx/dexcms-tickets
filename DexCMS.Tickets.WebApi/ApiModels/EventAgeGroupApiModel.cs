namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class EventAgeGroupApiModel
    {
        public int EventAgeGroupID { get; set; }
        public int MinimumAge { get; set; }
        public int? MaximumAge { get; set; }
        public string Name { get; set; }
        public int EventID { get; set; }
        public int TicketPricesCount { get; set; }
        public int TicketOptionsCount { get; set; }
    }
}
