using AutoMapper;
using Travel.Web.DTOs.AuthDtos;
using Travel.Web.DTOs.UserDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class UserMapping : Profile 
    {
        public UserMapping()
        {
            CreateMap<RegisterDto, User>();
            CreateMap<User, GetUserDto>();
        }
    }
}
