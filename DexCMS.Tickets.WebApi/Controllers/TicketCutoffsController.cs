using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DexCMS.Tickets.Events.Interfaces;
using DexCMS.Tickets.Events.Models;
using DexCMS.Tickets.Tickets.Interfaces;
using DexCMS.Tickets.Tickets.Models;
using DexCMS.Tickets.WebApi.ApiModels;

namespace DexCMS.Tickets.WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TicketCutoffsController : ApiController
    {
        private ITicketCutoffRepository repository;
        private IEventAgeGroupRepository agesRepository;
        private ITicketAreaRepository areasRepository;

        public TicketCutoffsController(
            ITicketCutoffRepository repo, 
            IEventAgeGroupRepository agesRepo,
            ITicketAreaRepository areasRepo)
        {
            repository = repo;
            agesRepository = agesRepo;
            areasRepository = areasRepo;
        }

        // GET api/EventAgeGroups
        public List<TicketCutoffApiModel> GetTicketCutoffs()
        {
            var items = repository.Items.Select(x => new TicketCutoffApiModel
            {
                TicketCutoffID = x.TicketCutoffID,
                EventID = x.EventID,
                Name = x.Name,
                OnSellDate = x.OnSellDate,
                CutoffDate = x.CutoffDate
            }).ToList();

            return items;
        }

        // GET api/EventAgeGroups/1
        [ResponseType(typeof(TicketCutoffApiModel))]
        public async Task<IHttpActionResult> GetTicketCutoffs(int id)
        {
            TicketCutoff ticketCutoff = await repository.RetrieveAsync(id);
            if (ticketCutoff == null)
            {
                return NotFound();
            }

            TicketCutoffApiModel model = new TicketCutoffApiModel
            {
                TicketCutoffID = ticketCutoff.TicketCutoffID,
                EventID = ticketCutoff.EventID,
                Name = ticketCutoff.Name,
                OnSellDate = ticketCutoff.OnSellDate,
                CutoffDate = ticketCutoff.CutoffDate
            };

            return Ok(model);
        }

        [ResponseType(typeof(List<TicketCutoffApiModel>))]
        public IHttpActionResult GetTicketCutoffs(string bytype, int id)
        {
            var items = new List<TicketCutoffApiModel>();

            if (bytype == "byevent")
            {
                items = BuildCutoffsWithPrices(id);
            }
            else
            {
                return NotFound();
            }

            return Ok(items);
        }

        // PUT api/TicketCutoffs/5
        public async Task<IHttpActionResult> PutTicketCutoff(int id, TicketCutoff TicketCutoff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != TicketCutoff.TicketCutoffID)
            {
                return BadRequest();
            }

            await repository.UpdateAsync(TicketCutoff, TicketCutoff.TicketCutoffID);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/TicketCutoffs
        [ResponseType(typeof(TicketCutoff))]
        public async Task<IHttpActionResult> PostTicketCutoff(TicketCutoff TicketCutoff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await repository.AddAsync(TicketCutoff);

            return CreatedAtRoute("DefaultApi", new { id = TicketCutoff.TicketCutoffID }, TicketCutoff);
        }

        // DELETE api/TicketCutoffs/5
        [ResponseType(typeof(TicketCutoff))]
        public async Task<IHttpActionResult> DeleteTicketCutoff(int id)
        {
            TicketCutoff TicketCutoff = await repository.RetrieveAsync(id);
            if (TicketCutoff == null)
            {
                return NotFound();
            }

            await repository.DeleteAsync(TicketCutoff);

            return Ok(TicketCutoff);
        }


        private List<TicketCutoffApiModel> BuildCutoffsWithPrices(int eventID)
        {
            List<TicketCutoffApiModel> items = new List<TicketCutoffApiModel>();
            
            //get sub info
            List<TicketCutoff> cutoffs = repository.Items.Where(x => x.EventID == eventID).OrderBy(x => x.OnSellDate).ToList(); ;
            List<EventAgeGroup> ages = agesRepository.Items.Where(x => x.EventID == eventID).OrderBy(x => x.MinimumAge).ToList();
            List<TicketArea> areas = areasRepository.Items.Where(x => x.EventID == eventID).OrderBy(x => x.TicketAreaID).ToList();

            foreach (TicketCutoff cutoff in cutoffs)
            {
                TicketCutoffApiModel item = new TicketCutoffApiModel
                {
                    TicketCutoffID = cutoff.TicketCutoffID,
                    EventID = cutoff.EventID,
                    Name = cutoff.Name,
                    OnSellDate = cutoff.OnSellDate,
                    CutoffDate = cutoff.CutoffDate,
                    TicketAreas = new List<TicketCutoffAreaApiModel>(),
                    TicketPricesCount = cutoff.TicketPrices.Count
                };

                //loop areas
                foreach (TicketArea area in areas)
                {
                    TicketCutoffAreaApiModel itemArea = new TicketCutoffAreaApiModel()
                    {
                        TicketAreaID = area.TicketAreaID,
                        Name = area.Name,
                        TicketPrices = new List<TicketCutoffPriceModel>()
                    };

                    //loops ages to create price model
                    foreach (EventAgeGroup age in ages)
                    {

                        TicketCutoffPriceModel itemPrice = new TicketCutoffPriceModel {
                            EventAgeGroupID = age.EventAgeGroupID,
                            Name = age.Name,
                            MinimumAge = age.MinimumAge,
                            MaximumAge = age.MaximumAge,
                            TicketCutoffID = cutoff.TicketCutoffID,
                            TicketAreaID = area.TicketAreaID,
                            BasePrice = null,
                            TicketPriceID = null
                        };

                        TicketPrice price = area.TicketPrices
                            .Where(x => x.EventAgeGroupID == age.EventAgeGroupID
                            && x.TicketCutoffID == cutoff.TicketCutoffID).SingleOrDefault();
                        if (price != null)
                        {
                            itemPrice.BasePrice = price.BasePrice;
                            itemPrice.TicketPriceID = price.TicketPriceID;
                        }

                        itemArea.TicketPrices.Add(itemPrice);
                    }

                    item.TicketAreas.Add(itemArea);
                }

                items.Add(item);
            }

            return items;
        }


    }//end controller



}