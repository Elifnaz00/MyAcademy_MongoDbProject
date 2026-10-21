namespace Travel.Web.DTOs.TourDtos
{
    public class TourFilterDto
    {
        public string? Search { get; set; }

        public List<string> Destinations { get; set; } = new();

        public List<string> Durations { get; set; } = new();

        public List<string> Prices { get; set; } = new();

        public List<string> Types { get; set; } = new();

        public string? Rating { get; set; }

        public string Sort { get; set; } = "onerilen";
    }
}
