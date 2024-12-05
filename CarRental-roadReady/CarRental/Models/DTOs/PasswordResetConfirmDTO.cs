namespace CarRental.Models.DTOs
{
    public class PasswordResetConfirmDTO
    {
        public string ResetToken { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
    }

}
