using Travel.Web.Entities.Enums;

namespace Travel.Web.DTOs.ReservationDtos
{
    public class RecentBookingsDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string TourId { get; set; }

        public string TourName { get; set; }
        public int TotalParticipants { get; set; }

        public DateTime StartDate { get; set; }
        public decimal TotalPrice { get; set; }

        public ReservationStatus ReservationStatus { get; set; }
    }
}
