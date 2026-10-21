using Travel.Web.Entities.Enums;

namespace Travel.Web.DTOs.ReservationDtos
{
    public class ResultReservationDto
    {
      
            public string Id { get; set; }

            public int Child { get; set; }

            public int Adult { get; set; }

            public decimal TotalPrice { get; set; }

            public DateTime ReservationCreatedDate { get; set; }

            public DateTime StartDate { get; set; }

            public DateTime EndDate { get; set; }

            public string TourId { get; set; }

            public string? TourName { get; set; }

            public string? TourImageUrl { get; set; }

            public string? TourType { get; set; }

            public ReservationStatus ReservationStatus { get; set; }
        }
    
}
