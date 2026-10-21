using Travel.Web.DTOs.CategoryDtos;

namespace Travel.Web.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<List<ResultCategoryDto>> GetAllAsync();
    }
}
