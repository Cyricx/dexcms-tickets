using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using DexCMS.Tickets.Events.Interfaces;
using DexCMS.Tickets.Events.Models;
using DexCMS.Tickets.Tickets.Interfaces;
using DexCMS.Tickets.Tickets.Models;
using DexCMS.Tickets.WebApi.ApiModels;
using DexCMS.Core.Infrastructure.Enums;
using DexCMS.Core.Infrastructure;

namespace DexCMS.Tickets.WebApi.Controllers
{
    public class RegistrationController : ApiController
    {
        private IEventRepository eventRepository;
        private IEventSeriesRepository seriesRepository;
        private ITicketSeatRepository seatsRepository;
        private int RegistrationExpirationMinutes = int.Parse(WebConfigurationManager.AppSettings["RegistrationExpirationMinutes"]);
        private DateTime cstTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Central Standard Time");

        public RegistrationController(IEventRepository eventRepo, IEventSeriesRepository seriesRepo, ITicketSeatRepository seatsRepo)
        {
            eventRepository = eventRepo;
            seriesRepository = seriesRepo;
            seatsRepository = seatsRepo;
        }

        // GET api/RegistrationAgeGroups/5
        [HttpGet]
        [ResponseType(typeof(List<RegistrationAgeGroupApiModel>))]
        public IHttpActionResult AgeGroups(string segment, int id)
        {
            Event evt = RetrieveEvent(segment);
            if (evt == null)
            {
                return NotFound();
            }

            if (!CanAccessRegistration(evt))
            {
                return BadRequest(GetDisabledMessage(evt));
            }

            var cutoff = evt.TicketCutoffs.Where(x =>
                cstTime > x.OnSellDate && cstTime < x.CutoffDate).SingleOrDefault();
            if (cutoff == null)
            {
                return NotFound();
            }

            var items = cutoff.TicketPrices.Where(x => x.TicketAreaID == id).OrderBy(x => x.EventAgeGroup.MinimumAge).Select(x => new RegistrationAgeGroupApiModel
            {
                RegistrationAgeGroupID = x.EventAgeGroupID,
                TicketPriceID = x.TicketPriceID,
                Name = x.EventAgeGroup.Name,
                AgeRange = (x.EventAgeGroup.MaximumAge.HasValue ? x.EventAgeGroup.MinimumAge + " - " + x.EventAgeGroup.MaximumAge : x.EventAgeGroup.MinimumAge + "+"),
                Display = x.EventAgeGroup.Name + " (" + (x.EventAgeGroup.MaximumAge.HasValue ? x.EventAgeGroup.MinimumAge + " - " + x.EventAgeGroup.MaximumAge : x.EventAgeGroup.MinimumAge + "+") + ") " + x.BasePrice.ToString("c"),
                AgeGroup = x.EventAgeGroup.Name + " (" + (x.EventAgeGroup.MaximumAge.HasValue ? x.EventAgeGroup.MinimumAge + " - " + x.EventAgeGroup.MaximumAge : x.EventAgeGroup.MinimumAge + "+") + ")",
                BasePrice = x.BasePrice

            }).ToList();

            return Ok(items);
        }

        [HttpGet]
        [ResponseType(typeof(List<RegistrationAgeGroupApiModel>))]
        public IHttpActionResult AgeGroups(string segment, int id, int secondKey)
        {
            Event evt = RetrieveEvent(segment);
            if (evt == null)
            {
                return NotFound();
            }

            if (!CanAccessRegistration(evt))
            {
                return BadRequest(GetDisabledMessage(evt));
            }

            var cutoff = evt.TicketCutoffs.Where(x =>
                cstTime > x.OnSellDate && cstTime < x.CutoffDate).SingleOrDefault();
            if (cutoff == null)
            {
                return NotFound();
            }

            var discount = evt.TicketDiscounts.Where(x => x.TicketDiscountID == secondKey).SingleOrDefault();
            var discountArea = discount.TicketAreaDiscounts.Where(x => x.TicketAreaID == id).SingleOrDefault();

            if (discount == null)
            {
                return NotFound();
            }

            List<RegistrationAgeGroupApiModel> items = new List<RegistrationAgeGroupApiModel>();

            foreach (var price in cutoff.TicketPrices.Where(x => x.TicketAreaID == id).OrderBy(x => x.EventAgeGroup.MinimumAge))
            {
                if (discount.EventAgeGroups.Contains(price.EventAgeGroup))
                {
                    var item = new RegistrationAgeGroupApiModel
                    {
                        RegistrationAgeGroupID = price.EventAgeGroupID,
                        TicketPriceID = price.TicketPriceID,
                        Name = price.EventAgeGroup.Name,
                        AgeRange = (price.EventAgeGroup.MaximumAge.HasValue ? price.EventAgeGroup.MinimumAge + " - " + price.EventAgeGroup.MaximumAge : price.EventAgeGroup.MinimumAge + "+"),
                        AgeGroup = price.EventAgeGroup.Name + " (" + (price.EventAgeGroup.MaximumAge.HasValue ? price.EventAgeGroup.MinimumAge + " - " + price.EventAgeGroup.MaximumAge : price.EventAgeGroup.MinimumAge + "+") + ")",
                        BasePrice = price.BasePrice
                    };

                    if (discountArea != null)
                    {
                        //apply discount
                        switch (discountArea.AdjustmentType)
                        {
                            case AdjustmentType.Percent:
                                item.BasePrice = item.BasePrice - (item.BasePrice * (discountArea.AdjustmentAmount / 100));

                                break;
                            case AdjustmentType.Flat:
                                item.BasePrice = item.BasePrice - discountArea.AdjustmentAmount;
                                break;
                        }
                        item.BasePrice = item.BasePrice < 0 ? 0 : item.BasePrice;

                    }

                    item.Display = price.EventAgeGroup.Name + " (" + (price.EventAgeGroup.MaximumAge.HasValue ? price.EventAgeGroup.MinimumAge + " - " + price.EventAgeGroup.MaximumAge : price.EventAgeGroup.MinimumAge + "+") + ") " + item.BasePrice.ToString("c");

                    items.Add(item);
                }
            }

            return Ok(items);
        }

        [HttpGet]
        [ResponseType(typeof(List<RegistrationAreaApiModel>))]
        public IHttpActionResult Areas(string segment)
        {
            Event evt = RetrieveEvent(segment);

            if (evt == null)
            {
                return NotFound();
            }

            if (!CanAccessRegistration(evt))
            {
                return BadRequest(GetDisabledMessage(evt));
            }

            //get areas that have available tickets for the selected age group
            var cutoff = evt.TicketCutoffs.Where(x =>
                cstTime > x.OnSellDate && cstTime < x.CutoffDate).SingleOrDefault();
            if (cutoff == null)
            {
                return Ok(new List<RegistrationAreaApiModel>());
            }

            var prices = cutoff.TicketPrices;
            var areaGroups = prices.GroupBy(x => x.TicketArea);

            List<RegistrationAreaApiModel> items = new List<RegistrationAreaApiModel>();

            foreach (var areaGroup in areaGroups)
            {
                var area = areaGroup.Key;

                var areaItem = new RegistrationAreaApiModel
                {
                    RegistrationAreaID = areaGroup.Key.TicketAreaID,
                    IsGA = areaGroup.Key.IsGA,
                    Name = areaGroup.Key.Name,
                    AvailableTickets = areaGroup.Key.TicketSeats.Count(
                        x => x.TicketSeatStatus == TicketSeatStatus.Available
                        && (!x.PendingPurchaseExpiration.HasValue || x.PendingPurchaseExpiration < cstTime)),
                    RegistrationSections = area.TicketSections.Select(s => new RegistrationSectionApiModel
                    {
                        RegistrationSectionID = s.TicketSectionID,
                        Name = s.Name,
                        AvailableTickets = s.TicketRows.Sum(r => r.TicketSeats.Count(st =>
                            st.TicketSeatStatus == TicketSeatStatus.Available
                            && (!st.PendingPurchaseExpiration.HasValue || st.PendingPurchaseExpiration < cstTime))),
                        RegistrationRows = s.TicketRows.Select(r => new RegistrationRowApiModel
                        {
                            RegistrationRowID = r.TicketRowID,
                            Designation = r.Designation,
                            AvailableTickets = r.TicketSeats.Count(st => st.TicketSeatStatus == TicketSeatStatus.Available
                                && (!st.PendingPurchaseExpiration.HasValue || st.PendingPurchaseExpiration < cstTime))
                        }).ToList()
                    }).ToList()
                };

                items.Add(areaItem);
            }

            return Ok(items);
        }

        [HttpGet]
        [ResponseType(typeof(List<RegistrationAreaApiModel>))]
        public IHttpActionResult Areas(string segment, int id)
        {
            Event evt = RetrieveEvent(segment);
            if (evt == null)
            {
                return NotFound();
            }

            if (!CanAccessRegistration(evt))
            {
                return BadRequest(GetDisabledMessage(evt));
            }

            //get areas that have available tickets for the selected age group
            var cutoff = evt.TicketCutoffs.Where(x =>
                cstTime > x.OnSellDate && cstTime < x.CutoffDate).SingleOrDefault();
            if (cutoff == null)
            {
                return Ok(new List<RegistrationAreaApiModel>());
            }
            List<RegistrationAreaApiModel> items = new List<RegistrationAreaApiModel>();

            var discount = evt.TicketDiscounts.Where(x => x.TicketDiscountID == id).SingleOrDefault();

            if (discount == null)
            {
                return NotFound();
            }

            if (discount.TicketSeats.Where(x => x.TicketSeatStatus == TicketSeatStatus.Reserved).Count() == 0)
            {
                var prices = cutoff.TicketPrices;
                var areaGroups = prices.GroupBy(x => x.TicketArea);

                foreach (var areaGroup in areaGroups)
                {
                    var area = areaGroup.Key;

                    var areaItem = new RegistrationAreaApiModel
                    {
                        RegistrationAreaID = areaGroup.Key.TicketAreaID,
                        IsGA = areaGroup.Key.IsGA,
                        Name = areaGroup.Key.Name,
                        AvailableTickets = areaGroup.Key.TicketSeats.Count(
                            x => x.TicketSeatStatus == TicketSeatStatus.Available
                            && (!x.PendingPurchaseExpiration.HasValue || x.PendingPurchaseExpiration < cstTime)),
                        RegistrationSections = area.TicketSections.Select(s => new RegistrationSectionApiModel
                        {
                            RegistrationSectionID = s.TicketSectionID,
                            Name = s.Name,
                            AvailableTickets = s.TicketRows.Sum(r => r.TicketSeats.Count(st =>
                                st.TicketSeatStatus == TicketSeatStatus.Available
                                && (!st.PendingPurchaseExpiration.HasValue || st.PendingPurchaseExpiration < cstTime))),
                            RegistrationRows = s.TicketRows.Select(r => new RegistrationRowApiModel
                            {
                                RegistrationRowID = r.TicketRowID,
                                Designation = r.Designation,
                                AvailableTickets = r.TicketSeats.Count(st => st.TicketSeatStatus == TicketSeatStatus.Available
                                    && (!st.PendingPurchaseExpiration.HasValue || st.PendingPurchaseExpiration < cstTime))
                            }).ToList()
                        }).ToList()
                    };

                    items.Add(areaItem);
                }
            }
            else
            {
                //deal with reserved tickets
                var areaGroups = discount.TicketSeats.Where(x => x.TicketSeatStatus == TicketSeatStatus.Reserved).GroupBy(x => x.TicketArea);

                foreach (var areaGroup in areaGroups)
                {
                    var area = areaGroup.Key;
                    if (discount.TicketSeats.Where(x => x.TicketAreaID == area.TicketAreaID).Count() > 0)
                    {

                        var areaItem = new RegistrationAreaApiModel
                        {
                            RegistrationAreaID = areaGroup.Key.TicketAreaID,
                            IsGA = areaGroup.Key.IsGA,
                            Name = areaGroup.Key.Name,
                            AvailableTickets = areaGroup.Key.TicketSeats.Count(
                                x => x.TicketSeatStatus == TicketSeatStatus.Reserved
                                && (x.TicketDiscountID == discount.TicketDiscountID)
                                && (!x.PendingPurchaseExpiration.HasValue || x.PendingPurchaseExpiration < cstTime)),
                            RegistrationSections = area.TicketSections.Select(s => new RegistrationSectionApiModel
                            {
                                RegistrationSectionID = s.TicketSectionID,
                                Name = s.Name,
                                AvailableTickets = s.TicketRows.Sum(r => r.TicketSeats.Count(st =>
                                    st.TicketSeatStatus == TicketSeatStatus.Reserved
                                    && (st.TicketDiscountID == discount.TicketDiscountID)
                                    && (!st.PendingPurchaseExpiration.HasValue || st.PendingPurchaseExpiration < cstTime))),
                                RegistrationRows = s.TicketRows.Select(r => new RegistrationRowApiModel
                                {
                                    RegistrationRowID = r.TicketRowID,
                                    Designation = r.Designation,
                                    AvailableTickets = r.TicketSeats.Count(st => st.TicketSeatStatus == TicketSeatStatus.Reserved
                                                                    && (st.TicketDiscountID == discount.TicketDiscountID)
                                        && (!st.PendingPurchaseExpiration.HasValue || st.PendingPurchaseExpiration < cstTime))
                                }).ToList()
                            }).ToList()
                        };
                        items.Add(areaItem);
                    }
                }

            }
            return Ok(items);
        }
        
        [HttpGet]
        [ResponseType(typeof(List<RegistrationTicketOption>))]
        public IHttpActionResult TicketOptions(string segment, int id)
        {
            Event evt = RetrieveEvent(segment);
            if (evt == null)
            {
                return NotFound();
            }

            if (!CanAccessRegistration(evt))
            {
                return BadRequest(GetDisabledMessage(evt));
            }

            List<RegistrationTicketOption> response = new List<RegistrationTicketOption>();

            //gather non-expired options
            var options = evt.TicketOptions.Where(x => x.CutoffDate > cstTime).ToList();

            foreach (var option in options)
            {
                //if there are choices available
                var availableChoices =
                    option.TicketOptionChoices.Where(x =>
                        (!x.MaximumAvailable.HasValue || x.MaximumAvailable > x.Tickets.Count)
                        && x.EventAgeGroups.Count(y => y.EventAgeGroupID == id) > 0)
                    .ToList();
                if (availableChoices.Count > 0)
                {
                    RegistrationTicketOption responseOption = new RegistrationTicketOption
                    {
                        BasePrice = option.BasePrice,
                        Description = option.Description,
                        IsRequired = option.IsRequired,
                        Name = option.Name,
                        TicketOptionID = option.TicketOptionID,
                        TicketOptionChoices = new List<RegistrationTicketOptionChoice>()
                    };

                    //build choices
                    foreach (var choice in availableChoices)
                    {
                        decimal adjustedPrice = option.BasePrice + choice.MarkupPrice;

                        if (adjustedPrice <= 0)
                        {
                            adjustedPrice = 0;
                        }

                        responseOption.TicketOptionChoices.Add(new RegistrationTicketOptionChoice
                        {
                            AdjustedPrice = adjustedPrice,
                            Available = (choice.MaximumAvailable.HasValue ? choice.MaximumAvailable - choice.Tickets.Count : null),
                            Description = choice.Description,
                            Name = choice.Name,
                            TicketOptionChoiceID = choice.TicketOptionChoiceID
                        });
                    }

                    response.Add(responseOption);
                }
            }


            return Ok(response);
        }

        [HttpGet]
        [ResponseType(typeof(List<RegistrationTicketOption>))]
        public IHttpActionResult TicketOptions(string segment, int id, int secondKey)
        {
            Event evt = RetrieveEvent(segment);
            if (evt == null)
            {
                return NotFound();
            }

            if (!CanAccessRegistration(evt))
            {
                return BadRequest(GetDisabledMessage(evt));
            }

            TicketDiscount discount = evt.TicketDiscounts.Where(x => x.TicketDiscountID == secondKey).SingleOrDefault();
            if (discount == null)
            {
                return NotFound();
            }

            List<RegistrationTicketOption> response = new List<RegistrationTicketOption>();

            //gather non-expired options
            var options = evt.TicketOptions.Where(x => x.CutoffDate > cstTime).ToList();

            foreach (var option in options)
            {
                var discountOption = discount.TicketOptionDiscounts.Where(x => x.TicketOptionID == option.TicketOptionID).SingleOrDefault();

                //if there are choices available
                var availableChoices =
                    option.TicketOptionChoices.Where(x =>
                        (!x.MaximumAvailable.HasValue || x.MaximumAvailable > x.Tickets.Count)
                        && x.EventAgeGroups.Count(y => y.EventAgeGroupID == id) > 0)
                    .ToList();
                if (availableChoices.Count > 0)
                {
                    RegistrationTicketOption responseOption = new RegistrationTicketOption
                    {
                        BasePrice = option.BasePrice,
                        Description = option.Description,
                        IsRequired = option.IsRequired,
                        Name = option.Name,
                        TicketOptionID = option.TicketOptionID,
                        TicketOptionChoices = new List<RegistrationTicketOptionChoice>()
                    };

                    //build choices
                    foreach (var choice in availableChoices)
                    {
                        decimal adjustedPrice = option.BasePrice + choice.MarkupPrice;

                        if (discountOption != null)
                        {
                            switch (discountOption.AdjustmentType)
                            {
                                case AdjustmentType.Percent:
                                    adjustedPrice = adjustedPrice - (adjustedPrice * (discountOption.AdjustmentAmount / 100));
                                    break;
                                case AdjustmentType.Flat:
                                    adjustedPrice -= discountOption.AdjustmentAmount;
                                    break;
                            }
                        }

                        if (adjustedPrice <= 0)
                        {
                            adjustedPrice = 0;
                        }

                        responseOption.TicketOptionChoices.Add(new RegistrationTicketOptionChoice
                        {
                            AdjustedPrice = adjustedPrice,
                            Available = (choice.MaximumAvailable.HasValue ? choice.MaximumAvailable - choice.Tickets.Count : null),
                            Description = choice.Description,
                            Name = choice.Name,
                            TicketOptionChoiceID = choice.TicketOptionChoiceID
                        });
                    }

                    response.Add(responseOption);
                }
            }


            return Ok(response);
        }
        
        [HttpGet]
        [ResponseType(typeof(RegistrationCutoffApiModel))]
        public IHttpActionResult RegistrationPrices(string segment)
        {

            Event evt = RetrieveEvent(segment);
            if (evt == null)
            {
                return NotFound();
            }

            if (!CanAccessRegistration(evt))
            {
                return BadRequest(GetDisabledMessage(evt));
            }

            //get areas that have available tickets for the selected age group
            var cutoff = evt.TicketCutoffs.Where(x =>
                cstTime > x.OnSellDate && cstTime < x.CutoffDate).SingleOrDefault();
            if (cutoff == null)
            {
                return Ok(new List<RegistrationCutoffAreaApiModel>());
            }

            RegistrationCutoffApiModel item = new RegistrationCutoffApiModel
            {
                CutoffDate = cutoff.CutoffDate.ToShortDateString(),
                OnSellDate = cutoff.OnSellDate.ToShortDateString(),
                Areas = new List<RegistrationCutoffAreaApiModel>()
            };

            foreach (var area in cutoff.TicketPrices.GroupBy(x => x.TicketArea))
            {
                var prices = cutoff.TicketPrices.Where(x => x.TicketAreaID == area.Key.TicketAreaID).OrderBy(x => x.EventAgeGroup.MinimumAge);
                item.Areas.Add(new RegistrationCutoffAreaApiModel
                {
                    AreaID = area.Key.TicketAreaID,
                    Name = area.Key.Name,
                    Prices = prices.Select(x => new RegistrationCutoffPriceApiModel
                    {
                        AgeName = x.EventAgeGroup.Name,
                        AgeRange = (x.EventAgeGroup.MaximumAge.HasValue ? x.EventAgeGroup.MinimumAge + " - " + x.EventAgeGroup.MaximumAge : x.EventAgeGroup.MinimumAge + "+"),
                        BasePrice = x.BasePrice
                    }).ToList()
                });
            }

            return Ok(item);
        }

        [HttpGet]
        [ResponseType(typeof(RegistrationCutoffApiModel))]
        public IHttpActionResult RegistrationPrices(string segment, int id)
        {

            Event evt = RetrieveEvent(segment);
            if (evt == null)
            {
                return NotFound();
            }

            if (!CanAccessRegistration(evt))
            {
                return BadRequest(GetDisabledMessage(evt));
            }

            TicketDiscount discount = evt.TicketDiscounts.Where(x => x.TicketDiscountID == id).SingleOrDefault();
            if (discount == null)
            {
                return NotFound();
            }

            //get areas that have available tickets for the selected age group
            var cutoff = evt.TicketCutoffs.Where(x =>
                cstTime > x.OnSellDate && cstTime < x.CutoffDate).SingleOrDefault();
            if (cutoff == null)
            {
                return Ok(new List<RegistrationCutoffAreaApiModel>());
            }

            RegistrationCutoffApiModel item = new RegistrationCutoffApiModel
            {
                CutoffDate = cutoff.CutoffDate.ToShortDateString(),
                OnSellDate = cutoff.OnSellDate.ToShortDateString(),
                Areas = new List<RegistrationCutoffAreaApiModel>()
            };

            List<int> discountAreas = discount.TicketSeats.Select(x => x.TicketAreaID).Distinct().ToList();
            foreach (var area in cutoff.TicketPrices.Where(x => discountAreas.Contains(x.TicketAreaID)).GroupBy(x => x.TicketArea))
            {
                TicketAreaDiscount areaDiscount = discount.TicketAreaDiscounts.Where(x => x.TicketAreaID == area.Key.TicketAreaID).Single();

    var prices = cutoff.TicketPrices.Where(x => x.TicketAreaID == area.Key.TicketAreaID).OrderBy(x => x.EventAgeGroup.MinimumAge);
                item.Areas.Add(new RegistrationCutoffAreaApiModel
                {
                    AreaID = area.Key.TicketAreaID,
                    Name = area.Key.Name,
                    // TODO: Adjust price by discount
                    Prices = prices.Select(x => new RegistrationCutoffPriceApiModel
                    {
                        AgeName = x.EventAgeGroup.Name,
                        AgeRange = (x.EventAgeGroup.MaximumAge.HasValue ? x.EventAgeGroup.MinimumAge + " - " + x.EventAgeGroup.MaximumAge : x.EventAgeGroup.MinimumAge + "+"),
                        BasePrice = CalculateDiscountedPrice(areaDiscount.AdjustmentType, x.BasePrice, areaDiscount.AdjustmentAmount)
                    }).ToList()
                });
            }

            return Ok(item);
        }


        [HttpGet]
        [ResponseType(typeof(string))]
        public IHttpActionResult CheckEventDisabled(string segment)
        {
            Event evt = RetrieveEvent(segment);
            if (evt == null)
            {
                return NotFound();
            } else if (IsPublicallyOpen(evt) || User.IsInRole("Cashier"))
            {
                return Ok();
            }
            else
            {
                return BadRequest(GetDisabledMessage(evt));
            }
        }

        [HttpGet]
        [ResponseType(typeof(RegistrationCutoffApiModel))]
        public IHttpActionResult GetRegistrationEvent(string segment, int id)
        {

            Event evt = RetrieveEvent(segment);
            if (evt == null)
            {
                return NotFound();
            }

            //get areas that have available tickets for the selected age group
            var cutoff = evt.TicketCutoffs.Where(x =>
                cstTime > x.OnSellDate && cstTime < x.CutoffDate).SingleOrDefault();
            if (cutoff == null)
            {
                return Ok(new List<RegistrationCutoffAreaApiModel>());
            }

            RegistrationCutoffApiModel item = new RegistrationCutoffApiModel
            {
                CutoffDate = cutoff.CutoffDate.ToShortDateString(),
                OnSellDate = cutoff.OnSellDate.ToShortDateString(),
                Areas = new List<RegistrationCutoffAreaApiModel>()
            };

            var discount = evt.TicketDiscounts.Where(x => x.TicketDiscountID == id).SingleOrDefault();

            if (discount == null)
            {
                return NotFound();
            }

            if (discount.TicketSeats.Count == 0)
            {
                foreach (var area in cutoff.TicketPrices.GroupBy(x => x.TicketArea))
                {
                    var prices = cutoff.TicketPrices.Where(x => x.TicketAreaID == area.Key.TicketAreaID).OrderBy(x => x.EventAgeGroup.MinimumAge);
                    var regArea = new RegistrationCutoffAreaApiModel
                    {
                        AreaID = area.Key.TicketAreaID,
                        Name = area.Key.Name,
                        Prices = new List<RegistrationCutoffPriceApiModel>()
                    };

                    foreach (var price in prices)
                    {
                        if (discount.EventAgeGroups.Contains(price.EventAgeGroup))
                        {
                            //discount limited to certain ages
                            var areaPrice = new RegistrationCutoffPriceApiModel
                            {
                                AgeName = price.EventAgeGroup.Name,
                                AgeRange = (price.EventAgeGroup.MaximumAge.HasValue ? price.EventAgeGroup.MinimumAge + " - " + price.EventAgeGroup.MaximumAge : price.EventAgeGroup.MinimumAge + "+"),
                                BasePrice = price.BasePrice
                            };

                            var discountArea = discount.TicketAreaDiscounts.Where(x => x.TicketAreaID == area.Key.TicketAreaID).SingleOrDefault();
                            if (discountArea != null)
                            {
                                //apply discount
                                areaPrice.BasePrice = CalculateDiscountedPrice(discountArea.AdjustmentType, areaPrice.BasePrice, discountArea.AdjustmentAmount);
                            }

                            regArea.Prices.Add(areaPrice);
                        }
                    }



                    item.Areas.Add(regArea);
                }
            }
            else
            {
                //reserved seats
                foreach (var area in discount.TicketSeats.GroupBy(x => x.TicketArea))
                {
                    var prices = cutoff.TicketPrices.Where(x => x.TicketAreaID == area.Key.TicketAreaID).OrderBy(x => x.EventAgeGroup.MinimumAge);
                    var regArea = new RegistrationCutoffAreaApiModel
                    {
                        AreaID = area.Key.TicketAreaID,
                        Name = area.Key.Name,
                        Prices = new List<RegistrationCutoffPriceApiModel>()
                    };

                    foreach (var price in prices)
                    {
                        if (discount.EventAgeGroups.Contains(price.EventAgeGroup))
                        {
                            //discount limited to certain ages
                            var areaPrice = new RegistrationCutoffPriceApiModel
                            {
                                AgeName = price.EventAgeGroup.Name,
                                AgeRange = (price.EventAgeGroup.MaximumAge.HasValue ? price.EventAgeGroup.MinimumAge + " - " + price.EventAgeGroup.MaximumAge : price.EventAgeGroup.MinimumAge + "+"),
                                BasePrice = price.BasePrice
                            };

                            var discountArea = discount.TicketAreaDiscounts.Where(x => x.TicketAreaID == area.Key.TicketAreaID).SingleOrDefault();
                            if (discountArea != null)
                            {
                                //apply discount
                                switch (discountArea.AdjustmentType)
                                {
                                    case AdjustmentType.Percent:
                                        areaPrice.BasePrice = areaPrice.BasePrice - (areaPrice.BasePrice * (discountArea.AdjustmentAmount / 100));

                                        break;
                                    case AdjustmentType.Flat:
                                        areaPrice.BasePrice = areaPrice.BasePrice - discountArea.AdjustmentAmount;
                                        break;
                                }
                                areaPrice.BasePrice = areaPrice.BasePrice < 0 ? 0 : areaPrice.BasePrice;

                            }

                            regArea.Prices.Add(areaPrice);
                        }
                    }



                    item.Areas.Add(regArea);
                }

            }

            return Ok(item);
        }

        private static decimal CalculateDiscountedPrice(AdjustmentType adjustmentType, decimal basePrice, decimal adjustmentAmount)
        {
            switch (adjustmentType)
            {
                case AdjustmentType.Percent:
                    basePrice = basePrice - (basePrice * (adjustmentAmount / 100));

                    break;
                case AdjustmentType.Flat:
                    basePrice = basePrice - adjustmentAmount;
                    break;
            }
            basePrice = basePrice < 0 ? 0 : basePrice;
            return basePrice;
        }

        [HttpGet]
        public IHttpActionResult RetrieveDiscounts(string segment)
        {
            if (User.IsInRole("Cashier"))
            {
                Event evt = RetrieveEvent(segment);
                if (evt == null)
                {
                    return NotFound();
                }
                var discounts = new List<RegistrationDiscountResponse>();

                foreach (var item in evt.TicketDiscounts.Where(x => x.CutoffDate > cstTime).ToList())
                {
                    int? maxAvailable = item.MaximumAvailable.HasValue ?
(item.MaximumAvailable - 
item.TicketSeats.Count(
    x => x.TicketSeatStatus == TicketSeatStatus.Assigned 
    || x.TicketSeatStatus == TicketSeatStatus.Complete
    || (x.TicketSeatStatus == TicketSeatStatus.Available && x.PendingPurchaseExpiration > cstTime)))
: null;
                    if (maxAvailable.HasValue && maxAvailable > 0)
                    {
                        discounts.Add(new RegistrationDiscountResponse
                        {
                            Name = item.Name,
                            DiscountConfirmationNumber = item.SecurityConfirmationNumber,
                            MaxAvailable = maxAvailable,
                            TicketDiscountID = item.TicketDiscountID,
                            Code = item.Code
                        });
                    }
                }
                return Ok(discounts);
            }
            else
            {
                return Ok();
            }
        }

        [HttpPost]
        [ResponseType(typeof(RegistrationDiscountResponse))]
        public async Task<IHttpActionResult> VerifyDiscount(string segment, RegistrationVerifyDiscount model)
        {
            Event evt = RetrieveEvent(segment);
            if (evt == null)
            {
                return NotFound();
            }

            if (!CanAccessRegistration(evt))
            {
                return BadRequest(GetDisabledMessage(evt));
            }

            //find discount that matches code
            var discount = evt.TicketDiscounts.Where(x => x.Code.ToLower() == model.Code.ToLower()).SingleOrDefault();


            //bad code
            if (discount == null)
            {
                await Logger.WriteLog(LogType.Error, string.Format("TICKETS - Bad Code {0} attempted by {1}", model.Code, HttpContext.Current.Request.UserHostAddress));
                return BadRequest("BadCode");

            }

            //expired
            if (discount.CutoffDate < cstTime)
            {
                await Logger.WriteLog(LogType.Warning, string.Format("TICKETS - Expired Discount {0} requested to be used by {1}", discount.Name, HttpContext.Current.Request.UserHostAddress));
                return BadRequest("ExpiredCode");
            }
            int? maxAvailable = discount.MaximumAvailable.HasValue ?
    (discount.MaximumAvailable - discount.TicketSeats.Count(x => x.TicketSeatStatus == TicketSeatStatus.Assigned 
    || x.TicketSeatStatus == TicketSeatStatus.Complete
    || (x.TicketSeatStatus == TicketSeatStatus.Available && x.PendingPurchaseExpiration > cstTime)))
    : null;

            //all gone
            //if (discount.MaximumAvailable.HasValue 
            //    && discount.MaximumAvailable <= discount.TicketSeats.Count(x => x.TicketSeatStatus == TicketSeatStatus.Assigned || x.TicketSeatStatus == TicketSeatStatus.Complete))
            if (maxAvailable.HasValue && maxAvailable <= 0)
            {
                await Logger.WriteLog(LogType.Warning, string.Format("TICKETS - Discount has no tickets left {0} requested to be used by {1}", discount.Name, HttpContext.Current.Request.UserHostAddress));
                return BadRequest("NoneLeft");

            }

            RegistrationDiscountResponse response = new RegistrationDiscountResponse
            {
                Name = discount.Name,
                TicketDiscountID = discount.TicketDiscountID,
                DiscountConfirmationNumber = discount.SecurityConfirmationNumber,
                MaxAvailable = maxAvailable
            };
            return Ok(response);

        }

        [HttpDelete]
        public async Task<IHttpActionResult> UnreserveTicket(string segment, int id)
        {
            Event evt = RetrieveEvent(segment);
            if (evt == null)
            {
                return NotFound();
            }

            if (!CanAccessRegistration(evt))
            {
                return BadRequest(GetDisabledMessage(evt));
            }

            var seat = await seatsRepository.RetrieveAsync(id);

            if (seat == null)
            {
                return NotFound();
            }

            seat.PendingPurchaseConfirmation = null;
            seat.PendingPurchaseExpiration = null;
            await seatsRepository.UpdateAsync(seat, seat.TicketSeatID);

            return Ok();

        }

        [HttpPut]
        [ResponseType(typeof(RegistrationResetExpirationResponseModel))]
        public async Task<IHttpActionResult> ResetExpirations(string segment, RegistrationResetExpirationRequestModel model)
        {
            Event evt = RetrieveEvent(segment);
            if (evt == null)
            {
                return NotFound();
            }

            if (!CanAccessRegistration(evt))
            {
                return BadRequest(GetDisabledMessage(evt));
            }

            var expiration = cstTime.AddMinutes(RegistrationExpirationMinutes);
            RegistrationResetExpirationResponseModel response = new RegistrationResetExpirationResponseModel()
            {
                ExpirationDate = expiration.ToString("MM/dd/yyyy HH:mm:ss"),
                UpdatedTicketSeats = new List<int>()
            };

            foreach (var item in model.TicketSeats)
            {
                var seat = await seatsRepository.RetrieveAsync(item.TicketSeatID);
                if (seat.PendingPurchaseConfirmation == item.PendingPurchaseConfirmation)
                {
                    seat.PendingPurchaseExpiration = expiration;
                    await seatsRepository.UpdateAsync(seat, seat.TicketSeatID);
                    response.UpdatedTicketSeats.Add(item.TicketSeatID);
                }
            }

            return Ok(response);
        }

        [HttpPost]
        [ResponseType(typeof(RegistrationTicketResponseModel))]
        public async Task<IHttpActionResult> AddTickets(string segment, RegistrationAddTicketsApiModel addTicketsModel)
        {
            Event evt = RetrieveEvent(segment);
            if (evt == null)
            {
                return NotFound();
            }

            if (!CanAccessRegistration(evt))
            {
                return BadRequest(GetDisabledMessage(evt));
            }

            List<TicketSeat> seats = new List<TicketSeat>();
            string location = "";

            TicketDiscount discount = null;
            if (addTicketsModel.TicketDiscountID.HasValue)
            {
                discount = evt.TicketDiscounts.Where(x => x.TicketDiscountID == addTicketsModel.TicketDiscountID).SingleOrDefault();
                if (discount == null)
                {
                    return NotFound();
                }
                int? maxAvailable = discount.MaximumAvailable.HasValue ?
                    (discount.MaximumAvailable - discount.TicketSeats.Count(x => x.TicketSeatStatus == TicketSeatStatus.Assigned || x.TicketSeatStatus == TicketSeatStatus.Complete))
                    : null;
                if (maxAvailable.HasValue && maxAvailable < addTicketsModel.Tickets.Count)
                {
                    return BadRequest();
                }
            }

            var time = cstTime;

            if (discount == null || discount.TicketSeats.Where(x => x.TicketSeatStatus == TicketSeatStatus.Reserved).Count() == 0)
            {
                if (addTicketsModel.RowID.HasValue)
                {

                    seats = seatsRepository.Items.Where(x => (x.TicketRowID == addTicketsModel.RowID)
                    && (x.TicketSeatStatus == TicketSeatStatus.Available)
                    && (!x.PendingPurchaseExpiration.HasValue || (x.PendingPurchaseExpiration.Value < time))
                    ).OrderBy(x => Guid.NewGuid()).Take(addTicketsModel.SeatCount).ToList();

                    var singleSeat = seats.FirstOrDefault();
                    if (singleSeat != null)
                    {
                        location = singleSeat.TicketArea.Name + " " + singleSeat.TicketRow.TicketSection.Name + " " + singleSeat.TicketRow.Designation;
                    }
                }
                else
                {
                    seats = seatsRepository.Items.Where(x => x.TicketSeatStatus == TicketSeatStatus.Available
                        && (!x.PendingPurchaseExpiration.HasValue || x.PendingPurchaseExpiration.Value < time)
                        && x.TicketAreaID == addTicketsModel.AreaID).Take(addTicketsModel.SeatCount).ToList();
                    var singleSeat = seats.FirstOrDefault();
                    if (singleSeat != null)
                    {
                        location = singleSeat.TicketArea.Name;
                    }
                }
            }
            else
            {
                //dealing with reservations
                if (addTicketsModel.RowID.HasValue)
                {
                    seats = seatsRepository.Items.Where(x => x.TicketSeatStatus == TicketSeatStatus.Reserved
                        && x.TicketDiscountID == discount.TicketDiscountID
                        && (!x.PendingPurchaseExpiration.HasValue || x.PendingPurchaseExpiration.Value < time)
                        && x.TicketRowID == addTicketsModel.RowID).Take(addTicketsModel.SeatCount).ToList();
                    var singleSeat = seats.FirstOrDefault();
                    if (singleSeat != null)
                    {
                        location = singleSeat.TicketArea.Name + " " + singleSeat.TicketRow.TicketSection.Name + " " + singleSeat.TicketRow.Designation;
                    }
                }
                else
                {
                    seats = seatsRepository.Items.Where(x => x.TicketSeatStatus == TicketSeatStatus.Reserved
                        && x.TicketDiscountID == discount.TicketDiscountID
                        && (!x.PendingPurchaseExpiration.HasValue || x.PendingPurchaseExpiration.Value < time)
                        && x.TicketAreaID == addTicketsModel.AreaID).Take(addTicketsModel.SeatCount).ToList();
                    var singleSeat = seats.FirstOrDefault();
                    if (singleSeat != null)
                    {
                        location = singleSeat.TicketArea.Name;
                    }
                }
            }
            var expiration = cstTime.AddMinutes(RegistrationExpirationMinutes);

            RegistrationTicketResponseModel response = new RegistrationTicketResponseModel
            {
                Tickets = new List<PendingTicketModel>()
            };

            var i = 0;
            foreach (var item in seats)
            {
                PendingTicketModel ticket = new PendingTicketModel
                {
                    ConfirmationNumber = Guid.NewGuid().ToString(),
                    TicketPriceID = addTicketsModel.Tickets[i].TicketPriceID,
                    TicketSeatID = item.TicketSeatID,
                    ExpirationTime = expiration.ToString("MM/dd/yyyy HH:mm:ss")
                };

                if (addTicketsModel.TicketDiscountID.HasValue)
                {
                    item.TicketDiscountID = addTicketsModel.TicketDiscountID;
                }

                item.PendingPurchaseConfirmation = ticket.ConfirmationNumber;
                item.PendingPurchaseExpiration = expiration;
                await seatsRepository.UpdateAsync(item, item.TicketSeatID);
                i++;
                response.Tickets.Add(ticket);
            }

            return Ok(response);
        }

        private string GetDisabledMessage(Event evt)
        {
            return string.IsNullOrEmpty(evt.RegistrationDisabledMessage) ? "This event is not currently open for registration." : evt.RegistrationDisabledMessage;
        }

        private Event RetrieveEvent(string segment)
        {
            //check for series event
            var evt = seriesRepository.RetrievePublicSingle(segment);

            if (evt == null)
            {
                //check for regular event
                evt = eventRepository.RetrieveByUrlSegment(segment);
            }

            return evt;
        }

        private bool IsPublicallyOpen(Event evt)
        {
            return !evt.ForceDisableRegistration || evt.DisablePublicRegistration >= cstTime;
        }

        private bool CanAccessRegistration(Event evt)
        {
            return IsPublicallyOpen(evt) || User.IsInRole("Cashier");
        }



    }
}