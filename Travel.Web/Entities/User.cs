using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class User : BaseEntity
    {
       public string UserName { get; set; }
        public string UserSurName { get; set; }

        public string UserEmail { get; set; }

        public string Telephone { get; set; }

        public string Password { get; set; }

        public string RoleName { get; set; } 

        public string UserImage { get; set; }

    }
}
