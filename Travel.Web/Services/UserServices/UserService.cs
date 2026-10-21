using AutoMapper;
using MongoDB.Driver;
using Travel.Web.DTOs.AuthDtos;
using Travel.Web.DTOs.UserDtos;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMapper _mapper;

        public UserService(IDatabaseSettings databaseSettings, IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _userCollection = database.GetCollection<User>(databaseSettings.UserCollectionName);
            _mapper = mapper;
        }

        public async Task CreateAdminAsync()
        {
            var existingAdmin = _userCollection.Find(x => x.UserName == "admin").FirstOrDefault();
            if (existingAdmin != null)
            {
                return; 
            }
            var admin = new User
            {
                UserName = "admin",
                Password = "Admin123",
                RoleName = "admin"
            };

            await _userCollection.InsertOneAsync(admin);
        }

        public async Task CreateUserAsync(RegisterDto registerDto)
        {
     
            var mappedUser= _mapper.Map<RegisterDto, User>(registerDto);
            await _userCollection.InsertOneAsync(mappedUser);

        }

        public async Task<long> GetTotalUsersCountAsync()
        {
            return await _userCollection.CountDocumentsAsync(_ => true);
        }

        public async Task<GetUserDto>GetUser(string username, string password)
        {
           var user=  await _userCollection.Find(x => x.UserName == username && x.Password == password).FirstOrDefaultAsync();
            if (user is null)
            {
                return null;
            }

            return _mapper.Map<User, GetUserDto>(user);    
        }

        public async  Task<GetUserDto> GetUserByIdAsync(string id)
        {
            var user= await _userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (user is null)
            {
                return null;
            }

            return _mapper.Map<User, GetUserDto>(user); 
        }
    }
}
