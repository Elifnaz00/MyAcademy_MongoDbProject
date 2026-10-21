using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Travel.Web.DTOs.BannerDtos;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.BannerServices
{
    public class BannerServices : IBannerService
    {
        private readonly IMongoCollection<Banner> _bannerCollection;
        private readonly IMapper _mapper;


        public BannerServices(IDatabaseSettings databaseSettings, IMapper mapper)
        {
            var client= new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);   
            _bannerCollection = database.GetCollection<Banner>(databaseSettings.BannerCollectionName);

            _mapper = mapper;
        }

        public async Task CreateAsync(CreateBannerDto createBannerDto)
        {
            var banner= _mapper.Map<Banner>(createBannerDto);
            await _bannerCollection.InsertOneAsync(banner);
        }

        public async Task DeleteAsync(string id)
        {

            await _bannerCollection.DeleteOneAsync(b => b.Id == id);
        }

        public async Task<List<ResultBannerDto>> GetAllAsync()
        {
            var bannerList= await _bannerCollection.AsQueryable().ToListAsync();    
            var mappedBannerList= _mapper.Map<List<ResultBannerDto>>(bannerList);
            return mappedBannerList;
        }

        public async Task<ResultBannerDto> GetByIdAsync(string id)
        {
            var banner= await _bannerCollection.Find(b => b.Id == id).FirstOrDefaultAsync();
            var mappedBanner= _mapper.Map<ResultBannerDto>(banner);
            return mappedBanner;

        }

        public async Task UpdateAsync(UpdateBannerDto updateBannerDto)
        {
            var banner= _mapper.Map<Banner>(updateBannerDto);   
            await _bannerCollection.FindOneAndReplaceAsync(b => b.Id == banner.Id, banner);
        }
    }
}
