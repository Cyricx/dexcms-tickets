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
using DexCMS.Core.Extensions;
using Newtonsoft.Json;
using DexCMS.Core;
using DexCMS.Core.Enums;

namespace DexCMS.Tickets.WebApi.Controllers
{
    [Authorize(Roles ="Cashier")]
    public class CashierOrdersController : ApiController
    {
        private IOrderRepository repository;
        private ITicketSeatRepository seatRepository;
        private ITicketPriceRepository priceRepository;
        private ITicketDiscountRepository discountRepository;
        private ITicketOptionChoiceRepository choiceRepository;
        private DateTime cstTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Central Standard Time");

        public CashierOrdersController(
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
        [ResponseType(typeof(CashierOrderApiModel))]
        public async Task<IHttpActionResult> Get(int id)
        {
            var order = await repository.RetrieveAsync(id);
            Orders.Models.Payment payment = null;

            if (order.Payments != null && order.Payments.Count > 0)
            {
                payment = order.Payments.First();
            }
            InvoiceDetails details = null;
            var response = new CashierOrderApiModel
            {
                OrderID = order.OrderID,
                EnteredOn = order.EnteredOn,
                OrderStatus = order.OrderStatus,
                OrderTotal = order.OrderTotal,
                Notes = order.Notes
            };

            if (payment != null)
            {
                if (payment.PaymentType == PaymentType.PendingInvoice)
                {
                    details = JsonConvert.DeserializeObject<InvoiceDetails>(payment.PaymentDetails);

                    var invoice = Invoice.Get(PaypalConfiguration.GetAPIContext(), details.id);
                    if (invoice.status == "PAID")
                    {
                        payment.GrossPaid = decimal.Parse(invoice.total_amount.value);
                        payment.PaidOn = Convert.ToDateTime(invoice.invoice_date.Substring(0, 10));
                        payment.PaymentType = PaymentType.Paypal;
                        order.OrderStatus = OrderStatus.Complete;
                        await repository.UpdateAsync(order, order.OrderID);
                    }
                }
                else
                {
                    response.PaymentDetails = payment.PaymentDetails;
                    response.PaymentID = payment.PaymentID;
                    response.PaymentType = payment.PaymentType;
                    response.GrossPaid = payment.GrossPaid;
                }
            } 
            else
            {
                response.PaymentType = PaymentType.Pending;
            }


            if (details != null)
            {
                response.InvoiceEmail = details.email;
                response.InvoiceID = details.id;
            }

            return Ok(response);
        }

        [HttpPost]
        [ResponseType(typeof(OrderCreateResponseModel))]
        public async Task<IHttpActionResult> Post(CashierOrderApiModel model)
        {
            if (model == null || model.Tickets.Length == 0)
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

            foreach (var item in model.Tickets)
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


            //! CHANGING HERE
            switch (model.CashierOrderType)
            {
                case CashierOrderType.CashOrCheck:
                    return Ok(new { OrderID = order.OrderID });

                case CashierOrderType.Paypal:
                     Invoice invoice = CreateInvoice(model.InvoiceEmail, order);
                    invoice.id = order.OrderID.ToString();

                    try
                    {
                        var apiContext = PaypalConfiguration.GetAPIContext();
                        var createdInvoice = invoice.Create(apiContext);
                        createdInvoice.Send(apiContext, true);

                        var invoiceDetails = new
                        {
                            id = createdInvoice.id,
                            email = model.InvoiceEmail
                        };

                        order.Payments = new List<Orders.Models.Payment>
                        {
                            new Orders.Models.Payment
                            {
                                OrderID = order.OrderID,
                                PaymentDetails = JsonConvert.SerializeObject(invoiceDetails),
                                PaymentType = PaymentType.PendingInvoice
                            }
                        };
                        order.OrderStatus = OrderStatus.Pending;

                        await repository.UpdateAsync(order, order.OrderID);
                        return Ok(new { OrderID = order.OrderID });
                    }
                    catch (Exception ex)
                    {
                        await Logger.WriteLog(LogType.Error, ex.ToString());
                        throw ex;
                    }
  
                default:
                    return Ok();
            }          
        }

        private Invoice CreateInvoice(string email, Orders.Models.Order order)
        {
            return new Invoice
            {

                merchant_info = new MerchantInfo
                {
                    email = WebConfigurationManager.AppSettings["PaypalEmail"]
                },
                billing_info = new List<BillingInfo>
                        {
                            new BillingInfo {email = email }
                        },
                items = BuildItems(order),
                payment_term = new PaymentTerm
                {
                    due_date = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + " " + TimeZoneInfo.Local.TimeZoneAbbreviation()
                }
            };
        }

        

        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, CashierOrderApiModel model)
        {
            var order = await repository.RetrieveAsync(id);

            if (order == null || order.OrderID != model.OrderID)
            {
                return NotFound();
            }

            if (model.GrossPaid.HasValue)
            {
                Orders.Models.Payment payment = null;

                if (model.PaymentID.HasValue)
                {
                    payment = order.Payments.Where(x => x.PaymentID == model.PaymentID).SingleOrDefault();
                    payment.GrossPaid = model.GrossPaid.Value;
                    payment.PaymentType = model.PaymentType;
                    payment.PaymentDetails = model.PaymentDetails;
                }

                if (payment == null)
                {
                    payment = new Orders.Models.Payment
                    {
                        GrossPaid = model.GrossPaid.Value,
                        PaidOn = cstTime,
                        PaymentType = model.PaymentType,
                        PaymentDetails = model.PaymentDetails,
                        OrderID = model.OrderID
                    };
                }

                if (order.Payments == null || order.Payments.Count == 0)
                {
                    order.Payments = new List<Orders.Models.Payment>
                    {
                        payment
                    };
                }

            }
            order.OrderStatus = model.OrderStatus;
            order.Notes = model.Notes;

            await repository.UpdateAsync(order, order.OrderID);

            return Ok();
    }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id, string email)
        {
            var order = await repository.RetrieveAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            var payment = order.Payments.First();
            if (payment.PaymentType != PaymentType.PendingInvoice)
            {
                return NotFound();
            }

            //retrieve previous invoice
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            InvoiceDetails details = JsonConvert.DeserializeObject<InvoiceDetails>(payment.PaymentDetails);
            try
            {
                //check if old invoice is still valid
                var invoice = Invoice.Get(apiContext, details.id);

                //cancel invoice
                var cancelNotification = new CancelNotification
                {
                    subject = "Payment Canceled",
                    note = "The payment has been canceled.",
                    send_to_merchant = true,
                    send_to_payer = true
                };
                invoice.Cancel(apiContext, cancelNotification);
            }
            catch(Exception e)
            {
                //not valid, keep on trucking!
            }

            //create newinvoice
            Invoice newInvoice = CreateInvoice(email, order);

            newInvoice.id = order.OrderID.ToString();

            try
            {
                var createdInvoice = newInvoice.Create(apiContext);
                createdInvoice.Send(apiContext, true);

                var invoiceDetails = new
                {
                    id = createdInvoice.id,
                    email = email
                };

                payment.PaymentDetails = JsonConvert.SerializeObject(invoiceDetails);

                await repository.UpdateAsync(order, order.OrderID);
                return Ok(new { OrderID = order.OrderID });
            }
            catch (Exception ex)
            {
                await Logger.WriteLog(LogType.Error, ex.ToString());
                throw ex;
            }
        }

        //[HttpDelete]
        //public async Task<IHttpActionResult> Delete(int id)
        //{
        //    var order = await repository.RetrieveAsync(id);

        //    await repository.DeleteAsync(order);
        //    return Ok();
        //}

        //[HttpPut]
        //public async Task<IHttpActionResult> Put(int id, SecureOrderApiModel model)
        //{
        //    var order = await repository.RetrieveAsync(id);

        //    if (order != null && order.UserName == User.Identity.Name)
        //    {
        //        var apiContext = PaypalConfiguration.GetAPIContext();
        //        var payment = PayPal.Api.Payment.Get(apiContext, model.PaymentID);
        //        if (payment.state == "approved")
        //        {
        //            var transaction = payment.transactions.ToArray()[0];
        //            var sale = transaction.related_resources.ToArray()[0].sale;

        //            if (order.Payments == null)
        //            {
        //                order.Payments = new List<DexCMS.Tickets.Orders.Models.Payment>();
        //            }

        //            order.Payments.Add(new DexCMS.Tickets.Orders.Models.Payment
        //            {
        //                GrossPaid = decimal.Parse(transaction.amount.total),
        //                NetPaid = decimal.Parse(transaction.amount.total) - decimal.Parse(sale.transaction_fee.value),
        //                OrderID = order.OrderID,
        //                PaidOn = Convert.ToDateTime(payment.update_time),
        //                PaymentDetails = payment.ConvertToJson(),
        //                PaymentFee = decimal.Parse(sale.transaction_fee.value),
        //                PaymentType = PaymentType.Paypal
        //            });

        //            order.OrderStatus =
        //                order.OrderTotal == decimal.Parse(transaction.amount.total) ? OrderStatus.Complete : OrderStatus.Partial;

        //            await repository.UpdateAsync(order, order.OrderID);

        //            return Ok();
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }

        //}

        //private PayPal.Api.Payment CreatePayment(DexCMS.Tickets.Orders.Models.Order order)
        //{
        //    var payer = new Payer { payment_method = "paypal" };
        //    List<Transaction> transactions = BuildTransactions(order);
        //    var baseUrl = WebConfigurationManager.AppSettings["ServerUrl"] + "secure/";
        //    var payment = new PayPal.Api.Payment
        //    {
        //        intent = "sale",
        //        payer = payer,
        //        transactions = transactions,
        //        redirect_urls = new RedirectUrls
        //        {
        //            cancel_url = baseUrl + "cancel/" + order.OrderID,
        //            return_url = baseUrl + "payment"
        //        }
        //    };

        //    return payment;
        //}

        private List<InvoiceItem> BuildItems(DexCMS.Tickets.Orders.Models.Order order)
        {
            var invoiceItems = new List<InvoiceItem>();

            foreach (var ticket in order.Tickets)
            {
                var item = new InvoiceItem
                {
                   name = GetTicketName(ticket),
                   quantity = 1,
                   unit_price = new Currency
                   {
                       currency = "USD",
                       value = ticket.TicketTotalPrice.ToString()
                   }
                };
                invoiceItems.Add(item);
            }

            return invoiceItems;
        }

        //private List<Transaction> BuildTransactions(DexCMS.Tickets.Orders.Models.Order order)
        //{
        //    var details = new Details { shipping = "0.00", tax = "0.00", subtotal = order.OrderTotal.ToString() };
        //    var amount = new Amount { currency = "USD", details = details, total = order.OrderTotal.ToString() };
        //    var itemList = new ItemList() { items = new List<Item>() };

        //    foreach (var ticket in order.Tickets)
        //    {
        //        itemList.items.Add(new Item
        //        {
        //            name = GetTicketName(ticket),
        //            currency = "USD",
        //            price = ticket.TicketTotalPrice.ToString(),
        //            quantity = "1",
        //            sku = ticket.TicketID.ToString()
        //        });
        //    }

        //    var transaction = new Transaction
        //    {
        //        amount = amount,
        //        description = string.Format("Order #{0} purchased from {1}", order.OrderID, SiteSettings.Resolve.GetSetting("SiteTitle")),
        //        item_list = itemList,
        //        invoice_number = order.OrderID.ToString()
        //    };



        //    var transactions = new List<Transaction>
        //    {
        //        transaction
        //    };
        //    return transactions;
        //}

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
