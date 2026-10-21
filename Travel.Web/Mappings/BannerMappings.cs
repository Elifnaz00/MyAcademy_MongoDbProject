using AutoMapper;
using Travel.Web.DTOs.BannerDtos;
using Travel.Web.DTOs.RouteDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class BannerMappings : Profile
    {
        public BannerMappings()
        {
            CreateMap<CreateBannerDto,Banner>().ReverseMap();
            CreateMap<UpdateBannerDto,Banner>().ReverseMap();
            CreateMap<Banner,ResultBannerDto>();
            CreateMap<ResultBannerDto, UpdateBannerDto>().ReverseMap();
        }
    }
}
