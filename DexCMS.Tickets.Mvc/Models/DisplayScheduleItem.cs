
namespace DexCMS.Tickets.Mvc.Models
{
    public class DisplayScheduleItem
    {
        public int id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public bool allDay { get; set; }
        public string location { get; set; }
        public string details { get; set; }
        public string className { get; set; }
        public string status { get; set; }
        public string statusClass { get; set; }
        public string type { get; set; }
    }
}
