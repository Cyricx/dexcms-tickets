using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DexCMS.Tickets.Orders.Interfaces;
using DexCMS.Tickets.Orders.Models;
using DexCMS.Tickets.WebApi.ApiModels;
using DexCMS.Core;

namespace DexCMS.Tickets.WebApi.Controllers
{
    [Authorize(Roles="Admin")]
    public class OrdersController : ApiController
    {
        private IOrderRepository repository;
        private DateTime cstTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Central Standard Time");

        public OrdersController(IOrderRepository repo)
        {
            repository = repo;
        }

        public List<OrderApiModel> GetOrders()
        {
            var items = repository.Items.Select(x => new OrderApiModel
            {
                OrderID = x.OrderID,
                UserName = x.UserName,
                OrderStatus = (int)x.OrderStatus,
                OrderStatusName = x.OrderStatus.ToString(),
                EnteredOn = x.EnteredOn,
                OrderTotal = x.OrderTotal,
                RefundAmount = x.RefundAmount,
                RefundedOn = x.RefundedOn,
                OverridedBy = x.OverridedBy,
                OverridedOn = x.OverridedOn,
                OverideReason = x.OverrideReason,
                TicketCount = x.Tickets.Count
            }).ToList();

            return items;
        }

        [ResponseType(typeof(OrderApiModel))]
        public async Task<IHttpActionResult> GetOrder(int id)
        {
            Order x = await repository.RetrieveAsync(id);
            if (x == null)
            {
                return NotFound();
            }

            OrderApiModel model = new OrderApiModel()
            {
                OrderID = x.OrderID,
                UserName = x.UserName,
                OrderStatus = (int)x.OrderStatus,
                OrderStatusName = x.OrderStatus.ToString(),
                EnteredOn = x.EnteredOn,
                OrderTotal = x.OrderTotal,
                RefundAmount = x.RefundAmount,
                RefundedOn = x.RefundedOn,
                OverridedBy = x.OverridedBy,
                OverridedOn = x.OverridedOn,
                OverideReason = x.OverrideReason,
                Tickets = x.Tickets.Select(y => new TicketApiModel
                {
                    TicketID = y.TicketID,
                    TicketSeatID = y.TicketSeat.TicketSeatID,
                    FirstName = y.FirstName,
                    MiddleInitial = y.MiddleInitial,
                    LastName = y.LastName,
                    Location = TicketApiModel.BuildLocation(y.TicketSeat),
                    AgeGroup = y.TicketPrice.EventAgeGroup.Name,
                    DiscountName = y.TicketDiscountID.HasValue ? y.TicketDiscount.Name : null,
                    EventName = y.TicketSeat.TicketArea.Event.PageContent.Heading,
                    TicketOptionChoices = y.TicketOptionChoices.Select(z => new TicketOptionChoiceApiModel
                    {
                        TicketOptionChoiceID = z.TicketOptionChoiceID,
                        TicketOptionID = z.TicketOptionID,
                        TicketOptionName = z.TicketOption.Name,
                        Name = z.Name
                    }).ToList()
                }).ToList(),
                Payments = x.Payments.Select(y => new PaymentApiModel
                {
                    PaymentID = y.PaymentID,
                    OrderID = y.OrderID,
                    PaidOn = y.PaidOn,
                    PaymentType = (int)y.PaymentType,
                    PaymentTypeName = y.PaymentType.ToString(),  
                    GrossPaid = y.GrossPaid,
                    PaymentFee = y.PaymentFee,
                    NetPaid = y.NetPaid
                }).ToList()
            };

            return Ok(model);
        }

        public async Task<IHttpActionResult> Delete()
        {
            int registrationExpirationMinutes = int.Parse(SiteSettings.Resolve.GetSetting("RegistrationExpirationMinutes"));

            var orders = repository.Items.Where(x => x.OrderStatus == OrderStatus.Pending).ToList();
                
            foreach (var item in orders)
            {
                if (item.EnteredOn.AddMinutes(registrationExpirationMinutes) < cstTime)
                {
                    await repository.DeleteAsync(item);
                }
            }

            return Ok();
        }
    }
}
