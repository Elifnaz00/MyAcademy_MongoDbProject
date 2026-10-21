namespace Travel.Web.DTOs.ReservationDtos
{
    public class ToursWithReservationDto
    {
        public string TourId { get; set; }
        public string TourName { get; set; }
        public string ImageUrl { get; set; }
        public long ReservationCount { get; set; }
     
    }
}
