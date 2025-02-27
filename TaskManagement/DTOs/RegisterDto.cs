using System.ComponentModel.DataAnnotations;

namespace TaskManagement.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage ="UserName is required")]
        public string? Username { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required")]

        public string? Password { get; set; }
    }
}
