using AutoMapper;

namespace Travel.Web.Mappings
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<Entities.Category, DTOs.CategoryDtos.ResultCategoryDto>();
        }
    }
}
