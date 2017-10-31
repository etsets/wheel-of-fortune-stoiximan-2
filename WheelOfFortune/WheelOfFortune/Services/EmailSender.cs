using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WheelOfFortune.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {

        private readonly string  host;
        private readonly string username;
        private readonly string password;

        private SmtpClient smtp;

        public EmailSender()
        {
            host = "e-creativity.gr";
            username = "stoiximan@e-creativity.gr";
            password = "stoiximan123";

            smtp = new SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.Host = host;
            smtp.Credentials = new NetworkCredential(username, password);

        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(this.username);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = message;
            mailMessage.To.Add(email);

            try
            {
                smtp.Send(mailMessage);
                return Task.CompletedTask;
            }
            catch(Exception e)
            {
                throw new Exception("Could not send email");
            }
            
        }
    }
}
