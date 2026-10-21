using Travel.Web.DTOs.DestinationDtos;
using Travel.Web.DTOs.TourDtos;

namespace Travel.Web.Models.HomeModels
{
    public class HomeIndexViewModel
    {
        public List<ResultPopularTourDto> PopularTours { get; set; }

        

        public Dictionary<string, ResultDestinationDto> DestinationLookup { get; set; }
    }
}
