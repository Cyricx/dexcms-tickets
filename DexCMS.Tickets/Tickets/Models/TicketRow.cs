using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DexCMS.Tickets.Abstracts;

namespace DexCMS.Tickets.Tickets.Models
{
    public class TicketRow: ISeatable
    {
        [Key]
        public int TicketRowID { get; set; }

        [Required]
        [StringLength(50)]
        public string Designation { get; set; }

        [Required]
        public int TicketSectionID { get; set; }

        public virtual TicketSection TicketSection { get; set; }
        
        public virtual ICollection<TicketSeat> TicketSeats { get; set; }
    }
}
