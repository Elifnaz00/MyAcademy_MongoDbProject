namespace Travel.Web.DTOs.QıuestionDtos
{
    public class CreateQuestionDto
    {
        public string? UserId { get; set; }
        public string? QuestionContent { get; set; }
        public string? TourId { get; set; }
        public string? AnsweredText { get; set; }

        public bool IsReply { get; set; }
    }
}
