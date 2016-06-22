using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DexCMS.Tickets.Events.Models
{
    public class EventFaqItem
    {
        [Key]
        public int EventFaqItemID { get; set; }

        [Required]
        [StringLength(500)]
        public string Question { get; set; }

        [Required]
        public string Answer { get; set; }

        [Required]
        public int DisplayOrder { get; set; }

        public int? HelpfulMarks { get; set; }
        public int? UnhelpfulMarks { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }

        [Required]
        [StringLength(256)]
        public string LastUpdatedBy { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int EventFaqCategoryID { get; set; }

        public virtual EventFaqCategory EventFaqCategory { get; set; }

        [NotMapped]
        public bool? ResetMarks { get; set; }
    }
}
