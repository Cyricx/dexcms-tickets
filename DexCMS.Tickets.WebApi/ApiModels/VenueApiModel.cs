using System.Collections.Generic;

namespace DexCMS.Tickets.WebApi.ApiModels
{
    public class VenueApiModel
    {
        public int VenueID { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public int StateID { get; set; }

        public string ZipCode { get; set; }

        public List<VenueAreaApiModel> VenueAreas { get; set; }

    }

    public class VenueAreaApiModel
    {
        public int? VenueAreaID { get; set; }
        public string Name { get; set; }
        public bool IsGA { get; set; }
        public int? GASeatCount { get; set; }
        public List<VenueSectionApiModel> VenueSections { get; set; }
    }

    public class VenueSectionApiModel
    {
        public int? VenueSectionID { get; set; }
        public string Name { get; set; }
        public List<VenueRowApiModel> VenueRows { get; set; }
    }
    public class VenueRowApiModel
    {
        public int? VenueRowID { get; set; }
        public string Designation { get; set; }
        public int SeatCount { get; set; }
    }
}
