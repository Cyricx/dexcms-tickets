using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DexCMS.Tickets.Events.Interfaces;
using DexCMS.Tickets.Events.Models;
using DexCMS.Tickets.Mvc.Models;

namespace DexCMS.Tickets.Mvc.Controllers
{
    public class EventsController : Controller
    {
        private IEventRepository eventRepository;
        private IEventSeriesRepository seriesRepository;

        public EventsController(IEventRepository eventRepo, IEventSeriesRepository seriesRepo)
        {
            eventRepository = eventRepo;
            seriesRepository = seriesRepo;
        }

        // GET: Events
        public ActionResult Index(string urlSegment)
        {
            Event evt = RetrieveEvent(urlSegment);
            if (evt == null)
            {
                return HttpNotFound();
            }
            ViewBag.ScheduleStatuses = eventRepository.GetScheduleStatuses().OrderBy(s => s.Name);
            ViewBag.ScheduleTypes = eventRepository.GetScheduleTypes().OrderBy(s => s.Name);
            ViewBag.VenueScheduleLocations = eventRepository.GetVenueScheduleLocations(evt.VenueID).OrderBy(s => s.Name);
            return View(evt);
        }

        public ActionResult Schedule(string category)
        {
            Event evt = RetrieveEvent(category);

            List<DisplayScheduleItem> evts = new List<DisplayScheduleItem>();

            foreach (var item in evt.ScheduleItems)
            {
                var className = string.Format("{0} {1} {2}",
                    item.ScheduleStatus.CssClass,
                    item.ScheduleType.CssClass,
                    item.VenueScheduleLocationID.HasValue ? item.VenueScheduleLocation.CssClass : "");
                evts.Add(new DisplayScheduleItem
                {
                    id = item.ScheduleItemID,
                    title = item.Title,
                    start = item.StartDate.ToString("MM/dd/yyyy hh:mm tt"),
                    end = item.EndDate.HasValue ? item.EndDate.Value.ToString("MM/dd/yyyy hh:mm tt") : "",
                    allDay = item.IsAllDay,
                    location = item.VenueScheduleLocationID.HasValue ? item.VenueScheduleLocation.Name : item.OtherLocation,
                    details = item.Details.Replace('\r', ' ').Replace('\n', ' ').Replace("\"", "\\\""),
                    status = item.ScheduleStatus.Name,
                    statusClass = item.ScheduleStatus.CssClass,
                    type = item.ScheduleType.Name,
                    className = className
                });
            }

            return View(evts);
        }


        public ActionResult Prices(string category)
        {
            Event evt = RetrieveEvent(category);

            return View(evt);
        }

        public async Task<ActionResult> Registration(string category)
        {
            Event evt = RetrieveEvent(category);

            evt.LastViewedRegistration = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Central Standard Time");
            await eventRepository.UpdateAsync(evt, evt.EventID);

            return View(evt);
        }

        public ActionResult FAQ(string category)
        {
            Event evt = RetrieveEvent(category);

            DisplayFAQ faqDisplay = new DisplayFAQ
            {
                faqCategories = new List<EventFaqCategory>(),
                faqItems = new List<EventFaqItem>()
            };

            foreach (var faqCat in evt.EventFaqCategories.Where(x => x.IsActive).OrderBy(x => x.DisplayOrder))
            {
                List<EventFaqItem> faqItems = faqCat.EventFaqItems.Where(x => x.IsActive).OrderBy(x => x.DisplayOrder).ToList();

                faqDisplay.faqCategories.Add(new EventFaqCategory
                {
                    EventFaqCategoryID = faqCat.EventFaqCategoryID,
                    Name = faqCat.Name,
                    EventFaqItems = faqItems
                });
                faqDisplay.faqItems.AddRange(faqItems);
            }

            return View(faqDisplay);
        }

        private Event RetrieveEvent(string category, bool setViewBag = true)
        {
            //check for series event
            var evt = seriesRepository.RetrievePublicSingle(category);

            if (evt == null)
            {
                //check for regular event
                evt = eventRepository.RetrieveByUrlSegment(category);
            }

            if (evt != null && setViewBag)
            {
                ViewBag.UrlSegment = category;
                ViewBag.EventStart = evt.EventStart;
                ViewBag.EventEnd = evt.EventEnd;
                ViewBag.PageContent = evt.PageContent;
            }

            return evt;
        }
    }



}