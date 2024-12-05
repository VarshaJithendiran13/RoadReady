namespace CarRental.Models.DTOs
{
    public class PasswordResetResponseDTO
    {
        public int ResetId { get; set; }
        public int UserId { get; set; }
        public string ResetToken { get; set; } = null!;
        public DateTime ExpirationDate { get; set; }
        public bool IsUsed { get; set; }

        // The user's email or other necessary info
        public string UserEmail { get; set; } = null!;
    }
}
