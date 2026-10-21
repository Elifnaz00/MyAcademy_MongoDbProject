using Travel.Web.DTOs.AuthDtos;
using Travel.Web.DTOs.UserDtos;

namespace Travel.Web.Services.UserServices
{
    public interface IUserService
    {

        Task CreateUserAsync(RegisterDto registerDto);

        Task<GetUserDto> GetUser(string username, string password);

        Task CreateAdminAsync();

        Task<GetUserDto> GetUserByIdAsync(string id);

        Task<long> GetTotalUsersCountAsync();

    }
}
