using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Travel.Web.DTOs.BannerDtos;
using Travel.Web.DTOs.CategoryDtos;
using Travel.Web.DTOs.ReservationDtos;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.Enums;
using Travel.Web.Settings;


namespace Travel.Web.Services.TourServices
{
    public class TourService : ITourService
    {
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Destination> _destinationCollection;
        private readonly IMongoCollection<Comment> _commentCollection;

        public TourService(IDatabaseSettings databaseSettings, IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _tourCollection = database.GetCollection<Tour>(databaseSettings.TourCollectionName);
            
            _destinationCollection =
    database.GetCollection<Destination>("Destinations");
            _mapper = mapper;
            _commentCollection = database.GetCollection<Comment>("Comments");
        }


        private string GetLocalizedValue(
            LocalizedText? translations,
            string? fallback)
        {
            bool isEnglish = CultureInfo.CurrentUICulture
                .Name.StartsWith("en", StringComparison.OrdinalIgnoreCase);

            string? translatedValue = isEnglish
                ? translations?.En
                : translations?.Tr;

            return !string.IsNullOrWhiteSpace(translatedValue)
                ? translatedValue
                : fallback ?? string.Empty;
        }

        private void ApplyCurrentLanguage(ResultTourDto tour)
        {
            tour.Name = GetLocalizedValue(
                tour.NameTranslations,
                tour.Name);

            tour.Description = GetLocalizedValue(
                tour.DescriptionTranslations,
                tour.Description);

            tour.ShortImportantDescription = GetLocalizedValue(
                tour.ShortImportantDescriptionTranslations,
                tour.ShortImportantDescription);
        }
        private void ApplyCurrentLanguage(ResultPopularTourDto tour)
        {
            tour.Name = GetLocalizedValue(
                tour.NameTranslations,
                tour.Name);
        }
        public async Task ChangeStatusAsync(ResultTourDto resultTourDto)
        {
            resultTourDto.State = resultTourDto.State == TourState.Active ? TourState.Passive : TourState.Active;
           var changeTourState= _mapper.Map<Tour>(resultTourDto);
            await _tourCollection.ReplaceOneAsync(a=> a.Id == resultTourDto.Id, changeTourState);

        }

        public async Task CreateAsync(CreateTourDto createTourDto)
        {
            var tour = _mapper.Map<Tour>(createTourDto);
            await _tourCollection.InsertOneAsync(tour);
        }

        public async Task<List<ResultTourDto>> GetAllAsync()
        {
            var tours = await _tourCollection
                .AsQueryable()
                .ToListAsync();

            var destinations = await _destinationCollection
                .Find(Builders<Destination>.Filter.Empty)
                .ToListAsync();

            var result = _mapper.Map<List<ResultTourDto>>(tours);
           

            foreach (var tourDto in result)
            {
                ApplyCurrentLanguage(tourDto);
                var destination = destinations.FirstOrDefault(
                    d => d.Id == tourDto.DestinationId);

                tourDto.DestinationCountry = destination?.Country;
            }

            return result;
        }

        public async Task<List<ResultPopularTourDto>> GetPopularToursAsync()
        {

            var popularTours = await _tourCollection
                .Find(x => x.State == TourState.Active && x.IsFeatured)
                .Limit(8)
                .ToListAsync();

            var mappedPopularTours =
                _mapper.Map<List<ResultPopularTourDto>>(popularTours);

            foreach (var tour in mappedPopularTours)
            {
                ApplyCurrentLanguage(tour);
            }

            return mappedPopularTours;
        
        }


        public async Task<ResultTourDto> GetTourByIdAsync(string tourId)
        {
            var result = await _tourCollection
                .Find(g => g.Id == tourId)
                .FirstOrDefaultAsync();

            if (result is null)
            {
                return null!;
            }

            var resultMapped = _mapper.Map<ResultTourDto>(result);

            ApplyCurrentLanguage(resultMapped);

            return resultMapped;
        }

        
       public async Task<List<ResultTourByCategoryDto>> GetTourByCategoryAsync()
        {
            var documents = await _tourCollection
                .Aggregate()
                .Lookup(
                    "Categories",
                    "CategoryId",
                    "_id",
                    "Category"
                )
                .Unwind("Category")
                .Project(new BsonDocument
                {
            { "_id", 1 },
            { "Name", 1 },
            { "NameTranslations", 1 },
            { "Price", 1 },
            { "MaxQuota", 1 },
            { "MinQuota", 1 },
            { "DayTimeDuration", 1 },
            { "NightTimeDuration", 1 },
            { "ImageUrl", 1 },
            { "TourType", 1 },
            { "CategoryId", 1 },
            { "State", 1 },

            { "Category", new BsonDocument
                {
                    { "_id", "$Category._id" },
                    { "name", "$Category.name" }
                }
            }
                })
                .ToListAsync();

            bool isEnglish = CultureInfo.CurrentUICulture.Name
                .StartsWith("en", StringComparison.OrdinalIgnoreCase);

            var result = documents.Select(x =>
            {
                LocalizedText? translations =
                    x.Contains("NameTranslations") &&
                    x["NameTranslations"].IsBsonDocument
                        ? MongoDB.Bson.Serialization.BsonSerializer
                            .Deserialize<LocalizedText>(
                                x["NameTranslations"].AsBsonDocument)
                        : null;

                string originalName = x["Name"].AsString;

                string? translatedName = isEnglish
                    ? translations?.En
                    : translations?.Tr;

                return new ResultTourByCategoryDto
                {
                    Id = x["_id"].AsObjectId.ToString(),

                    Name = !string.IsNullOrWhiteSpace(translatedName)
                        ? translatedName
                        : originalName,

                    NameTranslations = translations,

                    Price = x["Price"].ToDecimal(),
                    MaxQuota = x["MaxQuota"].AsString,
                    MinQuota = x["MinQuota"].AsString,
                    DayTimeDuration = x["DayTimeDuration"].AsString,
                    NightTimeDuration = x["NightTimeDuration"].AsString,

                    ImageUrl = x["ImageUrl"].IsBsonNull
                        ? null
                        : x["ImageUrl"].AsString,

                    TourType = x["TourType"].AsString,
                    CategoryId = x["CategoryId"].AsObjectId.ToString(),

                    State = Enum.Parse<TourState>(x["State"].AsString),

                    Category = new ResultCategoryDto
                    {
                        Id = x["Category"]["_id"].AsObjectId.ToString(),
                        Name = x["Category"]["name"].AsString
                    }
                };
            }).ToList();

            return result;
        }


        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _tourCollection.DeleteOneAsync(
                Builders<Tour>.Filter.Eq(x => x.Id, id)
            );

            return result.DeletedCount > 0;
        }



        public async Task UpdateAsync(UpdateTourDto updateTourDto)
        {
            var tour = await _tourCollection
                            .Find(x => x.Id == updateTourDto.Id)
                            .FirstOrDefaultAsync();

            if (tour is null)
            {
                return;
            }

            _mapper.Map(updateTourDto, tour);

            await _tourCollection.ReplaceOneAsync(
                x => x.Id == tour.Id,
                tour);
        }

        public async Task<long> GetTotalCountToursAsync()
        {       
            return await _tourCollection.CountDocumentsAsync(FilterDefinition<Tour>.Empty);
        }

        public async Task<long> GetActiveToursCountAsync()
        {
            var filter = Builders<Tour>.Filter.Eq(x => x.State ,TourState.Active);
            return  await _tourCollection.CountDocumentsAsync(filter);
          
        }

        public async Task<long> GetPassiveToursCountAsync()
        {
            var filter = Builders<Tour>.Filter.Eq(x => x.State ,TourState.Passive);
            return await _tourCollection.CountDocumentsAsync(filter);
           
        }

        public async Task<int> GetTotalQuotaCountAsync()
        {
            var tours = await _tourCollection
         .Find(FilterDefinition<Tour>.Empty)
         .ToListAsync();

           return tours.Sum(x => int.Parse(x.MaxQuota));
        }


        public async Task<List<ResultTourDto>> GetFilteredToursAsync(
            TourFilterDto filter)
        {
            var builder = Builders<Tour>.Filter;
            var mongoFilter = builder.Empty;

            
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchRegex = new BsonRegularExpression(
                    Regex.Escape(filter.Search.Trim()),
                    "i");

                mongoFilter &= builder.Or(
                    builder.Regex(x => x.Name, searchRegex),
                    builder.Regex(x => x.StartingCity, searchRegex),
                    builder.Regex(x => x.TourRoute, searchRegex)
                );
            }

          
            if (filter.Prices != null && filter.Prices.Any())
            {
                var priceFilters = new List<FilterDefinition<Tour>>();

                foreach (var price in filter.Prices)
                {
                    switch (price)
                    {
                        case "p1":
                            priceFilters.Add(
                                builder.Gte(x => x.Price, 0m) &
                                builder.Lt(x => x.Price, 25000m));
                            break;

                        case "p2":
                            priceFilters.Add(
                                builder.Gte(x => x.Price, 25000m) &
                                builder.Lt(x => x.Price, 40000m));
                            break;

                        case "p3":
                            priceFilters.Add(
                                builder.Gte(x => x.Price, 40000m) &
                                builder.Lt(x => x.Price, 60000m));
                            break;

                        case "p4":
                            priceFilters.Add(
                                builder.Gte(x => x.Price, 60000m));
                            break;
                    }
                }

                if (priceFilters.Any())
                {
                    mongoFilter &= builder.Or(priceFilters);
                }
            }

           
            if (filter.Types != null && filter.Types.Any())
            {
                mongoFilter &= builder.In(
                    x => x.TourType,
                    filter.Types);
            }

            
            var tours = await _tourCollection
                .Find(mongoFilter)
                .ToListAsync();

            var result = _mapper.Map<List<ResultTourDto>>(tours);
            foreach (var tour in result)
            {
                ApplyCurrentLanguage(tour);
            }


            var destinations = await _destinationCollection
                .Find(Builders<Destination>.Filter.Empty)
                .ToListAsync();

            foreach (var tour in result)
            {
                var destination = destinations.FirstOrDefault(
                    d => d.Id == tour.DestinationId);

                tour.DestinationCountry = destination?.Country;
            }

           
            string Normalize(string? value)
            {
                if (string.IsNullOrWhiteSpace(value))
                    return string.Empty;

                return value.Trim()
                    .ToLowerInvariant()
                    .Replace('ı', 'i')
                    .Replace('ş', 's')
                    .Replace('ğ', 'g')
                    .Replace('ü', 'u')
                    .Replace('ö', 'o')
                    .Replace('ç', 'c');
            }

            
            if (filter.Destinations != null &&
                filter.Destinations.Any())
            {
                var selectedCountries = filter.Destinations
                    .Select(Normalize)
                    .ToHashSet();

                result = result
                    .Where(x => selectedCountries.Contains(
                        Normalize(x.DestinationCountry)))
                    .ToList();
            }

          
            if (filter.Durations != null &&
                filter.Durations.Any())
            {
                result = result.Where(tour =>
                {
                    var match = Regex.Match(
                        tour.DayTimeDuration ?? string.Empty,
                        @"\d+");

                    if (!match.Success ||
                        !int.TryParse(match.Value, out int days))
                    {
                        return false;
                    }

                    return filter.Durations.Any(duration =>
                        duration switch
                        {
                            "1-3" => days >= 1 && days <= 3,
                            "4-5" => days >= 4 && days <= 5,
                            "6-7" => days >= 6 && days <= 7,
                            "8+" => days >= 8,
                            _ => false
                        });
                }).ToList();
            }

           
            var tourIds = result
                .Select(x => x.Id)
                .ToList();

            var comments = tourIds.Count == 0
                ? new List<Comment>()
                : await _commentCollection
                    .Find(x => tourIds.Contains(x.TourId))
                    .ToListAsync();

            
            var ratingGroups = comments
                .Where(x => x.CommentRating >= 1 &&
                            x.CommentRating <= 5)
                .GroupBy(x => x.TourId)
                .ToDictionary(
                    group => group.Key,
                    group => new
                    {
                        Average = group.Average(
                            x => x.CommentRating),
                        Count = group.Count()
                    });

            foreach (var tour in result)
            {
                if (ratingGroups.TryGetValue(tour.Id, out var rating))
                {
                    tour.AverageRating = Math.Round(
                        rating.Average, 1);

                    tour.RatingCount = rating.Count;
                }
                else
                {
                    tour.AverageRating = 0;
                    tour.RatingCount = 0;
                }
            }

            
            decimal? minimumRating = filter.Rating switch
            {
                "r45" => 4.5m,
                "r40" => 4.0m,
                "r35" => 3.5m,
                _ => null
            };

            if (minimumRating.HasValue)
            {
                result = result
                    .Where(x => x.RatingCount > 0 &&
                                x.AverageRating >= minimumRating.Value)
                    .ToList();
            }

            
            result = filter.Sort switch
            {
                "puan" => result
                    .OrderByDescending(x => x.AverageRating)
                    .ToList(),

                "fiyat-artan" => result
                    .OrderBy(x => x.Price)
                    .ToList(),

                "fiyat-azalan" => result
                    .OrderByDescending(x => x.Price)
                    .ToList(),

                _ => result
            };

            return result;
        }

        public async Task<long> GetPopularToursCountAsync()
        {
            return await _tourCollection.Find(Builders<Tour>.Filter.Eq(x => x.IsFeatured, true)).CountDocumentsAsync();
        }

        
    }





}

