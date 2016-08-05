using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DexCMS.Tickets.Abstracts;
using DexCMS.Tickets.Events.Models;

namespace DexCMS.Tickets.Tickets.Models
{
    public class TicketArea: ISeatable
    {
        
        [Key]
        public int TicketAreaID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public int EventID { get; set; }

        [Required]
        public bool IsGA { get; set; }
        
        public int? DisplayOrder { get; set; }

        public virtual Event Event { get; set; }
        
        public virtual ICollection<TicketSection> TicketSections { get; set; }
        
        public virtual ICollection<TicketSeat> TicketSeats { get; set; }
        
        public virtual ICollection<TicketPrice> TicketPrices { get; set; }
        
        public virtual ICollection<TicketAreaDiscount> TicketAreaDiscounts { get; set; }

    }
}
