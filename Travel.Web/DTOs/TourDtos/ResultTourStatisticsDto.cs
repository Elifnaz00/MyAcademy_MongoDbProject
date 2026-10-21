namespace Travel.Web.DTOs.TourDtos
{
    public class ResultTourStatisticsDto
    {
        public long TotalTourCount { get; set; }
        public long ActiveTourCount { get; set; }
        public long PassiveTourCount { get; set; }
        public int TotalQuota { get; set; }
    }
}
