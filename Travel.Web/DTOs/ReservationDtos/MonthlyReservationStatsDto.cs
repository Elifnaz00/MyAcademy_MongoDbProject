namespace Travel.Web.DTOs.ReservationDtos
{
    public class MonthlyReservationStatsDto
    {
        public string Month { get; set; } = string.Empty;

        public int ReservationCount { get; set; }
    }
}
