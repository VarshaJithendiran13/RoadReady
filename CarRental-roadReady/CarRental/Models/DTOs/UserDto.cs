using System.ComponentModel.DataAnnotations;

namespace CarRental.Models.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(100, ErrorMessage = "First name cannot be longer than 100 characters.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(100, ErrorMessage = "Last name cannot be longer than 100 characters.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(200, ErrorMessage = "Email cannot be longer than 200 characters.")]
        public string Email { get; set; } = null!;

        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; }

        [StringLength(50, ErrorMessage = "Role cannot be longer than 50 characters.")]
        public string? Role { get; set; }
    }
}
