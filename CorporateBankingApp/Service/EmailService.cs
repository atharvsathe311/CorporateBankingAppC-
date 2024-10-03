using System.Net.Mail;
using System.Net;

namespace CorporateBankingApp.Service
{
    public class EmailService : IEmailService
    {
        public async void SendEmail(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential("bankingcorporate011@gmail.com", "uwlnioczhmkmvztb")
            };

            try
            {
                Console.WriteLine("Sending Email ..");
                smtpClient.SendMailAsync("bankingcorporate011@gmail.com", toEmail, subject, body).Wait();
                Console.WriteLine("Email Sent");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

    }
}

