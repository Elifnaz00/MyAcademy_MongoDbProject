namespace Travel.Web.DTOs.CommentDtos
{
    public class CreateCommentDto
    {
        public string TourId { get; set; }
        public string CommentContent { get; set; }
        public decimal CommentRating { get; set; }
    }
}
