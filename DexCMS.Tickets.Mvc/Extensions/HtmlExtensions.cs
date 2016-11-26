using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace DexCMS.Tickets.Mvc.Extensions
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString BuildEventList(this HtmlHelper html, object events)
        {
            if (events != null)
            {
                List<Events.Models.Event> evts = (List<Events.Models.Event>)events;
                TagBuilder ulTag = new TagBuilder("ul");
                foreach (var evt in evts)
                {
                    string segment = evt.EventSeriesID.HasValue ? evt.EventSeries.SeriesUrlSegment : evt.EventUrlSegment;

                    TagBuilder liTag = BuildLiAnchor("~/events/" + segment, evt.PageContent.Heading); ;
                    ulTag.InnerHtml += liTag.ToString();

                    if (HttpContext.Current.User.IsInRole("Cashier"))
                    {
                        ulTag.InnerHtml += BuildLiAnchor("~/secure/checkins/" + segment, evt.PageContent.Heading + " Check In").ToString();
                    }
                }

                return new MvcHtmlString(ulTag.ToString());
            }
            else
            {
                return new MvcHtmlString("");
            }
        }

        private static TagBuilder BuildLiAnchor(string url, string text)
        {
            TagBuilder liTag = new TagBuilder("li");
            TagBuilder aTag = new TagBuilder("a");
            aTag.Attributes["title"] = text;

            aTag.Attributes["href"] = UrlHelper.GenerateContentUrl(url, new HttpContextWrapper(HttpContext.Current));

            aTag.InnerHtml = text;

            liTag.InnerHtml = aTag.ToString();
            return liTag;
        }
    }
}
