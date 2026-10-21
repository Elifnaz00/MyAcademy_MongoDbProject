using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Travel.Web.Entities.Common;
using Travel.Web.Entities.Enums;

namespace Travel.Web.Entities
{
    public class Reservation : BaseEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        public int Child {  get; set; }

        public int Adult { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime ReservationCreatedDate {  get; set; }

        public DateTime StartDate {  get; set; }

        public DateTime EndDate { get; set; }

        public string TourId { get; set; }

        public ReservationStatus ReservationStatus { get; set; }
    }
}
