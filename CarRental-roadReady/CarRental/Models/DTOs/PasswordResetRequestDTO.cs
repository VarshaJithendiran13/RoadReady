using System.ComponentModel.DataAnnotations;

namespace CarRental.Models.DTOs
{
    public class PasswordResetRequestDTO
    {
        // Email of the user who is requesting the password reset
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = null!;
    }
}


