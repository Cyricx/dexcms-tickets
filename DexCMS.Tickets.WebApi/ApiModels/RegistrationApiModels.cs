using System.Collections.Generic;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class RegistrationAgeGroupApiModel
    {
        public int RegistrationAgeGroupID { get; set; }
        public int TicketPriceID { get; set; }
        public string Name { get; set; }
        public string AgeRange { get; set; }
        public string AgeGroup { get; set; }
        public decimal BasePrice { get; set; }
        public string Display { get; set; }
    }

    public class RegistrationAreaApiModel
    {
        public int RegistrationAreaID { get; set; }
        public string Name { get; set; }
        public bool IsGA { get; set; }
        public int AvailableTickets { get; set; }
        public List<RegistrationSectionApiModel> RegistrationSections { get; set; }
    }

    public class RegistrationSectionApiModel
    {
        public int RegistrationSectionID { get; set; }
        public string Name { get; set; }
        public List<RegistrationRowApiModel> RegistrationRows { get; set; }
        public int AvailableTickets { get; set; }
    }
    public class RegistrationRowApiModel
    {
        public int RegistrationRowID { get; set; }
        public string Designation { get; set; }
        public int AvailableTickets { get; set; }
    }
    public class RegistrationCutoffApiModel
    {
        public string OnSellDate { get; set; }
        public string CutoffDate { get; set; }
        public List<RegistrationCutoffAreaApiModel> Areas { get; set; }
    }
    public class RegistrationCutoffAreaApiModel
    {
        public int AreaID { get; set; }
        public string Name { get; set; }
        public List<RegistrationCutoffPriceApiModel> Prices { get; set; }
    }
    public class RegistrationCutoffPriceApiModel
    {
        public string AgeName { get; set; }
        public string AgeRange { get; set; }
        public decimal BasePrice { get; set; }
    }

    public class RegistrationAddTicketsApiModel
    {
        public int AreaID { get; set; }
        public int? SectionID { get; set; }
        public int? RowID { get; set; }
        public int SeatCount { get; set; }
        public int? TicketDiscountID { get; set; }
        public List<RegistrationAddTicketApiModel> Tickets { get; set; }
    }
    public class RegistrationAddTicketApiModel
    {
        public int TicketPriceID { get; set; }
    }

    public class RegistrationTicketResponseModel
    {
        public List<PendingTicketModel> Tickets { get; set; }
    }
    public class PendingTicketModel
    {
        public int TicketSeatID { get; set; }
        public string ConfirmationNumber { get; set; }
        public int TicketPriceID { get; set; }
        public string ExpirationTime { get; set; }
    }

    public class RegistrationResetExpirationRequestModel
    {
        public List<ResetSeatModel> TicketSeats { get; set; }
    }
    public class RegistrationResetExpirationResponseModel
    {
        public string ExpirationDate { get; set; }
        public List<int> UpdatedTicketSeats { get; set; }
    }

    public class ResetSeatModel
    {
        public int TicketSeatID { get; set; }
        public string PendingPurchaseConfirmation { get; set; }
    }

    public class RegistrationTicketOption
    {
        public int TicketOptionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsRequired { get; set; }
        public List<RegistrationTicketOptionChoice> TicketOptionChoices { get; set; }
    }
    public class RegistrationTicketOptionChoice
    {
        public int TicketOptionChoiceID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal AdjustedPrice { get; set; }
        public int? Available { get; set; }
    }

    public class RegistrationVerifyDiscount
    {
        public string Code { get; set; }
    }
    public class RegistrationDiscountResponse
    {
        public int TicketDiscountID { get; set; }
        public string Name { get; set; }
        public string DiscountConfirmationNumber { get; set; }
        public int? MaxAvailable { get; set; }
        public string Code { get; set; }
    }
}
