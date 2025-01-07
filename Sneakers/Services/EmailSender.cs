using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace Sneakers.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MailMessage
            {
                From = new MailAddress("anhruoia1a1@gmail.com", "Sneakers"),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(email));

            SmtpClient client = new SmtpClient()
            {
                Port = 587,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Credentials = new NetworkCredential("anhruoia1a1@gmail.com", "ebsygqnndldayced")
            };
            return client.SendMailAsync(message);
        }
    }
}
