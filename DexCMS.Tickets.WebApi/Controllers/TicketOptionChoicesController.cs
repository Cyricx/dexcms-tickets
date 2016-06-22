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
    public class TicketOptionChoicesController : ApiController
    {
        private ITicketOptionChoiceRepository repository;

        public TicketOptionChoicesController(ITicketOptionChoiceRepository repo)
        {
            repository = repo;
        }

        // GET api/TicketOptionChoices
        public List<TicketOptionChoiceApiModel> GetTicketOptionChoices()
        {
            var items = repository.Items.Select(x => new TicketOptionChoiceApiModel
            {
                TicketOptionChoiceID = x.TicketOptionChoiceID,
                Description = x.Description,
                MaximumAvailable = x.MaximumAvailable,
                Name = x.Name,
                MarkupPrice = x.MarkupPrice,
                TicketOptionID = x.TicketOptionID,
                TicketOptionName = x.TicketOption.Name,
                TicketCount = x.Tickets.Count
            }).ToList();

            return items;
        }

        // GET api/TicketOptionChoices/5
        [ResponseType(typeof(TicketOptionChoice))]
        public async Task<IHttpActionResult> GetTicketOptionChoice(int id)
        {
            TicketOptionChoice ticketOptionChoice = await repository.RetrieveAsync(id);
            if (ticketOptionChoice == null)
            {
                return NotFound();
            }

            TicketOptionChoiceApiModel model = new TicketOptionChoiceApiModel()
            {
                TicketOptionChoiceID = ticketOptionChoice.TicketOptionChoiceID,
                cbEventAges = ticketOptionChoice.EventAgeGroups.Select(x => x.EventAgeGroupID).ToList(),
                Description = ticketOptionChoice.Description,
                MarkupPrice = ticketOptionChoice.MarkupPrice,
                MaximumAvailable = ticketOptionChoice.MaximumAvailable,
                Name = ticketOptionChoice.Name,
                TicketOptionID = ticketOptionChoice.TicketOptionID,
                TicketOptionName = ticketOptionChoice.TicketOption.Name
            };

            return Ok(model);
        }

        [ResponseType(typeof(List<TicketOptionChoiceApiModel>))]
        public IHttpActionResult GetTicketOptionChoice(string bytype, int id)
        {
            List<TicketOptionChoiceApiModel> items = new List<TicketOptionChoiceApiModel>();

            if (bytype == "byoption")
            {
                items = repository.Items.Where(x => x.TicketOptionID == id).Select(x => new TicketOptionChoiceApiModel
                {
                    TicketOptionChoiceID = x.TicketOptionChoiceID,
                    Description = x.Description,
                    MaximumAvailable = x.MaximumAvailable,
                    Name = x.Name,
                    MarkupPrice = x.MarkupPrice,
                    TicketOptionID = x.TicketOptionID,
                    TicketOptionName = x.TicketOption.Name,
                    cbEventAges = x.EventAgeGroups.Select(y => y.EventAgeGroupID).ToList(),
                    AgeGroups = x.EventAgeGroups.Select(y => new TicketChoiceAgeApiModel
                    {
                        MaximumAge = y.MaximumAge,
                        MinimumAge = y.MinimumAge,
                        Name = y.Name
                    }).ToList(),
                    TicketCount = x.Tickets.Count
                }).ToList();
            }
            else
            {
                return NotFound();
            }

            return Ok(items);
        }

        // PUT api/TicketOptionChoices/5
        public async Task<IHttpActionResult> PutTicketOptionChoice(int id, TicketOptionChoice ticketOptionChoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ticketOptionChoice.TicketOptionChoiceID)
            {
                return BadRequest();
            }

            await repository.UpdateAsync(ticketOptionChoice, ticketOptionChoice.TicketOptionChoiceID);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/TicketOptionChoices
        [ResponseType(typeof(TicketOptionChoice))]
        public async Task<IHttpActionResult> PostTicketOptionChoice(TicketOptionChoice ticketOptionChoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await repository.AddAsync(ticketOptionChoice);

            return CreatedAtRoute("DefaultApi", new { id = ticketOptionChoice.TicketOptionChoiceID }, ticketOptionChoice);
        }

        // DELETE api/TicketOptionChoices/5
        [ResponseType(typeof(TicketOptionChoice))]
        public async Task<IHttpActionResult> DeleteTicketOptionChoice(int id)
        {
            TicketOptionChoice ticketOptionChoice = await repository.RetrieveAsync(id);
            if (ticketOptionChoice == null)
            {
                return NotFound();
            }

            await repository.DeleteAsync(ticketOptionChoice);

            return Ok(ticketOptionChoice);
        }

    }



}