namespace Travel.Web.DTOs.FavoriteDto
{
    public class ResultFavoriteDto
    {
        public string TourName { get; set; }
        public DateTime CreatedAt { get; set; }

        public string ShortImportantDescription { get; set; } 

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }


    }
}
