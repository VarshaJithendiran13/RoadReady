using CarRental;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

public class EmailService: IEmailService

{
    private readonly string _smtpServer = "smtp.gmail.com"; // Gmail SMTP server
    private readonly int _port = 587; // TLS port
    private readonly string _senderEmail = "rentalc82@gmail.com"; // Sender email address
    private readonly string _senderPassword = "dozs krfd qgxe ijke"; // Sender email password (use an App Password if 2FA is enabled)

    public async Task SendEmailAsync(string recipientEmail, string subject, string body)
    {
        var email = new MimeMessage();

        // Set sender email
        email.From.Add(new MailboxAddress("CarRental Service", _senderEmail));

        // Set recipient email
        email.To.Add(new MailboxAddress("", recipientEmail));

        // Set subject and body
        email.Subject = subject;
        email.Body = new TextPart("plain") { Text = body };

        using var smtpClient = new SmtpClient();
        try
        {
            // Connect to the SMTP server and authenticate
            await smtpClient.ConnectAsync(_smtpServer, _port, MailKit.Security.SecureSocketOptions.StartTls);

            // Authenticate using sender email and app password
            await smtpClient.AuthenticateAsync(_senderEmail, _senderPassword);

            // Send the email
            await smtpClient.SendAsync(email);
        }
        catch (Exception ex)
        {
            // Handle exception if there is an error during the email sending process
            Console.WriteLine($"An error occurred while sending the email: {ex.Message}");
        }
        finally
        {
            // Disconnect the SMTP client, ensuring the connection is closed properly
            await smtpClient.DisconnectAsync(true);
            smtpClient.Dispose();
        }
    }
}
