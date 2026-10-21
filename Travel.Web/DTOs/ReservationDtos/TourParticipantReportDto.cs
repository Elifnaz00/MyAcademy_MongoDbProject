namespace Travel.Web.DTOs.ReservationDtos
{
    public class TourParticipantReportDto
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Telephone { get; set; } = string.Empty;

        public string TourName { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Adult { get; set; }

        public int Child { get; set; }

        public int TotalPeople => Adult + Child;

        public DateTime ReservationCreatedDate { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
