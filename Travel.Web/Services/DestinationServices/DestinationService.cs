using AutoMapper;

using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Travel.Web.DTOs.BannerDtos;
using Travel.Web.DTOs.DestinationDtos;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.DestinationServices
{
    public class DestinationService : IDestinationService
    {

        private readonly IMongoCollection<Destination> _destinationCollection;
        private readonly IMapper _mapper;

        public DestinationService(IDatabaseSettings databaseSettings, IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _destinationCollection = database.GetCollection<Destination>(databaseSettings.DestinationCollectionName);

            _mapper = mapper;
        }

        public async Task<List<ResultDestinationDto>> GetAllAsync()
        {
            var destinationList= await _destinationCollection.AsQueryable().ToListAsync();
            var mappeddestinationList = _mapper.Map<List<ResultDestinationDto>>(destinationList);
            return mappeddestinationList;
        }

        public async Task<List<ResultDestinationDto>> GetPopularDestinationsAsync()
        {
            var popularDestinations = await _destinationCollection.AsQueryable().Where(d => d.IsPopular).Take(4).ToListAsync();
            var mappedPopularDestinationList = _mapper.Map<List<ResultDestinationDto>>(popularDestinations);
            return mappedPopularDestinationList;
        }

     

    }
}
