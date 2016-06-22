namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class VenueScheduleLocationApiModel
    {
        public int VenueScheduleLocationID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string CssClass { get; set; }
        public int VenueID { get; set; }
        public string VenueName { get; set; }
        public int ScheduleItemCount { get; set; }
    }
}
