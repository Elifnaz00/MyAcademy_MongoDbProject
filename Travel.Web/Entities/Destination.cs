using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class Destination : BaseEntity
    {
       public string? Country { get; set; }

       public string? City { get; set; }

       public string? ShortDescription { get; set; }

       public bool IsPopular { get; set; }
       public string? ImageUrl { get; set; }

    }
}
