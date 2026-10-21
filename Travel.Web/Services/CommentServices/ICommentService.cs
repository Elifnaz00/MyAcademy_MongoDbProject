using Travel.Web.DTOs.CommentDtos;

namespace Travel.Web.Services.CommentServices
{
    public interface ICommentService
    {
        Task CreateAsync(CreateCommentDto createCommentDto, string userId);

        Task<List<ResultCommentDto>> GetCommentsByTourIdAsync(string tourId);

        Task<List<ResultCommentDto>> GetCommentsByUserIdAsync(string userId);
    }
}
