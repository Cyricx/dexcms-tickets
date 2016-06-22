using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DexCMS.Tickets.Events.Models;

namespace DexCMS.Tickets.Tickets.Models
{
    public class TicketOptionChoice
    {
        [Key]
        public int TicketOptionChoiceID { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [MaxLength]
        public string Description { get; set; }

        [Column(TypeName = "Money")]
        [DataType(DataType.Currency)]
        public decimal MarkupPrice { get; set; }

        public int? MaximumAvailable { get; set; }

        [Required]
        public int TicketOptionID { get; set; }

        public virtual TicketOption TicketOption { get; set; }

        public virtual ICollection<EventAgeGroup> EventAgeGroups { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }

        [NotMapped]
        public int[] cbEventAges { get; set; }
    }
}
