using Travel.Web.DTOs.QıuestionDto;
using Travel.Web.DTOs.QıuestionDtos;

namespace Travel.Web.Services.QuestionServices
{
    public interface IQuestionService
    {
        Task<List<ResultQuestionDto>> GetAllQuestionsByTourAsync(string tourId);
        Task CreateQuestionAsync(CreateQuestionDto createQuestionDto, string userId);

        Task<List<ResultQuestionDto>> GetAllQuestionsByUserIdAsync(string userId);

        Task<long> GetPendingQuestionsCountAsync();

        Task<List<ResultQuestionDto>> GetUnAnswerdQuestionsAsync();

        Task<ResultQuestionDto> GetByIdQuestionAsync(string Id);

        Task ReplyQuestionAsync(ReplyQuestionDto replyQuestionDto);
    }
}
