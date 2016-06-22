using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DexCMS.Tickets.Events.Models;

namespace DexCMS.Tickets.Tickets.Models
{
    public class TicketDiscount
    {
        
        [Key]
        public int TicketDiscountID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(36)]
        public string SecurityConfirmationNumber { get; set; }

        [Required]
        public int EventID { get; set; }

        [Required]
        public bool IsActive { get; set; }
        
        public int? MaximumAvailable { get; set; }

        [Required]
        public DateTime CutoffDate { get; set; }

        public virtual Event Event { get; set; }

               
        public virtual ICollection<TicketAreaDiscount> TicketAreaDiscounts { get; set; }
        
        public virtual ICollection<TicketOptionDiscount> TicketOptionDiscounts { get; set; }
        public virtual ICollection<TicketSeat> TicketSeats { get; set; }

        public virtual ICollection<EventAgeGroup> EventAgeGroups { get; set; }

        [NotMapped]
        public int[] cbEventAges { get; set; }
    }

    public enum AdjustmentType
    {
        Percent,
        Flat
    }



}
