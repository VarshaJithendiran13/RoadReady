namespace CarRental.Models.DTOs
{
    public class PasswordResetDTO
    {
        public int ResetId { get; set; }
        public int UserId { get; set; }
        public string ResetToken { get; set; } = null!;
        public DateTime ExpirationDate { get; set; }
        public bool IsUsed { get; set; }
        public string UserEmail { get; set; } = null!;
    }
}
