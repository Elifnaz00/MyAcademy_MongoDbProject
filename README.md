# 🌍 Travelio — Tur Rezervasyon Platformu

Travelio, **ASP.NET Core MVC ve MongoDB** kullanılarak geliştirilen, kullanıcıların seyahat turlarını keşfedebildiği, rezervasyon oluşturabildiği ve seyahat deneyimlerini paylaşabildiği dinamik bir tur rezervasyon platformudur.

Proje kapsamında kullanıcı işlemlerinden tur yönetimine, rezervasyon takibinden yönetim paneline kadar bir seyahat platformunun temel iş akışları ele alınmıştır. Ayrıca Türkçe ve İngilizce dil desteği, raporlama ve istatistiksel gösterimler gibi işlevlerle uygulamanın kapsamı genişletilmiştir.

## 📌 Proje Hakkında

Travelio, MongoDB'nin doküman tabanlı veri modelleme yaklaşımını ASP.NET Core MVC ile birleştirerek gerçek bir seyahat rezervasyon senaryosunu uygulamayı amaçlar.

Projenin temel hedefleri:

- MongoDB ile veri modelleme ve CRUD işlemlerini uygulamak.
- MVC mimarisiyle kullanıcı arayüzü ve sunucu tarafı işlemlerini düzenlemek.
- Kullanıcı ve yönetici rollerine yönelik işlevler geliştirmek.
- Rezervasyon, yorum, favori ve soru-cevap süreçlerini yönetmek.
- Çoklu dil desteği ve raporlama araçlarını projeye entegre etmek.

## ✨ Özellikler


## 🗄️ MongoDB Doküman Modelleme

Travelio, NoSQL veritabanı olarak MongoDB ve veri erişimi için MongoDB.Driver kullanılarak geliştirilmiştir.

Veri modelleri, MongoDB'nin doküman tabanlı yapısına uygun şekilde tasarlanmıştır. Özellikle birbiriyle ilişkili tur bilgilerinin uygun durumlarda tek bir doküman içerisinde tutulmasını sağlayan **iç içe doküman (Nested Document / Embedded Document) modelleme yaklaşımı** kullanılmıştır.

### İç İçe Doküman Yapısı

`Tour` dokümanı içerisinde aşağıdaki gibi turla ilişkili bilgiler tutulabilir:

- **Tur Tarihleri:** Birden fazla tur tarihi ve her tarihe özel kontenjan bilgileri.
- **Günlük Tur Programı:** Gün gün tur programı, açıklamalar ve şehir bilgileri.
- **Görsel Galerisi:** Tura ait görseller.
- **Öne Çıkan Özellikler:** Turun sunduğu özellikler ve avantajlar.

Bu yaklaşım, birbiriyle yakından ilişkili tur verilerinin aynı doküman içerisinde tutulmasını sağlar ve MongoDB'nin gömülü doküman modelleme özelliklerinden yararlanır.

### Ayrı Koleksiyonlar

Diğer iş alanlarına ait veriler ise kendi koleksiyonlarında yönetilmektedir:

- `Tours` — Turlar
- `Destinations` — Destinasyonlar
- `Categories` — Kategoriler
- `Reservations` — Rezervasyonlar
- `Users` — Kullanıcılar
- `Comments` — Yorumlar
- `Questions` — Sorular ve cevaplar
- `Favorites` — Favoriler

Bu yapı, turla doğrudan ilişkili ayrıntılar için gömülü doküman modellemesini; diğer iş alanları için ise ayrı koleksiyon yaklaşımını bir arada ele alır.

### 🏠 Ana Sayfa ve Tur Keşfi
- Tur ve destinasyon bilgilerinin dinamik olarak listelenmesi.
- Popüler destinasyonların görüntülenmesi.
- Tur kartları üzerinden detay sayfalarına erişim.
- Tur listeleme ekranında sıralama ve filtreleme arayüzleri.

### 🧳 Tur Detayları
- Tur açıklamaları, fiyat ve destinasyon bilgilerinin görüntülenmesi.
- Tur detay sayfasında kullanıcı yorumlarının listelenmesi.
- Tur hakkında soru gönderme ve mevcut cevapları görüntüleme.
- Tur içeriklerinin Türkçe ve İngilizce sunulmasına yönelik yerelleştirme desteği.

### 📅 Rezervasyon Yönetimi
- Tur rezervasyonu oluşturma.
- Yetişkin ve çocuk katılımcı bilgilerinin yönetilmesi.
- Rezervasyon bilgilerinin MongoDB üzerinde saklanması.
- Rezervasyon durumlarının takip edilmesi.
- Kullanıcının yalnızca kendi rezervasyonlarını görüntüleyebilmesi.
- Yönetim panelinden rezervasyonları listeleme ve durumlarını güncelleme.

### 👤 Kullanıcı İşlemleri
- Kullanıcı kaydı ve giriş işlemleri.
- Cookie tabanlı kimlik doğrulama.
- Kullanıcıya özel rezervasyonların görüntülenmesi.
- Favori turların yönetimi.
- Tur yorumlarının ve sorularının kullanıcıyla ilişkilendirilmesi.

### ⭐ Yorum ve Puanlama
- Kullanıcıların turlar hakkında yorum yapabilmesi.
- Yorumlarla birlikte puan bilgilerinin saklanması.
- Tur detay sayfasında yorumların görüntülenmesi.

### 💬 Soru-Cevap Sistemi
- Kullanıcıların tur detay sayfasından soru gönderebilmesi.
- Cevaplanmamış soruların yönetim panelinde listelenmesi.
- Yönetici tarafından soruların yanıtlanması.
- Yanıtlanan soruların kullanıcı tarafında görüntülenmesi.

### 🛠️ Yönetim Paneli
- Tur, destinasyon ve kategori bilgilerinin yönetimi.
- Rezervasyonların görüntülenmesi ve durumlarının güncellenmesi.
- Kullanıcı yorumlarının ve sorularının yönetilmesi.
- Tur ve rezervasyon istatistiklerinin dashboard üzerinden izlenmesi.
- Yönetim işlemlerinin kullanıcı tarafındaki ekranlardan ayrılması.

### 📊 Dashboard ve Raporlama
- Tur ve rezervasyon istatistiklerinin gösterilmesi.
- Popüler turların rezervasyon verileri üzerinden değerlendirilmesi.
- Chart.js ile grafiksel veri sunumu.
- ClosedXML ile Excel raporları ve QuestPDF ile PDF raporları oluşturulmasına yönelik raporlama altyapısı.

### 🌐 Çoklu Dil Desteği
- Türkçe ve İngilizce dil seçenekleri.
- ASP.NET Core Localization ile arayüz metinlerinin yerelleştirilmesi.
- Menü, başlık ve kullanıcı arayüzü metinlerinin seçilen dile göre sunulması.
- Tur içeriklerinin çoklu dil desteğiyle yönetilmesine yönelik çalışma.

## 🧰 Kullanılan Teknolojiler

| Teknoloji | Kullanım Alanı |
|---|---|
| C# | Uygulama geliştirme dili |
| ASP.NET Core MVC | Web uygulaması ve MVC yapısı |
| MongoDB | NoSQL veritabanı |
| MongoDB.Driver | MongoDB veri erişimi |
| Cookie Authentication | Kullanıcı kimlik doğrulama |
| FluentValidation | Veri doğrulama |
| AutoMapper | Nesneler arası dönüşüm |
| Chart.js | İstatistik ve grafik gösterimleri |
| ClosedXML | Excel raporlama |
| QuestPDF | PDF raporlama |
| ASP.NET Core Localization | Çoklu dil desteği |
| HTML, CSS, JavaScript | Arayüz geliştirme |
| Bootstrap, jQuery, AJAX | Arayüz düzeni ve etkileşimler |
| Git & GitHub | Sürüm kontrolü |

## 🗄️ Veritabanı

Projenin veri katmanında MongoDB ve MongoDB.Driver kullanılmıştır.

Uygulamadaki temel veri grupları şunlardır:

- **Tours:** Tur bilgileri.
- **Destinations:** Destinasyon bilgileri.
- **Categories:** Tur kategorileri.
- **Reservations:** Rezervasyon kayıtları.
- **Users:** Kullanıcı bilgileri.
- **Comments:** Tur yorumları ve puanları.
- **Questions:** Tur soruları ve yanıtları.
- **Favorites:** Kullanıcıların favori turları.

Projede ayrıca banner ve rota gibi içerikler için kullanılan koleksiyonlar da bulunmaktadır.

MongoDB'nin esnek doküman yapısı, tur ve rezervasyon gibi farklı iş alanlarına ait verilerin yönetilmesinde kullanılmıştır.

## 🏗️ Uygulama Yapısı

Proje, ASP.NET Core MVC yaklaşımıyla geliştirilmiştir. Sorumluluklar controller, servis, entity ve DTO gibi bileşenler üzerinden ayrıştırılmıştır.

- **Controllers:** HTTP isteklerini karşılar ve ilgili işlemleri yönetir.
- **Services:** Veri erişimi ve uygulama işlemlerini yürütür.
- **Entities:** MongoDB üzerinde saklanan veri modellerini tanımlar.
- **DTOs:** Katmanlar arasında taşınan verileri düzenler.
- **Views:** Razor kullanılarak kullanıcı arayüzlerini oluşturur.
- **Localization resources:** Yerelleştirilmiş arayüz metinlerini barındırır.

## 🖼️ Proje Ekran Görüntüleri

### 1. Ana Sayfa ve Tur Sayfası

![MongoDB Proje Görseli 1](screenshots/mongo1.png)
![MongoDB Proje Görseli 2](screenshots/mongo2.png)
![MongoDB Proje Görseli 3](screenshots/mongo3.png)
![MongoDB Proje Görseli 5](screenshots/mongo5.png)
![MongoDB Proje Görseli 6](screenshots/mongo6.png)

### 1. İngilizce Arayüz Örneği
![MongoDB Proje Görseli 4](screenshots/mongo4.png)

### 2. Tur Detay Sayfası ~ Gün Gün Tur Programı ~ Rezervasyon ~ Yorumlar ~ Sorular
![MongoDB Proje Görseli 7](screenshots/mongo7.png)
![MongoDB Proje Görseli 8](screenshots/mongo8.png)
![MongoDB Proje Görseli 9](screenshots/mongo9.png)
![MongoDB Proje Görseli 10](screenshots/mongo10.png)

### 3. Kullanıcı Giriş ve Kayıt Ekranları

![MongoDB Proje Görseli 11](screenshots/mongo11.png)
![MongoDB Proje Görseli 12](screenshots/mongo12.png)

### 4. Kullanıcı ~ Rezervasyonlarım ~ Yorumlarım ~ Sorularım ~ Favorilerim
![MongoDB Proje Görseli 13](screenshots/mongo13.png)
![MongoDB Proje Görseli 14](screenshots/mongo14.png)
![MongoDB Proje Görseli 15](screenshots/mongo15.png)
![MongoDB Proje Görseli 16](screenshots/mongo16.png)

### 4. Yönetim Paneli ve Raporlama

![MongoDB Proje Görseli 27](screenshots/mongoson.png)
![MongoDB Proje Görseli 18](screenshots/mongo18.png)
![MongoDB Proje Görseli 19](screenshots/mongo19.png)
![MongoDB Proje Görseli 20](screenshots/mongo20.png)
![MongoDB Proje Görseli 21](screenshots/mongo21.png)
![MongoDB Proje Görseli 22](screenshots/mongo22.png)
![MongoDB Proje Görseli 23](screenshots/mongo18.png)
![MongoDB Proje Görseli 24](screenshots/mongo19.png)
![MongoDB Proje Görseli 25](screenshots/mongo20.png)
![MongoDB Proje Görseli 26](screenshots/mongo21.png)
![MongoDB Proje Görseli 27](screenshots/mongo22.png)



## 🎯 Projede Edinilen Deneyimler

Travelio geliştirme süreci boyunca:

- ASP.NET Core MVC ile dinamik web uygulaması geliştirme,
- MongoDB doküman modelleri ve veri erişimi,
- Servis ve DTO tabanlı kod organizasyonu,
- Kimlik doğrulama ve kullanıcıya özel veri yönetimi,
- Form doğrulama ve nesne eşleme,
- Çoklu dil desteği,
- Dashboard, grafikler ve raporlama araçlarıyla çalışma

konularında pratik deneyim kazanılması hedeflenmiştir.

## 👩‍💻 Geliştirici

**Elifnaz Demir**

- GitHub: [Elifnaz00](https://github.com/Elifnaz00)
- Proje deposu: [MyAcademy_MongoDbProject](https://github.com/Elifnaz00/MyAcademy_MongoDbProject)

---

*Travelio, ASP.NET Core MVC ve MongoDB teknolojileriyle seyahat rezervasyon süreçlerini uygulamak amacıyla geliştirilen bir eğitim ve portföy projesidir.*
