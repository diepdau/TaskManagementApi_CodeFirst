using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Password is required.")]

        public string Username { get; set; } = string.Empty;


        [Required(ErrorMessage = "Email is required."), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required."), MinLength(8)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
