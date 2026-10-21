namespace Travel.Web.DTOs.StatisticDto
{
    public class StatisticDto
    {
        public long TotalTours { get; set; }
        public long ActiveTours { get; set; }
       
        public long TotalReservations { get; set; }
        public long PopularTours { get; set; }
        public long TotalUsers { get; set; }
        public long PendingQuestions { get; set; }
    }
}
