using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DexCMS.Tickets.Schedules.Models
{
    public class ScheduleType
    {
        [Key]
        public int ScheduleTypeID { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        [StringLength(25)]
        public string CssClass { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual ICollection<ScheduleItem> ScheduleItems { get; set; }

    }
}
