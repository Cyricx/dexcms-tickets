namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class ScheduleStatusApiModel
    {
        public int ScheduleStatusID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string CssClass { get; set; }
        public int ScheduleItemCount { get; set; }

    }
}
