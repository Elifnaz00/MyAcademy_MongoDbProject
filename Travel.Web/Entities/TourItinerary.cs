namespace Travel.Web.Entities
{
    public class TourItinerary
    {
        public string Day { get; set; } //kaçıncı gün

        public string DayTitle { get; set; } //Istanbul -> Roma

        public string ProgramDescription { get; set; }

        public string Accomodation { get; set; } //konaklanan yer

        public string TransportType { get; set; }
        public string Meal { get; set; } //öğün
    }
}
