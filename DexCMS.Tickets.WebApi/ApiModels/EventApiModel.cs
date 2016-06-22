using System;
using System.Collections.Generic;
using DexCMS.Base.Enums;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class EventApiModel
    {
        public int EventID { get; set; }
        public int VenueID { get; set; }
        public string VenueName { get; set; }
        public int? EventSeriesID { get; set; }
        public string EventSeriesName { get; set; }
        public DateTime EventStart { get; set; }
        public DateTime EventEnd { get; set; }
        public EventContentInfo PageContent { get; set; }
        public string PageContentHeading { get; set; }
        public bool IsPublic { get; set; }
        public string EventUrlSegment { get; set; }
        public int? AvailableCount { get; set; }
        public int? AssignedCount { get; set; }
        public int? DisabledCount { get; set; }
        public int? ReservedCount { get; set; }
        public int? CompleteCount { get; set; }
        public bool ForceDisableRegistration { get; set; }
        public DateTime? LastViewedRegistration { get; set; }
        public string RegistrationDisabledMessage { get; set; }
        public DateTime? DisablePublicRegistration { get; set; }
    }

    public class EventContentInfo
    {
        public int PageContentID { get; set; }

        public string Heading { get; set; }

        public string Body { get; set; }

        public string PageTitle { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        public int ContentAreaID { get; set; }
        public int? ContentCategoryID { get; set; }
        public int? ContentSubCategoryID { get; set; }
        public int? PageTypeID { get; set; }

        public SEOChangeFrequency ChangeFrequency { get; set; }

        public DateTime? LastModified { get; set; }

        public string LastModifiedBy { get; set; }

        public double? Priority { get; set; }

        public bool AddToSitemap { get; set; }

        public int? LayoutTypeID { get; set; }
        public List<EventContentBlockInfo> ContentBlocks { get; set; }
        public List<EventImageInfo> PageContentImages { get; set; }
        public string UrlSegment { get; set; }
    }
    public class EventContentBlockInfo
    {
        public int ContentBlockID { get; set; }
        public string BlockTitle { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class EventImageInfo
    {
        public int ImageID { get; set; }
        public string Alt { get; set; }
        public string Thumbnail { get; set; }
        public int DisplayOrder { get; set; }
    }
}
