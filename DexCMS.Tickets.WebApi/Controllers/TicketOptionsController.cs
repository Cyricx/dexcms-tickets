using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DexCMS.Tickets.Tickets.Interfaces;
using DexCMS.Tickets.Tickets.Models;
using DexCMS.Tickets.WebApi.ApiModels;

namespace DexCMS.Tickets.WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TicketOptionsController : ApiController
    {
        private ITicketOptionRepository repository;

        public TicketOptionsController(ITicketOptionRepository repo)
        {
            repository = repo;
        }

        // GET api/TicketOptions
        public List<TicketOptionApiModel> GetTicketOptions()
        {
            var items = repository.Items.Select(x => new TicketOptionApiModel
            {
                TicketOptionID = x.TicketOptionID,
                CutoffDate = x.CutoffDate,
                Description = x.Description,
                EventID = x.EventID,
                BasePrice = x.BasePrice,
                Name = x.Name,
                IsRequired = x.IsRequired,
                TicketOptionChoiceCount = x.TicketOptionChoices.Count
            }).ToList();

            return items;
        }

        // GET api/TicketOptions/5
        [ResponseType(typeof(TicketOption))]
        public async Task<IHttpActionResult> GetTicketOption(int id)
        {
            TicketOption ticketOption = await repository.RetrieveAsync(id);
            if (ticketOption == null)
            {
                return NotFound();
            }

            TicketOptionApiModel model = new TicketOptionApiModel()
            {
                TicketOptionID = ticketOption.TicketOptionID,
                CutoffDate = ticketOption.CutoffDate,
                Description = ticketOption.Description,
                EventID = ticketOption.EventID,
                BasePrice = ticketOption.BasePrice,
                Name = ticketOption.Name,
                IsRequired = ticketOption.IsRequired,
                TicketOptionChoiceCount = ticketOption.TicketOptionChoices.Count
            };

            return Ok(model);
        }

        [ResponseType(typeof(List<TicketOptionApiModel>))]
        public IHttpActionResult GetTicketOption(string bytype, int id)
        {
            List<TicketOptionApiModel> items = new List<TicketOptionApiModel>();

            if (bytype == "byevent")
            {
                items = repository.Items.Where(x => x.EventID == id).Select(x => new TicketOptionApiModel
                {
                    TicketOptionID = x.TicketOptionID,
                    CutoffDate = x.CutoffDate,
                    Description = x.Description,
                    EventID = x.EventID,
                    BasePrice = x.BasePrice,
                    Name = x.Name,
                    IsRequired = x.IsRequired,
                    TicketOptionChoiceCount = x.TicketOptionChoices.Count
                }).ToList();
            }
            else
            {
                return NotFound();
            }

            return Ok(items);
        }


        // PUT api/TicketOptions/5
        public async Task<IHttpActionResult> PutTicketOption(int id, TicketOption ticketOption)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ticketOption.TicketOptionID)
            {
                return BadRequest();
            }

            await repository.UpdateAsync(ticketOption, ticketOption.TicketOptionID);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/TicketOptions
        [ResponseType(typeof(TicketOption))]
        public async Task<IHttpActionResult> PostTicketOption(TicketOption ticketOption)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await repository.AddAsync(ticketOption);

            return CreatedAtRoute("DefaultApi", new { id = ticketOption.TicketOptionID }, ticketOption);
        }

        // DELETE api/TicketOptions/5
        [ResponseType(typeof(TicketOption))]
        public async Task<IHttpActionResult> DeleteTicketOption(int id)
        {
            TicketOption ticketOption = await repository.RetrieveAsync(id);
            if (ticketOption == null)
            {
                return NotFound();
            }

            await repository.DeleteAsync(ticketOption);

            return Ok(ticketOption);
        }

    }



}