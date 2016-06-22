using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DexCMS.Tickets.Tickets.Interfaces;
using DexCMS.Tickets.Tickets.Models;

namespace DexCMS.Tickets.WebApi.Controllers
{
    public class SecureTicketOptionsController : ApiController
    {
        private ITicketOptionRepository repository;

        public SecureTicketOptionsController(ITicketOptionRepository repo)
        {
            repository = repo;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            TicketOption option = await repository.RetrieveAsync(id);
            if (option == null)
            {
                return NotFound();
            }

            Dictionary<string, string> queryStrings = Request.GetQueryNameValuePairs().ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);

            int choiceID = int.Parse(queryStrings["choiceid"]);
            int? discountID = null;
            string discountConfirmation = "";

            if (queryStrings.ContainsKey("discountid"))
            {
                discountID = int.Parse(queryStrings["discountid"]);
            }

            if (queryStrings.ContainsKey("discountconfirmation"))
            {
                discountConfirmation = queryStrings["discountconfirmation"];
            }

            TicketOptionChoice choice = option.TicketOptionChoices.Where(x => x.TicketOptionChoiceID == choiceID).SingleOrDefault();

            if (choice == null)
            {
                return NotFound();
            }

            TicketOptionDiscount optionDiscount = null;
            if (discountID.HasValue)
            {
                TicketDiscount discount = option.Event.TicketDiscounts.Where(x => x.TicketDiscountID == discountID && x.SecurityConfirmationNumber == discountConfirmation).SingleOrDefault();
                if (discount == null)
                {
                    return NotFound();
                }
                optionDiscount = discount.TicketOptionDiscounts.Where(x => x.TicketOptionID == option.TicketOptionID).SingleOrDefault();
            }

            decimal price = option.BasePrice;

            price += choice.MarkupPrice;

            price = price < 0 ? 0 : price;

            if (optionDiscount != null)
            {
                switch (optionDiscount.AdjustmentType)
                {
                    case AdjustmentType.Percent:
                        price = price - (price * (optionDiscount.AdjustmentAmount / 100));
                        break;
                    case AdjustmentType.Flat:
                        price = price - optionDiscount.AdjustmentAmount;
                        break;
                    default:
                        break;
                }
                if (price < 0)
                {
                    price = 0;
                }
            }

            return Ok(string.Format("{0}: {1} +{2:c}", option.Name, choice.Name, price));
        }
    }
}
