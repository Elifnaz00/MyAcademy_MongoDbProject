using AutoMapper;
using Travel.Web.DTOs.FavoriteDto;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class FavoriteMapping : Profile
    {
        public FavoriteMapping()
        {
            CreateMap<AddFavoriteDto, Favorite>();
            CreateMap<Favorite, ResultFavoriteDto>();
                
        }
    }
}
