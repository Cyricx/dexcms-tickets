namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class EventFaqCategoryApiModel
    {
        public int EventID { get; set; }
        public int EventFaqCategoryID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public int ItemCount { get; set; }
    }
}
