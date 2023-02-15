using System.ComponentModel.DataAnnotations;

namespace NewsWeb.WebApp.Models
{
    public class UserLoginModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public bool IsRememberMe { get; set; } = false;
    }
}
