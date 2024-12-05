using System.ComponentModel.DataAnnotations;

namespace CarRental.Models.DTOs
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        public string Password { get; set; }

        public string Role { get; set; } // Optional role, e.g., "User" or "Admin"
    }
}
