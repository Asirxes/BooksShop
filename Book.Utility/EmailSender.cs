using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace Book.Utility;

public class EmailSender : IEmailSender
{
    public EmailSender(IConfiguration _config)
    {
        // SendGridSecret = _config.GetValue<string>("SendGrid:SecretKey");

        LoginSecret = _config.GetValue<string>("Gmail:Login");

        PasswordSecret = _config.GetValue<string>("Gmail:Password");
    }
    // public string SendGridSecret { get; set; }

    public string LoginSecret { get; set; }

    public string PasswordSecret { get; set; }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var emailToSend = new MimeMessage();
        emailToSend.From.Add(MailboxAddress.Parse(LoginSecret));
        emailToSend.To.Add(MailboxAddress.Parse(email));
        emailToSend.Subject = subject;
        emailToSend.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };


        using (var emailClient = new SmtpClient())
        {
            emailClient.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            emailClient.Authenticate(LoginSecret, PasswordSecret);
            emailClient.Send(emailToSend);
            emailClient.Disconnect(true);
        }

        return Task.CompletedTask;

        // var client = new SendGridClient(SendGridSecret);
        // var from = new EmailAddress("hello@dotnetmastery.com", "Bulky Book");
        // var to = new EmailAddress(email);
        // var msg = MailHelper.CreateSingleEmail(from, to, subject,"", htmlMessage);
        // return client.SendEmailAsync(msg);
    }
}