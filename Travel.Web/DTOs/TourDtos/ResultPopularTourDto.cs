using Travel.Web.Entities;

namespace Travel.Web.DTOs.TourDtos
{
    public class ResultPopularTourDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public LocalizedText? NameTranslations { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }

        public string DayTimeDuration { get; set; }

        public  string DestinationId { get; set; }
        public List<TourItinerary> Itineraries { get; set; }

     
    }
}
