using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using DexCMS.Tickets.Events.Interfaces;
using DexCMS.Tickets.WebApi.ApiModels;

namespace DexCMS.Tickets.WebApi.Controllers
{
    public class PublicEventsController : ApiController
    {
        private IEventRepository repository;

        public PublicEventsController(IEventRepository repo)
        {
            repository = repo;
        }

        [HttpGet]
        [ResponseType(typeof(List<PublicEventApiModel>))]
        public IHttpActionResult Get()
        {
            var events = repository.Items.Where(x => x.IsPublic && x.EventEnd > DateTime.Now)
                .OrderBy(x => x.PageContent.Heading)
                .Select(x => new PublicEventApiModel
                {
                    Heading = x.PageContent.Heading,
                    SeriesUrlSegment = x.EventSeriesID.HasValue ? x.EventSeries.SeriesUrlSegment : "",
                    EventUrlSegment = x.EventUrlSegment
                }).ToList();

            return Ok(events);
        }

    }
}
