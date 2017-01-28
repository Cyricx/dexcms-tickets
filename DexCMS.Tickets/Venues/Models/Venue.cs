using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DexCMS.Base.Models;
using DexCMS.Tickets.Events.Models;
using DexCMS.Core.Models;

namespace DexCMS.Tickets.Venues.Models
{
    public class Venue
    {
        
        [Key]
        public int VenueID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }
        
        [Required]
        public int StateID { get; set; }

        [Required]
        [StringLength(15)]
        public string ZipCode { get; set; }
        
        public virtual ICollection<VenueArea> VenueAreas { get; set; }
        
        public virtual State State { get; set; }
        
        public virtual ICollection<Event> Events { get; set; }

        [NotMapped]
        public int SectionCount
        {
            get
            {
                int count = 0;
                if (VenueAreas != null)
                {
                    foreach (var item in VenueAreas)
                    {
                        count += item.VenueSections.Count;
                    }
                }
                return count;
            }
        }
    }
}
