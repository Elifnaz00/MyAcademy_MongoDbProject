namespace Travel.Web.DTOs.UserDtos
{
    public class GetUserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserSurName { get; set; }

        public string UserEmail { get; set; }
        public string UserImage { get; set; }

        public string Telephone { get; set; }

        public string RoleName { get; set; }
    }
}
