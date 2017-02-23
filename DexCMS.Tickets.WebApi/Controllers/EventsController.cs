using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DexCMS.Tickets.Events.Interfaces;
using DexCMS.Tickets.Events.Models;
using DexCMS.Tickets.WebApi.ApiModels;
using DexCMS.Base.WebApi.ApiModels;

namespace DexCMS.Tickets.WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EventsController : ApiController
    {
        private IEventRepository repository;

        public EventsController(IEventRepository repo)
        {
            repository = repo;
        }

        // GET api/Events
        public List<EventApiModel> GetEvents()
        {
            var items = repository.Items.Select(x => new EventApiModel
            {
                EventID = x.EventID,
                VenueID = x.VenueID,
                VenueName = x.Venue.Name,
                EventSeriesID = x.EventSeriesID,
                EventSeriesName = x.EventSeriesID.HasValue ? x.EventSeries.SeriesName : "",
                EventStart = x.EventStart,
                EventEnd = x.EventEnd,
                PageContentHeading = x.PageContent.Heading,
                PageContent = new PageContentApiModel
                {
                   PageContentID = x.PageContent.PageContentID,
                   Heading = x.PageContent.Heading,
                   Body = x.PageContent.Body,
                   PageTitle = x.PageContent.PageTitle,
                   MetaKeywords = x.PageContent.MetaKeywords,
                   MetaDescription = x.PageContent.MetaDescription,
                   ContentAreaID = x.PageContent.ContentAreaID,
                   ContentCategoryID = x.PageContent.ContentCategoryID,
                   ContentSubCategoryID = x.PageContent.ContentSubCategoryID,
                   PageTypeID = x.PageContent.PageTypeID,
                   ChangeFrequency = x.PageContent.ChangeFrequency,
                   LastModified = x.PageContent.LastModified,
                   LastModifiedBy = x.PageContent.LastModifiedBy,
                   Priority = x.PageContent.Priority,
                   AddToSitemap = x.PageContent.AddToSitemap,
                   LayoutTypeID = x.PageContent.LayoutTypeID,
                   UrlSegment = x.PageContent.UrlSegment
                },
                IsPublic = x.IsPublic,
                EventUrlSegment = x.EventUrlSegment
            }).ToList();

            return items;
        }

       // [ResponseType(typeof(List<EventApiModel>))]
        public async Task<IHttpActionResult> GetBy(string bytype, int id)
        {
            //var items = new List<EventApiModel>();

            if (bytype == "lookupname")
            {
                Event eventModel = await repository.RetrieveAsync(id);
                //items.Add(new EventApiModel { PageContentTitle = eventModel.PageContent.Title });
                return Ok(eventModel.PageContent.Heading);
            } else
            {
                return NotFound();
            }

            //return Ok(items);
        }

        // GET api/Events/5
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> GetEvent(int id)
        {
            Event eventModel = await repository.RetrieveAsync(id);
            if (eventModel == null)
            {
                return NotFound();
            }

            EventApiModel model = new EventApiModel()
            {
                EventID = eventModel.EventID,
                VenueID = eventModel.VenueID,
                VenueName = eventModel.Venue.Name,
                EventSeriesID = eventModel.EventSeriesID,
                EventSeriesName = eventModel.EventSeriesID.HasValue ? eventModel.EventSeries.SeriesName : "",
                EventStart = eventModel.EventStart,
                EventEnd = eventModel.EventEnd,
                PageContentHeading = eventModel.PageContent.Heading,
                LastViewedRegistration = eventModel.LastViewedRegistration,
                ForceDisableRegistration = eventModel.ForceDisableRegistration,
                RegistrationDisabledMessage = eventModel.RegistrationDisabledMessage,
                DisablePublicRegistration = eventModel.DisablePublicRegistration,
                PageContent = new EventContentInfo
                {
                   PageContentID = eventModel.PageContent.PageContentID,
                   Heading = eventModel.PageContent.Heading,
                   Body = eventModel.PageContent.Body,
                   PageTitle = eventModel.PageContent.PageTitle,
                   MetaKeywords = eventModel.PageContent.MetaKeywords,
                   MetaDescription = eventModel.PageContent.MetaDescription,
                   ContentAreaID = eventModel.PageContent.ContentAreaID,
                   ContentCategoryID = eventModel.PageContent.ContentCategoryID,
                   ContentSubCategoryID = eventModel.PageContent.ContentSubCategoryID,
                   PageTypeID = eventModel.PageContent.PageTypeID,
                   ChangeFrequency = eventModel.PageContent.ChangeFrequency,
                   LastModified = eventModel.PageContent.LastModified,
                   LastModifiedBy = eventModel.PageContent.LastModifiedBy,
                   Priority = eventModel.PageContent.Priority,
                   AddToSitemap = eventModel.PageContent.AddToSitemap,
                   ContentBlocks = eventModel.PageContent.ContentBlocks.OrderBy(x => x.DisplayOrder)
                    .Select(x => new EventContentBlockInfo
                    {
                        ContentBlockID = x.ContentBlockID,
                        BlockTitle = x.BlockTitle,
                        DisplayOrder = x.DisplayOrder
                    }).ToList(),
                   LayoutTypeID = eventModel.PageContent.LayoutTypeID,
                   UrlSegment = eventModel.PageContent.UrlSegment
                   // ,
                   //PageContentImages = eventModel.PageContent.PageContentImages.OrderBy(x => x.DisplayOrder)
                   // .Select(x => new EventImageInfo
                   // {
                   //     Alt = x.Image.Alt,
                   //     ImageID = x.ImageID,
                   //     DisplayOrder = x.DisplayOrder,
                   //     Thumbnail = x.Image.Thumbnail
                   // }).ToList()
                },
                IsPublic = eventModel.IsPublic,
                EventUrlSegment = eventModel.EventUrlSegment
            };

            return Ok(model);
        }

        // PUT api/Events/5
        public async Task<IHttpActionResult> PutEvent(int id, Event eventModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eventModel.EventID)
            {
                return BadRequest();
            }
            eventModel.PageContent.LastModified = DateTime.Now;
            eventModel.PageContent.LastModifiedBy = User.Identity.Name;

            await repository.UpdateAsync(eventModel, eventModel.EventID);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Events
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> PostEvent(Event eventModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            eventModel.PageContent.LastModified = DateTime.Now;
            eventModel.PageContent.LastModifiedBy = User.Identity.Name;
            

            await repository.AddAsync(eventModel);

            return CreatedAtRoute("DefaultApi", new { id = eventModel.EventID }, eventModel);
        }

        // DELETE api/Events/5
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> DeleteEvent(int id)
        {
            Event eventModel = await repository.RetrieveAsync(id);
            if (eventModel == null)
            {
                return NotFound();
            }

            await repository.DeleteAsync(eventModel);

            return Ok(eventModel);
        }

    }

  

}