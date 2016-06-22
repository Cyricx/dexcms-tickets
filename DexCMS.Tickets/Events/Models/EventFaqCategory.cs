using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DexCMS.Tickets.Events.Models
{
    public class EventFaqCategory
    {
        [Key]
        public int EventFaqCategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int DisplayOrder { get; set; }

        public virtual ICollection<EventFaqItem> EventFaqItems { get; set; }


        [Required]
        public int EventID { get; set; }

        public virtual Event Event { get; set; }

    }
}
