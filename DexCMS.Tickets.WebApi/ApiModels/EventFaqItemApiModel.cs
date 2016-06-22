using System;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class EventFaqItemApiModel
    {
        public int EventFaqItemID { get; set; }
        public string Answer { get; set; }
        public int DisplayOrder { get; set; }
        public int EventFaqCategoryID { get; set; }
        public string EventFaqCategoryName { get; set; }
        public int? HelpfulMarks { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public string Question { get; set; }
        public bool? ResetMarks { get; set; }
        public int? UnhelpfulMarks { get; set; }
    }

}
