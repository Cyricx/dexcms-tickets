using System;
using System.ComponentModel.DataAnnotations;
using DexCMS.Core.Attributes;
using DexCMS.Tickets.Events.Models;
using DexCMS.Tickets.Venues.Models;

namespace DexCMS.Tickets.Schedules.Models
{
    public class ScheduleItem
    {
        [Key]
        public int ScheduleItemID { get; set; }

        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        [Required]
        [IsDateBeforeDate("EndDate")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public bool IsAllDay { get; set; }

        [StringLength(250)]
        public string OtherLocation { get; set; }

        public int? VenueScheduleLocationID { get; set; }
        public virtual VenueScheduleLocation VenueScheduleLocation { get; set; }

        [Required]
        public int ScheduleStatusID { get; set; }
        public virtual ScheduleStatus ScheduleStatus { get; set; }

        [Required]
        public int ScheduleTypeID { get; set; }
        public virtual ScheduleType ScheduleType { get; set; }

        [MaxLength]
        public string Details { get; set; }

        [Required]
        public int EventID { get; set; }
        public virtual Event Event { get; set; }

    }
}
