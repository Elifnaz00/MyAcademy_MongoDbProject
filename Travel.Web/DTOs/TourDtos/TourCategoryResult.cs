using Travel.Web.Entities;
using Travel.Web.Entities.Enums;

namespace Travel.Web.DTOs.TourDtos
{
    public class TourCategoryResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string MaxQuota { get; set; }
        public string MinQuota { get; set; }
        public string DayTimeDuration { get; set; }
        public string NightTimeDuration { get; set; }
        public string? ImageUrl { get; set; }
        public string TourType { get; set; }
        public string CategoryId { get; set; }
        public TourState State { get; set; }

        public Category Category { get; set; }
    }
}
