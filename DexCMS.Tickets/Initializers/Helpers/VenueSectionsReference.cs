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
            BalconyL = Context.VenueSections.Where(x => x.Name == "L" && x.VenueAreaID == VenueAreas.Balcony).Select(x => x.VenueSectionID).SingleOrDefault();
            BalconyM = Context.VenueSections.Where(x => x.Name == "M" && x.VenueAreaID == VenueAreas.Balcony).Select(x => x.VenueSectionID).SingleOrDefault();
            BalconyR = Context.VenueSections.Where(x => x.Name == "R" && x.VenueAreaID == VenueAreas.Balcony).Select(x => x.VenueSectionID).SingleOrDefault();
            LowerSeatingL = Context.VenueSections.Where(x => x.Name == "L" && x.VenueAreaID == VenueAreas.LowerSeating).Select(x => x.VenueSectionID).SingleOrDefault();
            LowerSeatingM = Context.VenueSections.Where(x => x.Name == "M" && x.VenueAreaID == VenueAreas.LowerSeating).Select(x => x.VenueSectionID).SingleOrDefault();
            LowerSeatingR = Context.VenueSections.Where(x => x.Name == "R" && x.VenueAreaID == VenueAreas.LowerSeating).Select(x => x.VenueSectionID).SingleOrDefault();
        }
    }
}
