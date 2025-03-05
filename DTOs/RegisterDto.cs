using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Password is required.")]

        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required."), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required."), MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }
}
