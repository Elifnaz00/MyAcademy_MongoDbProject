using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class Question : BaseEntity
    {
        public string UserId { get; set; }

        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserImage { get; set; }
        public string QuestionContent { get; set; }
        public DateTime CreatedAt { get; set; }
        public string TourId { get; set; }
        public string TourName { get; set; }
        public string? AnsweredText { get; set; }

        public bool IsReply { get; set; }


    }
}
