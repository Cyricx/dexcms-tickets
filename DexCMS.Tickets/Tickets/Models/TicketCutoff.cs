using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DexCMS.Tickets.Events.Models;
using DexCMS.Core.Attributes;

namespace DexCMS.Tickets.Tickets.Models
{
    public class TicketCutoff
    {
        [Key]
        public int TicketCutoffID { get; set; }

        [Required]
        public int EventID { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        [Required]
        [IsDateBeforeDate("CutoffDate")]
        public DateTime OnSellDate { get; set; }
        
        [Required]
        public DateTime CutoffDate { get; set; }
      
        public virtual Event Event { get; set; }
        
        public virtual ICollection<TicketPrice> TicketPrices { get; set; }
    }
}
