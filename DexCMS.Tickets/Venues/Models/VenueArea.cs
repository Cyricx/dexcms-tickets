using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DexCMS.Tickets.Venues.Models
{
    public class VenueArea
    {
        
        [Key]
        public int VenueAreaID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public int VenueID { get; set; }

        [Required]
        public bool IsGA { get; set; }

        public int? GASeatCount { get; set; }
        
        public virtual Venue Venue { get; set; }

        public virtual ICollection<VenueSection> VenueSections { get; set; }

    }
}
