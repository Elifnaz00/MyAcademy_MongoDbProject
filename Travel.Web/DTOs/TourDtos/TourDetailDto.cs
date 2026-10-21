using Travel.Web.DTOs.CommentDtos;
using Travel.Web.DTOs.QıuestionDto;

namespace Travel.Web.DTOs.TourDtos
{
    public class TourDetailDto
    {
        public ResultTourDto Tour { get; set; }
        public List<ResultCommentDto> Comments { get; set; }
        public List<ResultQuestionDto> Questions { get; set; }
    }
}
