using Ninject;
using DexCMS.Tickets.Contexts;
using DexCMS.Tickets.Repositories.Events;
using DexCMS.Tickets.Repositories.Orders;
using DexCMS.Tickets.Repositories.Schedules;
using DexCMS.Tickets.Repositories.Tickets;
using DexCMS.Tickets.Repositories.Venues;
using DexCMS.Tickets.Events.Interfaces;
using DexCMS.Tickets.Orders.Interfaces;
using DexCMS.Tickets.Schedules.Interfaces;
using DexCMS.Tickets.Tickets.Interfaces;
using DexCMS.Tickets.Venues.Interfaces;

namespace DexCMS.Tickets.Globals
{
    public static class TicketsRegister<T> where T : IDexCMSTicketsContext
    {
        public static void RegisterServices(IKernel kernel)
        {
            //! Ticketing.Events
            kernel.Bind<IEventRepository>().To<EventRepository>();
            kernel.Bind<IEventAgeGroupRepository>().To<EventAgeGroupRepository>();
            kernel.Bind<IEventFaqCategoryRepository>().To<EventFaqCategoryRepository>();
            kernel.Bind<IEventFaqItemRepository>().To<EventFaqItemRepository>();
            kernel.Bind<IEventSeriesRepository>().To<EventSeriesRepository>();

            //! Ticket.Orders
            kernel.Bind<IOrderRepository>().To<OrderRepository>();
            kernel.Bind<ITicketRepository>().To<TicketRepository>();

            //! Ticketing.Schedules
            kernel.Bind<IScheduleItemRepository>().To<ScheduleItemRepository>();
            kernel.Bind<IScheduleStatusRepository>().To<ScheduleStatusRepository>();
            kernel.Bind<IScheduleTypeRepository>().To<ScheduleTypeRepository>();

            //! Ticketing.Tickets
            kernel.Bind<ITicketAreaRepository>().To<TicketAreaRepository>();
            kernel.Bind<ITicketAreaDiscountRepository>().To<TicketAreaDiscountRepository>();
            kernel.Bind<ITicketDiscountRepository>().To<TicketDiscountRepository>();
            kernel.Bind<ITicketSeatRepository>().To<TicketSeatRepository>();
            kernel.Bind<ITicketRowRepository>().To<TicketRowRepository>();
            kernel.Bind<ITicketCutoffRepository>().To<TicketCutoffRepository>();
            kernel.Bind<ITicketPriceRepository>().To<TicketPriceRepository>();
            kernel.Bind<ITicketOptionRepository>().To<TicketOptionRepository>();
            kernel.Bind<ITicketOptionDiscountRepository>().To<TicketOptionDiscountRepository>();
            kernel.Bind<ITicketOptionChoiceRepository>().To<TicketOptionChoiceRepository>();

            //! Ticketing.Venues
            kernel.Bind<IVenueRepository>().To<VenueRepository>();
            kernel.Bind<IVenueAreaRepository>().To<VenueAreaRepository>();
            kernel.Bind<IVenueRowRepository>().To<VenueRowRepository>();
            kernel.Bind<IVenueSectionRepository>().To<VenueSectionRepository>();
            kernel.Bind<IVenueScheduleLocationRepository>().To<VenueScheduleLocationRepository>();


            kernel.Bind<IDexCMSTicketsContext>().To<T>();
        }
    }
}
