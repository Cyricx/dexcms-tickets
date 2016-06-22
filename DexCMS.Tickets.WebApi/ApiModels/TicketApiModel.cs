using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DexCMS.Tickets.Tickets.Models;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class TicketApiModel
    {
        public int TicketID { get; set; }
        public int TicketSeatID { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string AgeGroup { get; set; }
        public string DiscountName { get; set; }
        public string EventName { get; set; }
        public List<TicketOptionChoiceApiModel> TicketOptionChoices { get; set; }

        public static string BuildLocation(TicketSeat seat)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(seat.TicketArea.Name);
            if (seat.TicketRowID.HasValue)
            {
                sb.Append(" ");
                sb.Append(seat.TicketRow.TicketSection.Name);
                sb.Append(" ");
                sb.Append(seat.TicketRow.Designation);
            }
            sb.Append(" ");
            sb.Append(seat.TicketSeatID);
            return sb.ToString();
        }
    }

}
