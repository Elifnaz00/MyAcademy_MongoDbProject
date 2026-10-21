using AutoMapper;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class TourMapping : Profile
    {
        public TourMapping()
        {
            CreateMap<Tour,ResultTourDto>().ReverseMap();

            CreateMap<CreateTourDto, Tour>();

            CreateMap<Tour, ResultPopularTourDto>();
            CreateMap<Tour, ResultTourByCategoryDto>();

            CreateMap<ResultTourDto, UpdateTourDto>();
            CreateMap<UpdateTourDto, Tour>();
           

        }
    }
}
