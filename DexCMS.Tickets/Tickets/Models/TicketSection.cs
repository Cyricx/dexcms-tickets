using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DexCMS.Tickets.Tickets.Models
{
    public class TicketSection
    {
        
        [Key]
        public int TicketSectionID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int TicketAreaID { get; set; }

        public virtual TicketArea TicketArea { get; set; }
        
        public virtual ICollection<TicketRow> TicketRows { get; set; }
        
        public virtual ICollection<TicketPrice> TicketPrices { get; set; }
    }
}
