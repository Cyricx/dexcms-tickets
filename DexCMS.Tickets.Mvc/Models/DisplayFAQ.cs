using System.Collections.Generic;
using DexCMS.Tickets.Events.Models;

namespace DexCMS.Tickets.Mvc.Models
{
    public class DisplayFAQ
    {
        public List<EventFaqCategory> faqCategories { get; set; }
        public List<EventFaqItem> faqItems { get; set; }
    }
}
