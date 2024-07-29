using System.ComponentModel.DataAnnotations;

namespace StayHub.Data.ViewModels
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
    public class LoginClaimsModel
    {
        public string id { get; set; }
        public string? token { get; set; }
        public string role { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string expiry { get; set; }
    }
}
