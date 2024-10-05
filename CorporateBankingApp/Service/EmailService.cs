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

        public async void SendNewRegistrationMail(string toEmail, string companyName, string loginUsername, string password)
        {
            string userEmail = "john.doe@example.com"; // User's Username or Email
            string appName = "CIB Portal";
            string subject = "New Registration";

            var body = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Welcome to {appName}</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}
        .container {{
            width: 100%;
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            padding: 20px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }}
        .header {{
            background-color: #0d6efd;
            padding: 10px;
            text-align: center;
            color: #ffffff;
            font-size: 24px;
        }}
        .content {{
            padding: 20px;
        }}
        .content h1 {{
            color: #333333;
        }}
        .credentials {{
            background-color: #f9f9f9;
            padding: 15px;
            border: 1px solid #dddddd;
        }}
        .button {{
            background-color: #0d6efd;
            color: #ffffff;
            padding: 10px 20px;
            text-decoration: none;
            border-radius: 5px;
            display: inline-block;
            margin-top: 20px;
        }}
        .footer {{
            margin-top: 30px;
            font-size: 12px;
            color: #777777;
            text-align: center;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            Welcome to {appName}!
        </div>
        <div class=""content"">
            <h1>Dear {companyName},</h1>
            <p>Welcome to <strong>{appName}</strong>! We’re excited to have you onboard and look forward to providing you with an exceptional experience.</p>

            <div class=""credentials"">
                <h3>Login Details:</h3>
                <p><strong>Username:</strong> {loginUsername}</p>
                <p><strong>Temporary Password:</strong> {password}</p>
            </div>

            <p>You can log in to your account by visiting the following link:</p>
            <a href=""https://corporate-internet-banking.vercel.app/login"" class=""button"">Log In Now</a>

            <h3>Next Steps:</h3>
            <ol>
                <li><strong>Login</strong> using the provided credentials.</li>
                <li>For security reasons, we recommend you <strong>change your password</strong> after logging in. You can do this under your account settings.</li>
            </ol>

            <p>If you need any assistance, feel free to contact our support team at <strong>bankingcorporate011@gmail.com</strong>.</p>
            <p>We’re here to help!</p>
        </div>

        <div class=""footer"">
            Best regards,<br>
            <strong>Atharv & Amit</strong><br>
            CIB Team<br>
            <a href=""https://corporate-internet-banking.vercel.app/login"">Visit our website</a><br>
            [Support Contact Information]
        </div>
    </div>
</body>
</html>
";

            var smtpClient = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential("bankingcorporate011@gmail.com", "uwlnioczhmkmvztb")
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("bankingcorporate011@gmail.com", "Corporate Banking Portal"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            try
            {
                Console.WriteLine("Sending Email ..");
                smtpClient.SendMailAsync(mailMessage).Wait();
                Console.WriteLine("Email Sent");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }




        }
    }
}

