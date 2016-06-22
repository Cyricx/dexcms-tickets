using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DexCMS.Tickets.Events.Models;

namespace DexCMS.Tickets.Tickets.Models
{
    public class TicketPrice
    {
        [Key]
        public int TicketPriceID { get; set; }

        [Required]
        public int EventAgeGroupID { get; set; }

        [Required]
        public int TicketAreaID { get; set; }

        public int? TicketSectionID { get; set; }

        [Column(TypeName = "Money")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal BasePrice { get; set; }

        [Required]
        public int TicketCutoffID { get; set; }

        public virtual EventAgeGroup EventAgeGroup { get; set; }

        public virtual TicketArea TicketArea { get; set; }

        public virtual TicketSection TicketSection { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }

        public virtual TicketCutoff TicketCutoff { get; set; }
    }
}
