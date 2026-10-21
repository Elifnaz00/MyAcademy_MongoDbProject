using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Travel.Web.DTOs.CategoryDtos;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CategoryService(IDatabaseSettings databaseSettings, IMapper mapper)
        {
            var client= new MongoClient(databaseSettings.ConnectionString);
            var database= client.GetDatabase(databaseSettings.DatabaseName);
            _categoryCollection= database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper= mapper;
        }

        public async Task<List<ResultCategoryDto>> GetAllAsync()
        {
            var categories = await _categoryCollection.AsQueryable().ToListAsync();
            var result = _mapper.Map<List<ResultCategoryDto>>(categories);
            return result;
        }
    }
}
