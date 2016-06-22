using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DexCMS.Tickets.Schedules.Models;

namespace DexCMS.Tickets.Venues.Models
{
    public class VenueScheduleLocation
    {
        [Key]
        public int VenueScheduleLocationID { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        [StringLength(25)]
        public string CssClass { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual ICollection<ScheduleItem> ScheduleItems { get; set; }


        [Required]
        public int VenueID { get; set; }

        public virtual Venue Venue { get; set; }

    }
}
