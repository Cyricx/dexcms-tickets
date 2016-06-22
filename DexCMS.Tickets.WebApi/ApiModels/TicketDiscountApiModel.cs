using System;
using System.Collections.Generic;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class TicketDiscountApiModel
    {
        public int TicketDiscountID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public int EventID { get; set; }
        public bool IsActive { get; set; }
        public int? MaximumAvailable { get; set; }
        public DateTime CutoffDate { get; set; }
        public int TicketAreaDiscountCount { get; set; }
        public int TicketOptionDiscountCount { get; set; }
        public int TicketReservations { get; set; }
        public int TicketsClaimed { get; set; }
        public List<TicketDiscountAgeApiModel> AgeGroups { get; set; }
        public List<int> cbEventAges { get; set; }
        public int TotalReservations { get; set; }
        public string SecurityConfirmationNumber { get; set; }
    }

    public class TicketDiscountAgeApiModel
    {
        public int? MinimumAge { get; set; }
        public int? MaximumAge { get; set; }
        public string Name { get; set; }
    }
}
