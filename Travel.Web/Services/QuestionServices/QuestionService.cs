using AutoMapper;
using MongoDB.Driver;
using Travel.Web.DTOs.QıuestionDto;
using Travel.Web.DTOs.QıuestionDtos;
using Travel.Web.Entities;
using Travel.Web.Services.UserServices;
using Travel.Web.Settings;

namespace Travel.Web.Services.QuestionServices
{
    public class QuestionService : IQuestionService
    {

        private readonly IMongoCollection<Question> _questionCollection;
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public QuestionService(IUserService userService,IDatabaseSettings databaseSettings, IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);   
            _questionCollection = database.GetCollection<Question>(databaseSettings.QuestionCollectionName);
            _tourCollection = database.GetCollection<Tour>(databaseSettings.TourCollectionName);
            _mapper = mapper;
            _userService = userService;
        }

        public async Task CreateQuestionAsync(CreateQuestionDto createQuestionDto, string userId)
        {
           var user=  await _userService.GetUserByIdAsync(userId);
            if(user is null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }
            var question= _mapper.Map<Question>(createQuestionDto);  
           
            question.UserId= userId;
            question.CreatedAt= DateTime.UtcNow;
            question.UserSurname= user.UserSurName;
            question.UserName= user.UserName;
            question.UserImage= user.UserImage;
            question.IsReply = false;
          
            await _questionCollection.InsertOneAsync(question);

            
        }

        public async Task<List<ResultQuestionDto>> GetAllQuestionsByTourAsync(string tourId)
        {
            var questions= await _questionCollection.Find(q => q.TourId == tourId).ToListAsync();
            var mappedQuestions= _mapper.Map<List<ResultQuestionDto>>(questions);  
            return mappedQuestions;
        }

        public async Task<List<ResultQuestionDto>> GetAllQuestionsByUserIdAsync(string userId)
        {
            var questions = await _questionCollection.Find(q => q.UserId == userId).ToListAsync();
            var tours = await _tourCollection.Find(_ => true).ToListAsync();
            

            return questions.Select(question =>
            {
                var tour = tours.FirstOrDefault(t => t.Id == question.TourId);

                return new ResultQuestionDto
                {
                    Id = question.Id,
                    QuestionContent = question.QuestionContent,
                    TourId = question.TourId,
                    UserId = question.UserId,
                    CreatedAt = question.CreatedAt,
                    UserName = question.UserName,
                    UserSurname = question.UserSurname,
                    UserImage = question.UserImage,
                    TourName = tour.Name,
                    IsReply= question.IsReply,
                };}).OrderByDescending(x=>x.CreatedAt).ToList();

        }

        public async Task<ResultQuestionDto> GetByIdQuestionAsync(string Id)
        {
            var question= await _questionCollection.Find(x => x.Id == Id).FirstOrDefaultAsync();
            if(question is null)
            {
                throw new Exception("Soru bulunamadı.");
            }
            return _mapper.Map<ResultQuestionDto>(question);
        }

        public async Task<long> GetPendingQuestionsCountAsync()
        {
            return await _questionCollection.CountDocumentsAsync(x=> !x.IsReply);
        }

        public async Task<List<ResultQuestionDto>> GetUnAnswerdQuestionsAsync()
        {
            var questions= await _questionCollection.Find(x=>!x.IsReply).ToListAsync();
            var mappedQuestions= _mapper.Map<List<ResultQuestionDto>>(questions);
            return mappedQuestions;
        }

        public async Task ReplyQuestionAsync(ReplyQuestionDto replyQuestionDto)
        {
            var question = await _questionCollection.Find(x => x.Id == replyQuestionDto.QuestionId).FirstOrDefaultAsync();
            if (question is null)
            {
                throw new Exception("Soru bulunamadı.");
            }

            question.AnsweredText = replyQuestionDto.AnsweredText;
            question.IsReply = true;
            await _questionCollection.ReplaceOneAsync(x => x.Id == replyQuestionDto.QuestionId, question);
            
            
        }
    }
    }

