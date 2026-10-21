namespace Travel.Web.DTOs.CommentDtos
{
    public class ResultCommentDto
    {

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserImage { get; set; }
        public string CommentContent { get; set; }
        public DateTime CommentDate { get; set; }

        public decimal CommentRating { get; set; }
        public string TourId { get; set; }

        public string TourName { get; set; }

    }
}
