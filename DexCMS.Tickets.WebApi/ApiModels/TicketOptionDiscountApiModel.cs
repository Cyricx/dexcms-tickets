using DexCMS.Tickets.Tickets.Models;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class TicketOptionDiscountApiModel
    {
        public int TicketDiscountID { get; set; }

        public int TicketOptionID { get; set; }

        public AdjustmentType AdjustmentType { get; set; }

        public decimal AdjustmentAmount { get; set; }
    }
}
