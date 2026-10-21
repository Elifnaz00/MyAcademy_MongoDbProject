using AutoMapper;
using Travel.Web.DTOs.RouteDtos;

namespace Travel.Web.Mappings
{
    public class RouteMapping : Profile
    {
       public RouteMapping() { 
            CreateMap<Route, ResultRouteDto>();
            CreateMap<CreateRouteDto, Route>().ReverseMap();
            CreateMap<UpdateRouteDto, Route>().ReverseMap();
            CreateMap<ResultRouteDto, UpdateRouteDto>().ReverseMap();

        }
    }
}
