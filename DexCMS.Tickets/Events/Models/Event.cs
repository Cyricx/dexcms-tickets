using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DexCMS.Base.Models;
using DexCMS.Tickets.Tickets.Models;
using DexCMS.Tickets.Venues.Models;
using DexCMS.Core.Infrastructure.Attributes;
using DexCMS.Tickets.Schedules.Models;

namespace DexCMS.Tickets.Events.Models
{
    public class Event
    {
        [Key, ForeignKey("PageContent")]
        public int EventID { get; set; }

        [Required]
        [IsDateBeforeDate("EventEnd")]
        public DateTime EventStart { get; set; }

        [Required]
        public DateTime EventEnd { get; set; }

        [Required]
        public int VenueID { get; set; }

        public int? EventSeriesID { get; set; }

        [RequiredIfNull("EventSeriesID", ErrorMessage ="You must choose a series or enter a url segement.")]
        [RegularExpression("^[a-z0-9-]+$")]
        public string EventUrlSegment { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        public DateTime? LastViewedRegistration { get; set; }

        [Required]
        public bool ForceDisableRegistration { get; set; }

        public DateTime? DisablePublicRegistration { get; set; }

        public string RegistrationDisabledMessage { get; set; }

        public virtual Venue Venue { get; set; }

        public virtual PageContent PageContent { get; set; }
        public virtual ICollection<EventAgeGroup> EventAgeGroups { get; set; }


        public virtual ICollection<TicketArea> TicketAreas { get; set; }

        public virtual ICollection<TicketDiscount> TicketDiscounts { get; set; }
        
        public virtual ICollection<TicketCutoff> TicketCutoffs { get; set; }

        public virtual ICollection<TicketOption> TicketOptions { get; set; }
        public virtual ICollection<EventFaqCategory> EventFaqCategories { get; set; }

        public virtual ICollection<ScheduleItem> ScheduleItems { get; set; }

        public virtual EventSeries EventSeries { get; set; }

        [NotMapped]
        [Required]
        public bool AutoGenerateTickets { get; set; }
    }
}
