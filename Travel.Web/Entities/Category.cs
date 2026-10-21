using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class Category : BaseEntity
    {
        [BsonElement("name")]
        public string Name { get; set; }
    }
}
