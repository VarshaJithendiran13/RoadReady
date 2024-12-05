namespace CarRental
{
    public interface IEmailService
    {
        //Task SendPasswordResetEmailAsync(string email, string resetToken);
        Task SendEmailAsync(string recipientEmail, string subject, string body);
    }

}
