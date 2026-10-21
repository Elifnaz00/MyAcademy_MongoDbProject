
using AutoMapper;
using MongoDB.Driver;
using Travel.Web.DTOs.BannerDtos;
using Travel.Web.DTOs.CommentDtos;
using Travel.Web.Entities;
using Travel.Web.Services.UserServices;
using Travel.Web.Settings;

namespace Travel.Web.Services.CommentServices
{
    public class CommentService : ICommentService
    {
        private readonly IMongoCollection<Comment> _commentCollection;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IMongoCollection<Tour> _tourCollection;

        public CommentService(IDatabaseSettings databaseSettings, IMapper mapper, IUserService userService)
        {
            var client=new MongoClient(databaseSettings.ConnectionString);
            var database= client.GetDatabase(databaseSettings.DatabaseName);
            _commentCollection= database.GetCollection<Comment>(databaseSettings.CommentCollectionName);
            _tourCollection= database.GetCollection<Tour>(databaseSettings.TourCollectionName);
            _mapper = mapper;
            _userService = userService;
        }

        public async Task CreateAsync(CreateCommentDto createCommentDto, string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);

            if (user is null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            var comment = _mapper.Map<Comment>(createCommentDto);

            comment.UserId = user.Id;
            comment.UserName = user.UserName;
            comment.UserSurname = user.UserSurName;
            comment.UserImage = user.UserImage;
            comment.CommentDate = DateTime.Now;

            await _commentCollection.InsertOneAsync(comment);
        }

        public async Task<List<ResultCommentDto>> GetCommentsByTourIdAsync(string tourId)
        {

            var comments = await _commentCollection.Find(x => x.TourId == tourId).ToListAsync();
            var commentMapped= _mapper.Map<List<ResultCommentDto>>(comments);
            return commentMapped;
        }

        public async Task<List<ResultCommentDto>> GetCommentsByUserIdAsync(string userId)
        {
            var comments= await _commentCollection.Find(x => x.UserId == userId).ToListAsync();

            var tours = await _tourCollection
        .Find(_ => true)
        .ToListAsync();

            return comments.Select(comment =>
            {
                var tour = tours.FirstOrDefault(x => x.Id == comment.TourId);
                return new ResultCommentDto
                {
                    UserId = comment.UserId,
                    UserName = comment.UserName,
                    UserSurname = comment.UserSurname,
                    UserImage = comment.UserImage,
                    CommentContent = comment.CommentContent,
                    CommentDate = comment.CommentDate,
                    CommentRating = comment.CommentRating,
                    TourId = comment.TourId,
                    TourName = tour?.Name ?? "Bilinmeyen Tur"
                };
            }
            ).ToList();
        }
    }
}
