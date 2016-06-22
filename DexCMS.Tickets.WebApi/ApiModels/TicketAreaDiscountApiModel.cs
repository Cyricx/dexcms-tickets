using DexCMS.Tickets.Tickets.Models;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class TicketAreaDiscountApiModel
    {
        public int TicketDiscountID { get; set; }

        public int TicketAreaID { get; set; }

        public AdjustmentType AdjustmentType { get; set; }

        public decimal AdjustmentAmount { get; set; }
    }
}
