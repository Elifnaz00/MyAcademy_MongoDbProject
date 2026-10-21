using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Travel.Web.Entities;
using Travel.Web.Entities.Enums;

namespace Travel.Web.DTOs.TourDtos
{
    public class CreateTourDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ShortImportantDescription { get; set; }

        public string TourRoute { get; set; }

        public decimal Price { get; set; }

        public string MaxQuota { get; set; }
        public string MinQuota { get; set; }
        public string Accomodation { get; set; } //konaklanan yer

        public string TransportType { get; set; }
        public string GuideLanguage { get; set; }
        public string VisaInformation { get; set; }

        public string DayTimeDuration { get; set; }

        public string NightTimeDuration { get; set; }

        public string? ImageUrl { get; set; }

        public string TourType { get; set; }

        public List<string>? GalleryImages { get; set; }
        public string DestinationId { get; set; }

        public string CategoryId { get; set; }

        public List<TourDate> TourDates { get; set; }


        public string StartingCity { get; set; }
        public List<string> Features { get; set; }

        public List<string> PriceIncludes { get; set; }
        public List<string> NotPriceIncludes { get; set; }

        [BsonRepresentation(BsonType.String)]
        public TourState State { get; set; }

        public List<TourItinerary> Itineraries { get; set; }

        public LocalizedText? NameTranslations { get; set; }

        public LocalizedText? DescriptionTranslations { get; set; }

        public LocalizedText? ShortImportantDescriptionTranslations { get; set; }

    }
}
