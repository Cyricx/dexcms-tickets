using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DexCMS.Tickets.Venues.Models
{
    public class VenueSection
    {
        
        [Key]
        public int VenueSectionID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int VenueAreaID { get; set; }

        public virtual VenueArea VenueArea { get; set; }
        
        public virtual ICollection<VenueRow> VenueRows { get; set; }
    }
}
