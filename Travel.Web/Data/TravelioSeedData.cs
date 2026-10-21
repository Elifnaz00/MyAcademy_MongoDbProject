
using MongoDB.Driver;
using MongoDB.Bson;
using Travel.Web.Entities;
using Travel.Web.Entities.Enums;
using Travel.Web.Settings;

namespace Travel.Web.Data
{
    public class TravelioSeedData
    {
        private readonly IMongoCollection<Category> _categories;
        private readonly IMongoCollection<Destination> _destinations;
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Tour> _tours;
        private readonly IMongoCollection<Reservation> _reservations;
        private readonly IMongoCollection<Comment> _comments;
        private readonly IMongoCollection<Question> _questions;
        private readonly IMongoCollection<Favorite> _favorites;

        public TravelioSeedData(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _categories = database.GetCollection<Category>(
                settings.CategoryCollectionName);

            _destinations = database.GetCollection<Destination>(
                settings.DestinationCollectionName);

            _users = database.GetCollection<User>(
                settings.UserCollectionName);

            _tours = database.GetCollection<Tour>(
                settings.TourCollectionName);

            _reservations = database.GetCollection<Reservation>(
                settings.ReservationCollectionName);

            _comments = database.GetCollection<Comment>(
                settings.CommentCollectionName);

            _questions = database.GetCollection<Question>(
                settings.QuestionCollectionName);

            _favorites = database.GetCollection<Favorite>(
                settings.FavoriteCollectionName);
        }

        public async Task SeedAsync()
        {
            // 1. Kategoriler
            var categories = new[]
            {
                "Kültür Turları",
                "Avrupa Turları",
                "Deniz Tatili",
                "Doğa Turları"
            };

            foreach (var name in categories)
            {
                if (!await _categories.Find(x => x.Name == name)
                    .AnyAsync())
                {
                    await _categories.InsertOneAsync(new Category
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Name = name
                    });
                }
            }

            var categoryList = await _categories
                .Find(_ => true).ToListAsync();

            // 2. Destinasyonlar
            var destinationData = new[]
 {
    (
        Country: "İtalya",
        City: "Roma",
        Description: "Tarihi yapılar ve İtalyan mutfağı.",
        ImageUrl: "https://images.unsplash.com/photo-1552832230-c0197dd311b5?auto=format&fit=crop&w=900&q=80"
    ),
    (
        Country: "Fransa",
        City: "Paris",
        Description: "Sanat, tarih ve romantik sokaklar.",
        ImageUrl: "https://images.unsplash.com/photo-1502602898657-3e91760cbb34?auto=format&fit=crop&w=900&q=80"
    ),
    (
        Country: "İspanya",
        City: "Barselona",
        Description: "Mimari, plajlar ve Akdeniz kültürü.",
        ImageUrl: "https://images.unsplash.com/photo-1583422409516-2895a77efded?auto=format&fit=crop&w=900&q=80"
    ),
    (
        Country: "Endonezya",
        City: "Bali",
        Description: "Tropikal doğa ve eşsiz plajlar.",
        ImageUrl: "https://images.unsplash.com/photo-1537996194471-e657df975ab4?auto=format&fit=crop&w=900&q=80"
    ),
    (
        Country: "Türkiye",
        City: "Kapadokya",
        Description: "Peri bacaları ve sıcak hava balonları.",
        ImageUrl: "https://images.unsplash.com/photo-1570939274717-7eda259b50ed?auto=format&fit=crop&w=900&q=80"
    )
};

            foreach (var item in destinationData)
            {
                if (!await _destinations
                    .Find(x => x.City == item.Item2).AnyAsync())
                {
                    await _destinations.InsertOneAsync(new Destination
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Country = item.Country,
                        City = item.City,
                        ShortDescription = item.Description,
                        ImageUrl = item.ImageUrl,
                        IsPopular = true
                    });
                }
            }

            var destinationList = await _destinations
                .Find(_ => true).ToListAsync();

            // 3. Örnek kullanıcılar
            var userData = new[]
            {
                ("ayse", "Ayşe", "Demir", "ayse@example.com"),
                ("mehmet", "Mehmet", "Yılmaz", "mehmet@example.com"),
                ("zeynep", "Zeynep", "Kaya", "zeynep@example.com"),
                ("can", "Can", "Arslan", "can@example.com"),
                ("elif", "Elif", "Şahin", "elif@example.com")
            };

            foreach (var item in userData)
            {
                if (!await _users.Find(x => x.UserName == item.Item1)
                    .AnyAsync())
                {
                    await _users.InsertOneAsync(new User
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        UserName = item.Item1,
                        UserSurName = item.Item3,
                        UserEmail = item.Item4,
                        Telephone = "",
                        Password = "Travelio123!",
                        RoleName = "user",
                        UserImage = ""
                    });
                }
            }

            var userList = await _users.Find(x => x.RoleName == "user")
                .ToListAsync();

            if (categoryList.Count == 0 ||
                destinationList.Count == 0 ||
                userList.Count == 0)
            {
                throw new InvalidOperationException(
                    "Seed için kategori, destinasyon ve kullanıcı gereklidir.");
            }

            // 4. Turlar
            var tourData = new[]
            {
                ("Roma Keşfi", "Rome Discovery", 18000m, "Roma"),
                ("Paris Rüyası", "Paris Dream", 22000m, "Paris"),
                ("Barselona Tatili", "Barcelona Holiday", 19500m, "Barselona"),
                ("Bali Cenneti", "Bali Paradise", 35000m, "Bali"),
                ("Kapadokya Macerası", "Cappadocia Adventure", 8500m, "Kapadokya"),
                ("İtalya Kültür Rotası", "Italian Culture Route", 26000m, "Roma"),
                ("Paris Sanat Turu", "Paris Art Tour", 24000m, "Paris"),
                ("Barselona ve Deniz", "Barcelona and the Sea", 21000m, "Barselona")
            };
            var today = DateTime.Today;
            foreach (var item in tourData)
            {
                if (await _tours.Find(x => x.Name == item.Item1)
                    .AnyAsync())
                    continue;

                var destination = destinationList
                    .First(x => x.City == item.Item4);

                var category = categoryList[
                    Array.IndexOf(tourData, item) % categoryList.Count];

                var start = DateTime.Today.AddDays(30);

                await _tours.InsertOneAsync(new Tour
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = item.Item1,
                    NameTranslations = new LocalizedText
                    {
                        Tr = item.Item1,
                        En = item.Item2
                    },
                    ShortImportantDescription =
                        $"{item.Item1} ile unutulmaz bir tatil.",
                    ShortImportantDescriptionTranslations = new LocalizedText
                    {
                        Tr = $"{item.Item1} ile unutulmaz bir tatil.",
                        En = $"Enjoy an unforgettable holiday with {item.Item2}."
                    },
                    Description =
                        $"{item.Item1} turunda tarihi yerleri, yerel kültürü ve yöresel lezzetleri keşfedin.",
                    DescriptionTranslations = new LocalizedText
                    {
                        Tr = $"{item.Item1} turunda tarihi yerleri, yerel kültürü ve yöresel lezzetleri keşfedin.",
                        En = $"Discover historic places, local culture and cuisine on {item.Item2}."
                    },
                    Price = item.Item3,
                    MaxQuota = "25",
                    MinQuota = "5",
                    Accomodation = "4 yıldızlı otel",
                    TourRoute = $"{destination.City} şehir turu",
                    TransportType = "Uçak",
                    GuideLanguage = "Türkçe, İngilizce",
                    VisaInformation = "Gidilecek ülkenin vize koşullarını kontrol ediniz.",
                    TourType = "Kültür",
                    DayTimeDuration = "5",
                    NightTimeDuration = "4",
                    ImageUrl = "",
                    GalleryImages = new List<string>(),
                    DestinationId = destination.Id,
                    CategoryId = category.Id,
                    TourDates = new List<TourDate>
{
    new TourDate
    {
        StartDate = today.AddDays(30),
        EndDate = today.AddDays(35),
        Quota = 25,
        IsActive = true
    },
    new TourDate
    {
        StartDate = today.AddDays(45),
        EndDate = today.AddDays(50),
        Quota = 20,
        IsActive = true
    },
    new TourDate
    {
        StartDate = today.AddDays(60),
        EndDate = today.AddDays(65),
        Quota = 25,
        IsActive = true
    },
    new TourDate
    {
        StartDate = today.AddDays(90),
        EndDate = today.AddDays(95),
        Quota = 15,
        IsActive = true
    }
},
                    StartingCity = "İstanbul",
                    Features = new List<string>
                    {
                        "Rehberlik", "Şehir turu", "Serbest zaman"
                    },
                    PriceIncludes = new List<string>
                    {
                        "Konaklama", "Rehberlik hizmeti"
                    },
                    NotPriceIncludes = new List<string>
                    {
                        "Kişisel harcamalar", "Ekstra turlar"
                    },
                    State = TourState.Active,
                    Itineraries = new List<TourItinerary>
                    {
                        new TourItinerary
                        {
                            Day = "1. Gün",
                            DayTitle = $"İstanbul - {destination.City}",
                            ProgramDescription = "Varış ve şehir turu.",
                            Accomodation = "Otel",
                            TransportType = "Uçak",
                            Meal = "Akşam yemeği"
                        },
                        new TourItinerary
                        {
                            Day = "2. Gün",
                            DayTitle = $"{destination.City} keşfi",
                            ProgramDescription = "Tarihi ve turistik yerleri ziyaret.",
                            Accomodation = "Otel",
                            TransportType = "Otobüs",
                            Meal = "Kahvaltı"
                        }
                    },
                    IsFeatured = Array.IndexOf(tourData, item) < 4
                });
            }

            var tourList = await _tours.Find(_ => true).ToListAsync();

            // 5. Rezervasyonlar
            // Yalnızca collection boşsa örnek rezervasyon ekle.
            if (await _reservations.CountDocumentsAsync(_ => true) == 0)
            {
                var now = DateTime.Now;

                for (int i = 0; i < 30; i++)
                {
                    var tour = tourList[i % tourList.Count];
                    var user = userList[i % userList.Count];

                    // Son altı ayın farklı günlerine dağıt.
                    var created = now.Date
                        .AddMonths(-5 + (i % 6))
                        .AddDays((i * 3) % 25)
                        .AddHours(10 + (i % 9));

                    var startDate = created.Date.AddDays(30 + i);

                    await _reservations.InsertOneAsync(new Reservation
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        UserId = user.Id,
                        TourId = tour.Id,
                        Adult = 1 + (i % 4),
                        Child = i % 3,
                        TotalPrice = tour.Price * (1 + (i % 4))
                            + tour.Price * (i % 3) * 0.5m,
                        ReservationCreatedDate = created,
                        StartDate = startDate,
                        EndDate = startDate.AddDays(4),
                        ReservationStatus = (i % 5 == 0)
                            ? ReservationStatus.Bekliyor
                            : (i % 7 == 0)
                                ? ReservationStatus.İptal
                                : ReservationStatus.Onaylandı
                    });
                }
            }

            // 6. Yorumlar
            if (await _comments.CountDocumentsAsync(_ => true) == 0)
            {
                for (int i = 0; i < Math.Min(8, tourList.Count); i++)
                {
                    var tour = tourList[i];
                    var user = userList[i % userList.Count];

                    await _comments.InsertOneAsync(new Comment
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        UserId = user.Id,
                        UserName = user.UserName,
                        UserSurname = user.UserSurName,
                        UserImage = user.UserImage,
                        TourId = tour.Id,
                        CommentContent =
                            $"{tour.Name} turundan çok memnun kaldım. Güzel bir deneyimdi.",
                        CommentDate = DateTime.Now.AddDays(-i * 7),
                        CommentRating = 4 + (i % 2)
                    });
                }
            }

            // 7. Sorular
            if (await _questions.CountDocumentsAsync(_ => true) == 0)
            {
                for (int i = 0; i < Math.Min(6, tourList.Count); i++)
                {
                    var tour = tourList[i];
                    var user = userList[i % userList.Count];
                    var replied = i % 2 == 0;

                    await _questions.InsertOneAsync(new Question
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        UserId = user.Id,
                        UserName = user.UserName,
                        UserSurname = user.UserSurName,
                        UserImage = user.UserImage,
                        QuestionContent = "Tur ücretine ulaşım dahil mi?",
                        CreatedAt = DateTime.Now.AddDays(-i * 3),
                        TourId = tour.Id,
                        TourName = tour.Name,
                        AnsweredText = replied
                            ? "Evet, belirtilen ulaşım hizmeti tur ücretine dahildir."
                            : null,
                        IsReply = replied
                    });
                }
            }

            // 8. Favoriler
            if (await _favorites.CountDocumentsAsync(_ => true) == 0)
            {
                for (int i = 0; i < Math.Min(10, tourList.Count * userList.Count); i++)
                {
                    var user = userList[i % userList.Count];
                    var tour = tourList[i % tourList.Count];

                    var exists = await _favorites.Find(x =>
                        x.UserId == user.Id && x.TourId == tour.Id)
                        .AnyAsync();

                    if (exists)
                        continue;

                    await _favorites.InsertOneAsync(new Favorite
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        UserId = user.Id,
                        TourId = tour.Id,
                        CreatedAt = DateTime.Now.AddDays(-i)
                    });
                }
            }
        }
        public async Task UpdateTourDatesAsync()
        {
            var tours = await _tours.Find(_ => true).ToListAsync();

            var today = DateTime.Today;

            foreach (var tour in tours)
            {
                var tourDates = new List<TourDate>
        {
            new TourDate
            {
                StartDate = today.AddDays(30),
                EndDate = today.AddDays(35),
                Quota = 25,
                IsActive = true
            },
            new TourDate
            {
                StartDate = today.AddDays(45),
                EndDate = today.AddDays(50),
                Quota = 20,
                IsActive = true
            },
            new TourDate
            {
                StartDate = today.AddDays(60),
                EndDate = today.AddDays(65),
                Quota = 25,
                IsActive = true
            },
            new TourDate
            {
                StartDate = today.AddDays(90),
                EndDate = today.AddDays(95),
                Quota = 15,
                IsActive = true
            }
        };

                var update = Builders<Tour>.Update
                    .Set(x => x.TourDates, tourDates);

                await _tours.UpdateOneAsync(
                    x => x.Id == tour.Id,
                    update);
            }
        }
    }
}
