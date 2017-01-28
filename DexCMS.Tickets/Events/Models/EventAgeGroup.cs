using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DexCMS.Core.Attributes;
using DexCMS.Tickets.Tickets.Models;

namespace DexCMS.Tickets.Events.Models
{
    public class EventAgeGroup
    {
        [Key]
        public int EventAgeGroupID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public int MinimumAge { get; set; }

        [IsIntGreaterThanInt("MinimumAge")]
        public int? MaximumAge { get; set; }

        [Required]
        public int EventID { get; set; }

        public virtual Event Event { get; set; }

        public virtual ICollection<TicketPrice> TicketPrices { get; set; }

        public virtual ICollection<TicketOptionChoice> TicketOptionChoices { get; set; }
        public virtual ICollection<TicketDiscount> TicketDiscounts { get; set; }
    }
}
