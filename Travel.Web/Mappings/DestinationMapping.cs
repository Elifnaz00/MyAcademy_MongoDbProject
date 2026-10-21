using AutoMapper;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class DestinationMapping : Profile
    {
        public DestinationMapping() 
        {
            CreateMap<Destination, DTOs.DestinationDtos.ResultDestinationDto>();
           
        }
    }
}
