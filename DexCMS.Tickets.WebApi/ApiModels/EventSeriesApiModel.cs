namespace DexCMS.Tickets.WebApi.ApiModels
{

    public class EventSeriesApiModel
    {
        public int EventSeriesID { get; set; }
        public string SeriesName { get; set; }
        public bool IsActive { get; set; }
        public bool AllowMultiplePublic { get; set; }
        public string SeriesUrlSegment { get; set; }
    }
}
