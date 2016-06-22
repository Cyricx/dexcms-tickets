using System;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class ScheduleItemApiModel
    {
        public int ScheduleItemID { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsAllDay { get; set; }
        public string OtherLocation { get; set; }
        public int? VenueScheduleLocationID { get; set; }
        public string VenueScheduleLocationName { get; set; }
        public int ScheduleStatusID { get; set; }
        public string ScheduleStatusName { get; set; }
        public int ScheduleTypeID { get; set; }
        public string ScheduleTypeName { get; set; }
        public string Details { get; set; }
        public int EventID { get; set; }
    }
}
