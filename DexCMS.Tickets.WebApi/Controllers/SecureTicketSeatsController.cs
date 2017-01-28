using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using DexCMS.Base.Enums;
using DexCMS.Tickets.Events.Models;
using DexCMS.Tickets.Tickets.Interfaces;
using DexCMS.Tickets.Tickets.Models;
using DexCMS.Tickets.WebApi.ApiModels;
using DexCMS.Core.Enums;
using DexCMS.Core;

namespace DexCMS.Tickets.WebApi.Controllers
{
    public class SecureTicketSeatsController : ApiController
    {
        private ITicketSeatRepository repository;
        private int RegistrationExpirationMinutes = int.Parse(WebConfigurationManager.AppSettings["RegistrationExpirationMinutes"]);


        public SecureTicketSeatsController(ITicketSeatRepository repo)
        {
            repository = repo;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(SecureTicketSeatApiModel[] model)
        {
            if (model == null || model.Length == 0)
            {
                return Ok();
            }

            List<SecureTicketSeatApiModel> response = new List<SecureTicketSeatApiModel>();

            Dictionary<int, TicketCutoff> cutoffs = new Dictionary<int, TicketCutoff>();

            foreach (var item in model)
            {
                item.IsValid = item.IsValid.HasValue ? item.IsValid : true;
                var seat = await repository.RetrieveAsync(item.TicketSeatID);
                if (seat.PendingPurchaseConfirmation == item.ConfirmationNumber
                    &&
                    (seat.TicketSeatStatus != TicketSeatStatus.Assigned
                    && seat.TicketSeatStatus != TicketSeatStatus.Complete
                    && seat.TicketSeatStatus != TicketSeatStatus.Disabled)
                    )
                {
                    Event evt = seat.TicketArea.Event;

                    //valid seat for this user
                    //calculate the price
                    decimal confirmedPrice = 0;
                    TicketCutoff cutoff = await RetrieveCutoff(cutoffs, item, evt);
                    TicketDiscount discount = await RetrieveDiscount(item, evt);
                    TicketAreaDiscount areaDiscount = RetrieveAreaDiscount(discount, seat);

                    decimal basePrice = await CalculateBasePrice(item, seat, evt, cutoff, areaDiscount);

                    confirmedPrice += basePrice;

                    //now for options
                    if (item.Options != null && item.Options.Count > 0)
                    {
                        foreach (KeyValuePair<int, int> options in item.Options)
                        {
                            //retrieve the ticket option
                            TicketOption option = evt.TicketOptions.Where(x => x.TicketOptionID == options.Key).SingleOrDefault();

                            //retrieve the choice
                            TicketOptionChoice choice = option.TicketOptionChoices
                                .Where(x => x.TicketOptionChoiceID == options.Value
                                && x.EventAgeGroups.Count(y => y.EventAgeGroupID == item.AgeID) > 0).SingleOrDefault();

                            //get discount
                            TicketOptionDiscount optionDiscount = RetrieveOptionDiscount(discount, option);

                            confirmedPrice += CalculateOptionPrice(option, choice, optionDiscount);
                        }
                    }

                    if (confirmedPrice != item.TotalPrice)
                    {
                        await Logger.WriteLog(LogType.Warning,
                            string.Format("Issue confirming price. Browser sent {0:c} and code confirmed {1:c} for Ticket: {2}",
                            item.TotalPrice, confirmedPrice, Newtonsoft.Json.JsonConvert.SerializeObject(item).ToString()));
                    }

                    item.TotalPrice = confirmedPrice;
                    if (item.IsValid.Value)
                    {
                        response.Add(item);
                    }

                }
            }

            return Ok(response);
        }

        private static decimal CalculateOptionPrice(TicketOption option, TicketOptionChoice choice, TicketOptionDiscount optionDiscount)
        {
            decimal optionPrice = option.BasePrice + choice.MarkupPrice;
            if (optionPrice < 0)
            {
                optionPrice = 0;
            }

            //adjust by discount
            if (optionDiscount != null)
            {
                switch (optionDiscount.AdjustmentType)
                {
                    case AdjustmentType.Percent:
                        optionPrice = optionPrice - (optionPrice * (optionDiscount.AdjustmentAmount / 100));
                        break;
                    case AdjustmentType.Flat:
                        optionPrice = optionPrice - optionDiscount.AdjustmentAmount;
                        break;
                    default:
                        break;
                }
                if (optionPrice < 0)
                {
                    optionPrice = 0;
                }
            }

            return optionPrice;
        }

        private static async Task<TicketDiscount> RetrieveDiscount(SecureTicketSeatApiModel item, Event evt)
        {
            TicketDiscount discount = null;

            if (item.TicketDiscountID.HasValue)
            {
                var time = DateTime.Now;
                discount = evt.TicketDiscounts
                    .Where(x => x.TicketDiscountID == item.TicketDiscountID.Value
                    && x.SecurityConfirmationNumber == item.DiscountConfirmationNumber
                    && x.CutoffDate > time
                    && (!x.MaximumAvailable.HasValue ||
                        (x.MaximumAvailable.Value > x.TicketSeats.Count(y => y.TicketSeatStatus == TicketSeatStatus.Assigned || y.TicketSeatStatus == TicketSeatStatus.Complete)))
                        && x.EventAgeGroups.Count(y => y.EventAgeGroupID == item.AgeID) > 0
                    ).SingleOrDefault();
                if (discount == null)
                {
                    await Logger.WriteLog(LogType.Error,
                        string.Format("Failed to retrieve Ticket Discount for Event: {0}, Seat {1} and Discount {2} used by {3}",
                        evt.EventID, item.TicketSeatID, item.TicketDiscountID, HttpContext.Current.Request.UserHostAddress));
                    item.IsValid = false;
                }
            }

            return discount;
        }

        private static TicketAreaDiscount RetrieveAreaDiscount(TicketDiscount discount, TicketSeat seat)
        {
            TicketAreaDiscount areaDiscount = null;

            if (discount != null)
            {
                //see if there is a discount for our area if not, null out the discount
                areaDiscount = discount.TicketAreaDiscounts.Where(x => x.TicketAreaID == seat.TicketAreaID).SingleOrDefault();
            }

            return areaDiscount;
        }

        private static TicketOptionDiscount RetrieveOptionDiscount(TicketDiscount discount, TicketOption option)
        {
            TicketOptionDiscount optionDiscount = null;
            if (discount != null)
            {
                optionDiscount = discount.TicketOptionDiscounts.Where(x => x.TicketOptionID == option.TicketOptionID).SingleOrDefault();

            }

            return optionDiscount;
        }

        private static async Task<decimal> CalculateBasePrice(SecureTicketSeatApiModel item, TicketSeat seat, Event evt, TicketCutoff cutoff, TicketAreaDiscount areaDiscount)
        {
            decimal basePrice = 0;

            TicketPrice price = cutoff.TicketPrices.Where(x => x.TicketAreaID == seat.TicketAreaID && x.EventAgeGroupID == item.AgeID).SingleOrDefault();

            if (price == null)
            {
                await Logger.WriteLog(LogType.Error,
                    string.Format("Failed to retrieve Ticket Price for Event: {0} and Seat {1}",
                    evt.EventID, item.TicketSeatID));
                item.IsValid = false;
            }


            //finally.. if discount null, assign ticket price, else, do calculation
            basePrice = price.BasePrice;

            if (areaDiscount != null)
            {
                switch (areaDiscount.AdjustmentType)
                {
                    case AdjustmentType.Percent:
                        basePrice = basePrice - (basePrice * (areaDiscount.AdjustmentAmount / 100));

                        break;
                    case AdjustmentType.Flat:
                        basePrice = basePrice - areaDiscount.AdjustmentAmount;

                        break;
                    default:
                        break;
                }

                if (basePrice < 0)
                {
                    basePrice = 0;
                }
            }

            return basePrice;
        }

        private static async Task<TicketCutoff> RetrieveCutoff(Dictionary<int, TicketCutoff> cutoffs, SecureTicketSeatApiModel item, Event evt)
        {
            TicketCutoff cutoff = null;
            if (cutoffs.ContainsKey(evt.EventID))
            {
                cutoff = cutoffs[evt.EventID];
            }
            else
            {
                cutoff = evt.TicketCutoffs.Where(x =>
                    DateTime.Now > x.OnSellDate && DateTime.Now < x.CutoffDate).SingleOrDefault();
                if (cutoff == null)
                {

                    await Logger.WriteLog(LogType.Error,
                        string.Format("Failed to retrieve Ticket Cutoff for Event: {0} and Seat {1}",
                        evt.EventID, item.TicketSeatID));
                    item.IsValid = false;
                }
                cutoffs.Add(cutoff.EventID, cutoff);
            }

            return cutoff;
        }

        [HttpPut]
        [ResponseType(typeof(RegistrationResetExpirationResponseModel))]
        public async Task<IHttpActionResult> Put(RegistrationResetExpirationRequestModel model)
        {

            DateTime cstTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Central Standard Time");
            var expiration = cstTime.AddMinutes(RegistrationExpirationMinutes);

            RegistrationResetExpirationResponseModel response = new RegistrationResetExpirationResponseModel()
            {
                ExpirationDate = expiration.ToString("MM/dd/yyyy HH:mm:ss"),
                UpdatedTicketSeats = new List<int>()
            };

            foreach (var item in model.TicketSeats)
            {
                var seat = await repository.RetrieveAsync(item.TicketSeatID);
                if (seat.PendingPurchaseConfirmation == item.PendingPurchaseConfirmation)
                {
                    seat.PendingPurchaseExpiration = expiration;
                    await repository.UpdateAsync(seat, seat.TicketSeatID);
                    response.UpdatedTicketSeats.Add(item.TicketSeatID);
                }
            }

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> UnreserveTicket(int id)
        {
            var seat = await repository.RetrieveAsync(id);

            if (seat == null)
            {
                return NotFound();
            }

            seat.PendingPurchaseConfirmation = null;
            seat.PendingPurchaseExpiration = null;
            await repository.UpdateAsync(seat, seat.TicketSeatID);

            return Ok();

        }

    }
}
