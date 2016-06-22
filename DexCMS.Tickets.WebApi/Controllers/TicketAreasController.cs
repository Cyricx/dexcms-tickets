using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using DexCMS.Tickets.Tickets.Interfaces;
using DexCMS.Tickets.WebApi.ApiModels;

namespace DexCMS.Tickets.WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TicketAreasController : ApiController
    {
        private ITicketAreaRepository repository;

        public TicketAreasController(ITicketAreaRepository repo)
        {
            repository = repo;
        }

        [ResponseType(typeof(List<TicketAreaApiModel>))]
        public IHttpActionResult GetTicketOptionDiscount(string bytype, int id)
        {
            List<TicketAreaApiModel> items = new List<TicketAreaApiModel>();

            if (bytype == "byevent")
            {
                items = repository.Items.Where(x => x.EventID == id).Select(x => new TicketAreaApiModel
                {
                    TicketAreaID = x.TicketAreaID,
                    Name = x.Name,
                    IsGA = x.IsGA,
                    EventID = x.EventID
                }).ToList();
            }
            else
            {
                return NotFound();
            }

            return Ok(items);
        }


    }

}