using System.ComponentModel.DataAnnotations;

namespace DexCMS.Tickets.Venues.Models
{
    public class VenueRow
    {
        
        [Key]
        public int VenueRowID { get; set; }

        [Required]
        [StringLength(50)]
        public string Designation { get; set; }

        [Required]
        public int VenueSectionID { get; set; }

        [Required]
        public int SeatCount { get; set; }

        public virtual VenueSection VenueSection { get; set; }
    }
}
