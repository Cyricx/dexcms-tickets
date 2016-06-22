using System.Data.Entity;
using DexCMS.Base.Contexts;
using DexCMS.Tickets.Events.Models;
using DexCMS.Tickets.Orders.Models;
using DexCMS.Tickets.Schedules.Models;
using DexCMS.Tickets.Tickets.Models;
using DexCMS.Tickets.Venues.Models;

namespace DexCMS.Tickets.Contexts
{
    public interface IDexCMSTicketsContext:IDexCMSBaseContext
    {
        DbSet<Event> Events { get; set; }
        DbSet<EventAgeGroup> EventAgeGroups { get; set; }
        DbSet<EventFaqCategory> EventFaqCategories { get; set; }
        DbSet<EventFaqItem> EventFaqItems { get; set; }
        DbSet<EventSeries> EventSeries { get; set; }

        DbSet<Order> Orders { get; set; }
        DbSet<Payment> Payments { get; set; }

        DbSet<ScheduleItem> ScheduleItems { get; set; }
        DbSet<ScheduleType> ScheduleTypes { get; set; }
        DbSet<ScheduleStatus> ScheduleStatuses { get; set; }

        DbSet<Ticket> Tickets { get; set; }
        DbSet<TicketArea> TicketAreas { get; set; }
        DbSet<TicketAreaDiscount> TicketAreaDiscounts { get; set; }
        DbSet<TicketDiscount> TicketDiscounts { get; set; }
        DbSet<TicketOptionDiscount> TicketOptionDiscounts { get; set; }
        DbSet<TicketOptionChoice> TicketOptionChoices { get; set; }
        DbSet<TicketOption> TicketOptions { get; set; }
        DbSet<TicketPrice> TicketPrices { get; set; }
        DbSet<TicketRow> TicketRows { get; set; }
        DbSet<TicketSeat> TicketSeats { get; set; }
        DbSet<TicketSection> TicketSections { get; set; }
        DbSet<TicketCutoff> TicketCutoffs { get; set; }

        DbSet<VenueArea> VenueAreas { get; set; }
        DbSet<Venue> Venues { get; set; }
        DbSet<VenueRow> VenueRows { get; set; }
        DbSet<VenueSection> VenueSections { get; set; }
        DbSet<VenueScheduleLocation> VenueScheduleLocations { get; set; }

    }
}
