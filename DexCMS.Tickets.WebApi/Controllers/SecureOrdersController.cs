using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using DexCMS.Tickets.Orders.Interfaces;
using DexCMS.Tickets.Orders.Models;
using DexCMS.Tickets.Tickets.Interfaces;
using DexCMS.Tickets.Tickets.Models;
using DexCMS.Tickets.WebApi.ApiModels;
using DexCMS.Tickets.WebApi.Payments;
using DexCMS.Core.Infrastructure;

namespace DexCMS.Tickets.WebApi.Controllers
{
    [Authorize]
    public class SecureOrdersController : ApiController
    {
        private IOrderRepository repository;
        private ITicketSeatRepository seatRepository;
        private ITicketPriceRepository priceRepository;
        private ITicketDiscountRepository discountRepository;
        private ITicketOptionChoiceRepository choiceRepository;
        private DateTime cstTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Central Standard Time");

        public SecureOrdersController(
            IOrderRepository repo,
            ITicketSeatRepository seatRepo,
            ITicketPriceRepository priceRepo,
            ITicketDiscountRepository discountRepo,
            ITicketOptionChoiceRepository choiceRepo)
        {
            repository = repo;
            seatRepository = seatRepo;
            priceRepository = priceRepo;
            discountRepository = discountRepo;
            choiceRepository = choiceRepo;
        }

        [HttpGet]
        [ResponseType(typeof(List<SecureOrderApiModel>))]
        public async Task<IHttpActionResult> Get()
        {
            var allOrders = repository.RetrieveUserOrders(User.Identity.Name)
                .OrderByDescending(x => x.EnteredOn).ToList();
            int RegistrationExpirationMinutes = int.Parse(WebConfigurationManager.AppSettings["RegistrationExpirationMinutes"]);


            List<SecureOrderApiModel> orders = new List<SecureOrderApiModel>();

            foreach (var x in allOrders)
            {
                if (x.OrderStatus == OrderStatus.Complete)
                {
                    orders.Add(new SecureOrderApiModel
                    {
                        OrderID = x.OrderID,
                        EnteredOn = x.EnteredOn,
                        OrderStatus = x.OrderStatus.ToString(),
                        OrderTotal = x.OrderTotal,
                        UserName = x.UserName,
                        TicketsDetails = x.Tickets.GroupBy(y => y.TicketSeat.TicketArea.Event)
                                        .Select(grp => new TicketsDetail
                                        {
                                            EventName = grp.Key.PageContent.Heading,
                                            TicketCount = grp.ToList().Count
                                        }).ToList(),
                        IsUpdated = x.Tickets.Where(y => string.IsNullOrEmpty(y.FirstName) && string.IsNullOrEmpty(y.LastName)).Count() == 0
                    });
                }
                else if (x.OrderStatus == OrderStatus.Pending)
                {
                    //check if order should be cancelled
                    if (x.EnteredOn.AddMinutes(RegistrationExpirationMinutes) < cstTime)
                    {
                        await repository.DeleteAsync(x);
                    }
                }
            }

            return Ok(orders);
        }

        [HttpGet]
        [ResponseType(typeof(SecureOrderApiModel))]
        public async Task<IHttpActionResult> Get(int id)
        {
            var order = await repository.RetrieveAsync(id);
            if (order == null || order.UserName != User.Identity.Name)
            {
                return NotFound();
            }

            var response = new SecureOrderApiModel
            {
                OrderID = order.OrderID,
                EnteredOn = order.EnteredOn,
                OrderStatus = order.OrderStatus.ToString(),
                OrderTotal = order.OrderTotal,
                UserName = order.UserName,
                Tickets = new List<SecureTicketSeatApiModel>()
            };

            foreach (var x in order.Tickets)
            {
                var ticket = new SecureTicketSeatApiModel
                {
                    ConfirmationNumber = x.TicketSeat.PendingPurchaseConfirmation,
                    TicketSeatID = x.TicketSeat.TicketSeatID,
                    TicketPriceID = x.TicketPriceID,
                    AgeID = x.TicketPrice.EventAgeGroupID,
                    BasePrice = x.TicketPrice.BasePrice,
                    TotalPrice = x.TicketTotalPrice,
                    EventName = x.TicketSeat.TicketArea.Event.PageContent.Heading,
                    TicketDiscountID = x.TicketDiscountID,
                    DiscountConfirmationNumber = x.TicketDiscount?.SecurityConfirmationNumber,
                    Options = new Dictionary<int, int>(),
                    IsValid = true
                };

                foreach (var y in x.TicketOptionChoices)
                {
                    ticket.Options.Add(y.TicketOptionID, y.TicketOptionChoiceID);
                }
                response.Tickets.Add(ticket);
            }

            return Ok(response);
        }

        [HttpPost]
        [ResponseType(typeof(OrderCreateResponseModel))]
        public async Task<IHttpActionResult> Post(SecureTicketSeatApiModel[] model)
        {
            if (model == null || model.Length == 0)
            {
                return Ok();
            }

            //Create Order
            DexCMS.Tickets.Orders.Models.Order order = new DexCMS.Tickets.Orders.Models.Order
            {
                EnteredOn = cstTime,
                OrderStatus = OrderStatus.Pending,
                OrderTicketReferences = new List<OrderTicketReference>(),
                UserName = User.Identity.Name
            };

            foreach (var item in model)
            {
                OrderTicketReference ticketRef = new OrderTicketReference
                {
                    TicketSeatID = item.TicketSeatID,
                    PendingPurchaseConfirmation = item.ConfirmationNumber,
                    EventAgeGroupID = item.AgeID,
                    TicketDiscountID = item.TicketDiscountID,
                    SecurityConfirmationNumber = item.DiscountConfirmationNumber,
                    TotalPrice = item.TotalPrice
                };

                if (item.Options != null && item.Options.Count > 0)
                {
                    ticketRef.TicketOptionChoices = new List<int>();
                    foreach (KeyValuePair<int, int> optionChoice in item.Options)
                    {
                        ticketRef.TicketOptionChoices.Add(optionChoice.Value);
                    }
                }

                order.OrderTicketReferences.Add(ticketRef);
            }

            //save
            await repository.AddAsync(order);

            if (order.OrderTotal > 0)
            {
                //Create order with paypal
                var payment = CreatePayment(order);

                var createdPayment = payment.Create(PaypalConfiguration.GetAPIContext());
                var links = createdPayment.links.GetEnumerator();
                var approvalUrl = "";
                while (links.MoveNext())
                {
                    var link = links.Current;
                    if (link.rel.ToLower().Trim().Equals("approval_url"))
                    {
                        approvalUrl = link.href;
                    }
                }

                return Ok(new
                {
                    OrderID = order.OrderID,
                    ApprovalUrl = approvalUrl
                });
            }
            else
            {
                order.Payments = new List<DexCMS.Tickets.Orders.Models.Payment>();

                order.Payments.Add(new DexCMS.Tickets.Orders.Models.Payment
                {
                    GrossPaid = 0,
                    NetPaid = 0,
                    OrderID = order.OrderID,
                    PaidOn = DateTime.Now,
                    PaymentDetails = "{\"type\":\"Automatic\"}",
                    PaymentFee = 0,
                    PaymentType = PaymentType.NoCharge
                });

                order.OrderStatus = OrderStatus.Complete;

                await repository.UpdateAsync(order, order.OrderID);

                return Ok(new { OrderID = order.OrderID });
            }
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var order = await repository.RetrieveAsync(id);

            await repository.DeleteAsync(order);
            return Ok();
        }

        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, SecureOrderApiModel model)
        {
            var order = await repository.RetrieveAsync(id);

            if (order != null && order.UserName == User.Identity.Name)
            {
                var apiContext = PaypalConfiguration.GetAPIContext();
                var payment = PayPal.Api.Payment.Get(apiContext, model.PaymentID);
                if (payment.state == "approved")
                {
                    var transaction = payment.transactions.ToArray()[0];
                    var sale = transaction.related_resources.ToArray()[0].sale;

                    if (order.Payments == null)
                    {
                        order.Payments = new List<DexCMS.Tickets.Orders.Models.Payment>();
                    }

                    order.Payments.Add(new DexCMS.Tickets.Orders.Models.Payment
                    {
                        GrossPaid = decimal.Parse(transaction.amount.total),
                        NetPaid = decimal.Parse(transaction.amount.total) - decimal.Parse(sale.transaction_fee.value),
                        OrderID = order.OrderID,
                        PaidOn = Convert.ToDateTime(payment.update_time),
                        PaymentDetails = payment.ConvertToJson(),
                        PaymentFee = decimal.Parse(sale.transaction_fee.value),
                        PaymentType = PaymentType.Paypal
                    });

                    order.OrderStatus =
                        order.OrderTotal == decimal.Parse(transaction.amount.total) ? OrderStatus.Complete : OrderStatus.Partial;

                    await repository.UpdateAsync(order, order.OrderID);

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }

        }

        private PayPal.Api.Payment CreatePayment(DexCMS.Tickets.Orders.Models.Order order)
        {
            var payer = new Payer { payment_method = "paypal" };
            List<Transaction> transactions = BuildTransactions(order);
            var baseUrl = WebConfigurationManager.AppSettings["ServerUrl"] + "secure/";
            var payment = new PayPal.Api.Payment
            {
                intent = "sale",
                payer = payer,
                transactions = transactions,
                redirect_urls = new RedirectUrls
                {
                    cancel_url = baseUrl + "cancel/" + order.OrderID,
                    return_url = baseUrl + "payment"
                }
            };

            return payment;
        }

        private List<Transaction> BuildTransactions(DexCMS.Tickets.Orders.Models.Order order)
        {
            var details = new Details { shipping = "0.00", tax = "0.00", subtotal = order.OrderTotal.ToString() };
            var amount = new Amount { currency = "USD", details = details, total = order.OrderTotal.ToString() };
            var itemList = new ItemList() { items = new List<Item>() };

            foreach (var ticket in order.Tickets)
            {
                itemList.items.Add(new Item
                {
                    name = GetTicketName(ticket),
                    currency = "USD",
                    price = ticket.TicketTotalPrice.ToString(),
                    quantity = "1",
                    sku = ticket.TicketID.ToString()
                });
            }

            var transaction = new Transaction
            {
                amount = amount,
                description = string.Format("Order #{0} purchased from {1}", order.OrderID, SiteSettings.Resolve.GetSetting("SiteTitle")),
                item_list = itemList,
                invoice_number = order.OrderID.ToString()
            };



            var transactions = new List<Transaction>
            {
                transaction
            };
            return transactions;
        }

        private string GetTicketName(Ticket ticket)
        {
            var ticketLevelName = "";
            if (ticket.TicketSeat.TicketRowID.HasValue)
            {
                ticketLevelName = string.Format(" {0} {1} {2}",
                    ticket.TicketSeat.TicketRow.TicketSection.Name,
                    ticket.TicketSeat.TicketRow.Designation,
                    ticket.TicketSeat.SeatNumber);
            }

            return string.Format("{0}{1} ({2})",
                ticket.TicketSeat.TicketArea.Name,
                ticketLevelName,
                ticket.TicketPrice.EventAgeGroup.Name);
        }
    }

}
