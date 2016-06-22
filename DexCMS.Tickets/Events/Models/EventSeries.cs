using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DexCMS.Tickets.Events.Models
{
    public class EventSeries
    {
        [Key]
        public int EventSeriesID { get; set; }


        [Required]
        [StringLength(50)]
        public string SeriesName { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression("^[a-z0-9-]+$")]
        public string SeriesUrlSegment { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool AllowMultiplePublic { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}
