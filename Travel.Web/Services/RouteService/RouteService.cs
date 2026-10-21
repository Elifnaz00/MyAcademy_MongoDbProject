using AutoMapper;
using Travel.Web.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Travel.Web.DTOs.RouteDtos;
using Travel.Web.Settings;
using Route = Travel.Web.Entities.Route;


namespace Travel.Web.Services.RouteService
{
    public class RouteService : IRouteService
    {

        private readonly IMongoCollection<Route> _routeCollection;
        private readonly IMapper _mapper;

        public RouteService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);   
            _routeCollection = database.GetCollection<Route>(databaseSettings.RouteCollectionName);
            _mapper = mapper;
        }

        public async Task CreateRouteAsync(CreateRouteDto createRouteDto)
        {
            var route = _mapper.Map<Route>(createRouteDto);    
            await _routeCollection.InsertOneAsync(route);
        }

        public async Task DeleteRouteAsync(string id)
        {
            await _routeCollection.DeleteOneAsync(r => r.Id == id);
        }

        public async Task<List<ResultRouteDto>> GetAllByCityAsynyc(string city)
        {
            var routes= await _routeCollection.Find(r => r.City.ToLower().Contains(city.ToLower())).ToListAsync();
            return _mapper.Map<List<ResultRouteDto>>(routes);
        }

        public async Task<List<ResultRouteDto>> GetAllRoutesAsync()
        {
           var routes= await _routeCollection.AsQueryable().ToListAsync();
           return _mapper.Map<List<ResultRouteDto>>(routes);
        }

        public async Task<ResultRouteDto> GetRouteByIdAsync(string id)
        {
            var route= await _routeCollection.Find(x=> x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<ResultRouteDto>(route);
        }

        public async Task UpdateRouteAsync(UpdateRouteDto updateRouteDto)
        {
            var route = _mapper.Map<Route>(updateRouteDto); 
            await _routeCollection.FindOneAndReplaceAsync(r => r.Id == route.Id, route);
        }
    }
}
