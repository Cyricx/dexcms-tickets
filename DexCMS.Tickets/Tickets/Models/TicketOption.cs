using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DexCMS.Tickets.Events.Models;

namespace DexCMS.Tickets.Tickets.Models
{
    public class TicketOption
    {
        
        [Key]
        public int TicketOptionID { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [MaxLength]
        public string Description { get; set; }

        [Column(TypeName = "Money")]
        [DataType(DataType.Currency)]
        public decimal BasePrice { get; set; }

        [Required]
        public DateTime CutoffDate { get; set; }

        [Required]
        public bool IsRequired { get; set; }

        [Required]
        public int EventID { get; set; }

        public virtual Event Event { get; set; }
        
        public virtual ICollection<TicketOptionChoice> TicketOptionChoices { get; set; }

        public virtual ICollection<TicketOptionDiscount> TicketOptionDiscounts { get; set; }

    }
}
