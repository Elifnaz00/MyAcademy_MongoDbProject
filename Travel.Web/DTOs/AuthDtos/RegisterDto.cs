namespace Travel.Web.DTOs.AuthDtos
{
    public class RegisterDto
    {
        public string UserName { get; set; }
        public string UserSurName { get; set; }
        public string UserEmail { get; set; }
        public string Telephone { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string? RoleName { get; set; }
    }
}
