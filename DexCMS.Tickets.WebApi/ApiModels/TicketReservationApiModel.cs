using System.Collections.Generic;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class TicketReservationApiModel
    {
        public int TicketDiscountID { get; set; }
        public string Name { get; set; }
        public int? MaximumAvailable { get; set; }
        public int TotalReservations { get; set; }
        public List<ReservationAreaApiModel> TicketAreas {get;set;}
    }

    public class ReservationAreaApiModel: ReservableApiModel
    {
        public int TicketAreaID { get; set; }
        public string Name { get; set; }
        public bool IsGA { get; set; }
        public List<ReservationSectionApiModel> TicketSections { get; set; }
    }

    public class ReservationSectionApiModel
    {
        public int TicketSectionID { get; set; }
        public string Name { get; set; }
        public List<ReservationRowApiModel> TicketRows { get; set; }
    }
    public class ReservationRowApiModel: ReservableApiModel
    {
        public string Designation { get; set; }
        public int TicketRowID { get; set; }
    }

    public class ReservationSeatApiModel
    {
        public int TicketDiscountID { get; set; }
        public int? TicketAreaID { get; set; }
        public int? TicketSectionID { get; set; }
        public int? TicketRowID { get; set; }
        public int DiscountReservations { get; set; }
        public int Unavailable { get; set; }

    }

    public class ReservableApiModel
    {
        public int MaxCapacity { get; set; }
        public int DiscountReservations { get; set; }
        public int DiscountAssigned { get; set; }
        public int Unavailable { get; set; }
        public int UnclaimedReservations { get; set; }
        public int Assigned { get; set; }
        public int Available { get; set; }
        public int PendingPurchase { get; set; }
        public int PendingDiscount { get; set; }
    }
}
