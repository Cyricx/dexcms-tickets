using DexCMS.Tickets.Contexts;
using System.Linq;

namespace DexCMS.Tickets.Initializers.Helpers
{
    public class VenueSectionsReference
    {
        public int BalconyL { get; set; }
        public int BalconyM { get; set; }
        public int BalconyR { get; set; }
        public int LowerSeatingL { get; set; }
        public int LowerSeatingM { get; set; }
        public int LowerSeatingR { get; set; }

        private VenueAreasReference VenueAreas { get; set; }
        public VenueSectionsReference(IDexCMSTicketsContext Context)
        {
            VenueAreas = new VenueAreasReference(Context);
            BalconyL = Context.VenueSections.Where(x => x.Name == "L" && x.VenueAreaID == VenueAreas.Balcony).Select(x => x.VenueSectionID).Single();
            BalconyM = Context.VenueSections.Where(x => x.Name == "M" && x.VenueAreaID == VenueAreas.Balcony).Select(x => x.VenueSectionID).Single();
            BalconyR = Context.VenueSections.Where(x => x.Name == "R" && x.VenueAreaID == VenueAreas.Balcony).Select(x => x.VenueSectionID).Single();
            LowerSeatingL = Context.VenueSections.Where(x => x.Name == "L" && x.VenueAreaID == VenueAreas.LowerSeating).Select(x => x.VenueSectionID).Single();
            LowerSeatingM = Context.VenueSections.Where(x => x.Name == "M" && x.VenueAreaID == VenueAreas.LowerSeating).Select(x => x.VenueSectionID).Single();
            LowerSeatingR = Context.VenueSections.Where(x => x.Name == "R" && x.VenueAreaID == VenueAreas.LowerSeating).Select(x => x.VenueSectionID).Single();
        }
    }
}
