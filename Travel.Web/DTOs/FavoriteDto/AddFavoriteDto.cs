namespace Travel.Web.DTOs.FavoriteDto
{
    public class AddFavoriteDto
    {
        public string TourId { get; set; }
        public string UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
