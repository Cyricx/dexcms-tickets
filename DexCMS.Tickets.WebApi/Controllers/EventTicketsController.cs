using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DexCMS.Tickets.Events.Interfaces;
using DexCMS.Tickets.Tickets.Interfaces;
using DexCMS.Tickets.Tickets.Models;
using DexCMS.Tickets.WebApi.ApiModels;

namespace DexCMS.Tickets.WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EventTicketsController : ApiController
    {
        private IEventRepository eventRepository;
        private ITicketAreaRepository areaRepository;

        public EventTicketsController(IEventRepository eventRepo, ITicketAreaRepository areaRepo)
        {
            eventRepository = eventRepo;
            areaRepository = areaRepo;
        }

        [ResponseType(typeof(EventTicketsApiModel))]
        public async Task<IHttpActionResult> GetEventTickets(int id)
        {
            var eventModel = await eventRepository.RetrieveAsync(id);

            if (eventModel == null)
            {
                return NotFound();
            }

            EventTicketsApiModel model = new EventTicketsApiModel()
            {
                EventID = eventModel.EventID,
                IsSetup = eventModel.TicketAreas.Count > 0,
                VenueID = eventModel.VenueID,
                TicketAreas = eventModel.TicketAreas.Select(a => new TicketAreaApiModel
                {
                    TicketAreaID = a.TicketAreaID,
                    IsGA = a.IsGA,
                    Name = a.Name,
                    DisplayOrder = a.DisplayOrder,
                    MaxCapacity = a.TicketSeats.Count,
                    NewMaxCapacity = a.TicketSeats.Count,
                    Assigned = a.TicketSeats.Count(ts => ts.TicketSeatStatus == TicketSeatStatus.Assigned || ts.TicketSeatStatus == TicketSeatStatus.Complete),
                    Unavailable = a.TicketSeats.Count(ts => ts.TicketSeatStatus == TicketSeatStatus.Disabled),
                    NewUnavailable = a.TicketSeats.Count(ts => ts.TicketSeatStatus ==  TicketSeatStatus.Disabled),
                    UnclaimedReservations = a.TicketSeats.Count(ts => ts.TicketSeatStatus == TicketSeatStatus.Reserved),
                    TicketSections = a.TicketSections.Select(s => new TicketSectionApiModel
                    {
                        TicketSectionID = s.TicketSectionID,
                        Name = s.Name,
                        TicketRows = s.TicketRows.Select(r => new TicketRowApiModel
                        {
                            TicketRowID = r.TicketRowID,
                            Designation = r.Designation,
                            MaxCapacity = r.TicketSeats.Count,
                            NewMaxCapacity = r.TicketSeats.Count,
                            Assigned = r.TicketSeats.Count(ts => ts.TicketSeatStatus == TicketSeatStatus.Assigned ||ts.TicketSeatStatus == TicketSeatStatus.Complete),
                            Unavailable = r.TicketSeats.Count(ts => ts.TicketSeatStatus == TicketSeatStatus.Disabled),
                            NewUnavailable = r.TicketSeats.Count(ts => ts.TicketSeatStatus == TicketSeatStatus.Disabled),
                            UnclaimedReservations = r.TicketSeats.Count(ts => ts.TicketSeatStatus == TicketSeatStatus.Reserved)
                        }).ToList()
                    }).ToList()
                }).ToList()
            };

            return Ok(model);
        }

        public async Task<IHttpActionResult> PutEventTickets(int id, EventTicketsApiModel item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.EventID || item.TicketAreas == null)
            {
                return BadRequest();
            }

            //build ticket Areas
            foreach (var apiArea in item.TicketAreas)
            {
                //see if this is an add or retrieve
                if (apiArea.TicketAreaID.HasValue)
                {
                    //Updating
                    TicketArea area = await areaRepository.RetrieveAsync(apiArea.TicketAreaID);
                    area.Name = apiArea.Name;
                    area.DisplayOrder = apiArea.DisplayOrder;

                    if (!area.IsGA)
                    {
                        foreach (var apiSection in apiArea.TicketSections)
                        {
                            if (apiSection.TicketSectionID.HasValue)
                            {
                                TicketSection section = area.TicketSections.Where(x => x.TicketSectionID == apiSection.TicketSectionID.Value).Single();

                                foreach (var apiRow in apiSection.TicketRows)
                                {
                                    if (apiRow.TicketRowID.HasValue)
                                    {
                                        //updating
                                        TicketRow row = section.TicketRows.Where(x => x.TicketRowID == apiRow.TicketRowID.Value).Single();
                                        //deal with new seats
                                        if (apiRow.NewMaxCapacity > apiRow.MaxCapacity)
                                        {
                                            List<TicketSeat> addSeats = BuildTickets(apiRow, area.TicketAreaID, apiRow.MaxCapacity + 1);
                                            addSeats.ForEach(x => row.TicketSeats.Add(x));
                                        }

                                        //deal with new unavailability
                                        SetUnvailableTickets(row.TicketSeats.ToList(), apiRow.NewUnavailable);
                                    }
                                    else
                                    {
                                        BuildNewRow(area, section, apiRow);
                                    }
                                }
                            }
                            else
                            {
                                BuildNewSection(area, apiSection);
                            }
                        }
                    }
                    else
                    {
                        //deal with new seats
                        if (apiArea.NewMaxCapacity > apiArea.MaxCapacity)
                        {
                            List<TicketSeat> addSeats = BuildTickets(apiArea, area.TicketAreaID, apiArea.MaxCapacity + 1);

                            addSeats.ForEach(x => area.TicketSeats.Add(x));
                        }
                        //deal with adjusted unavailability
                        SetUnvailableTickets(area.TicketSeats.ToList(), apiArea.NewUnavailable);
                    }
                    await areaRepository.UpdateAsync(area, area.TicketAreaID);
                }
                else
                {
                    //inserting
                    TicketArea area = new TicketArea()
                    {
                        Name = apiArea.Name,
                        IsGA = apiArea.IsGA,
                        EventID = item.EventID
                    };

                    //ADD FIRST to create ID
                    await areaRepository.AddAsync(area);

                    if (!area.IsGA)
                    {
                        area.TicketSections = new List<TicketSection>();
                        //loop and build sections
                        foreach (var apiSection in apiArea.TicketSections)
                        {
                            BuildNewSection(area, apiSection);
                        }

                    }
                    else
                    {
                        area.TicketSeats = new List<TicketSeat>();

                        List<TicketSeat> addSeats = BuildTickets(apiArea, area.TicketAreaID, 1);
                        SetUnvailableTickets(addSeats, apiArea.NewUnavailable);
                        addSeats.ForEach(x => area.TicketSeats.Add(x));

                    }

                    await areaRepository.UpdateAsync(area, area.TicketAreaID);


                }

            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        private void BuildNewSection(TicketArea area, TicketSectionApiModel apiSection)
        {
            TicketSection section = new TicketSection()
            {
                Name = apiSection.Name,
                TicketRows = new List<TicketRow>()
            };

            //loop and build rows
            foreach (var apiRow in apiSection.TicketRows)
            {
                BuildNewRow(area, section, apiRow);
            }

            area.TicketSections.Add(section);
        }

        private void BuildNewRow(TicketArea area, TicketSection section, TicketRowApiModel apiRow)
        {
            TicketRow row = new TicketRow()
            {
                Designation = apiRow.Designation,
                TicketSeats = new List<TicketSeat>()
            };

            List<TicketSeat> addSeats = BuildTickets(apiRow, area.TicketAreaID, 1);
            SetUnvailableTickets(addSeats, apiRow.NewUnavailable);

            addSeats.ForEach(x => row.TicketSeats.Add(x));

            section.TicketRows.Add(row);
        }

        private static List<TicketSeat> BuildTickets(SeatableApiModel location, int areaID, int startNumber)
        {
            //Create tickets!

            List<TicketSeat> addSeats = new List<TicketSeat>();
            for (int i = startNumber; i <= location.NewMaxCapacity; i++)
            {

                TicketSeat seat = new TicketSeat()
                {
                    SeatNumber = i,
                    TicketAreaID = areaID,
                    //IsDisabled = false,
                    //IsAssigned = false
                    TicketSeatStatus = TicketSeatStatus.Available
                };
                addSeats.Add(seat);
            }

            return addSeats;
        }

        private void SetUnvailableTickets(List<TicketSeat> seats, int? newUnavailable)
        {
            if (newUnavailable.HasValue)
            {

                var currentUnavailable = seats.Where(x => x.TicketSeatStatus == TicketSeatStatus.Disabled);
                var currentUnassigned = seats.Where(x => x.TicketSeatStatus == TicketSeatStatus.Available);
                int unavailableCount = currentUnavailable.Count();
                if (unavailableCount < newUnavailable.Value)
                {
                    //disable some
                    int correctionAmount = newUnavailable.Value - unavailableCount;
                    if (correctionAmount <= currentUnassigned.Count())
                    {
                        var correctionSeats = currentUnassigned.Take(correctionAmount).ToList();
                        correctionSeats.ForEach(x => x.TicketSeatStatus = TicketSeatStatus.Disabled);
                    }
                }
                else if (unavailableCount > newUnavailable.Value)
                {
                    //enable some!
                    int correctionAmount = unavailableCount - newUnavailable.Value;
                    var correctionSeats = currentUnavailable.Take(correctionAmount).ToList();
                    correctionSeats.ForEach(x => x.TicketSeatStatus = TicketSeatStatus.Available);

                }
            }
        }
    }
}