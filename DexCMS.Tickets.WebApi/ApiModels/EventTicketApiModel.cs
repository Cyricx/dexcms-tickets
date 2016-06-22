using System.Collections.Generic;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class EventTicketsApiModel
    {
        public int EventID { get; set; }
        public bool IsSetup { get; set; }
        public int VenueID { get; set; }
        public List<TicketAreaApiModel> TicketAreas { get; set; }
    }



    public class TicketSectionApiModel
    {
        public int? TicketSectionID { get; set; }
        public string Name { get; set; }
        public List<TicketRowApiModel> TicketRows { get; set; }
    }

    public class TicketRowApiModel : SeatableApiModel
    {
        public int? TicketRowID { get; set; }
        public string Designation { get; set; }

    }

    public class SeatableApiModel
    {
        public int MaxCapacity { get; set; }
        public int? UnclaimedReservations { get; set; }
        public int? Unavailable { get; set; }
        public int? Assigned { get; set; }
        public int? NewMaxCapacity { get; set; }
        public int? NewUnavailable { get; set; }
    }

}
