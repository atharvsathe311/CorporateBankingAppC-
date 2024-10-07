using System.Net.Mail;
using System.Net;
using CloudinaryDotNet;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;
using System;
using CorporateBankingApp.Data;
using CorporateBankingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CorporateBankingApp.Service
{
    public class EmailService : IEmailService
    {
        private readonly CorporateBankAppDbContext _context;
        public EmailService(CorporateBankAppDbContext corporateBankAppDbContext) 
        {
            _context = corporateBankAppDbContext;
        }
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

        public async Task SendNewTransactionEmail(string toEmail, string BankName, string accountNumber, string transactionId, decimal transactionAmount, DateTime transactionDate, string transactionStatus, string transactionRemarks)
        {
            string subject = "New Transaction Alert";
            string body = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>New Transaction Alert</title>
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
        .details {{
            background-color: #f9f9f9;
            padding: 15px;
            border: 1px solid #dddddd;
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
            New Transaction Alert
        </div>
        <div class=""content"">
            <h1>Dear Customer,</h1>
            <p>Transaction Alert for your {BankName} Bank Account</p>
            <p>A new Transaction was submitted for Approval with following Details</p>

            <div class=""details"">
                <h3>Transaction Details:</h3>
                <p><strong>Account Number:</strong> {accountNumber}</p>
                <p><strong>Transaction ID:</strong> {transactionId}</p>
                <p><strong>Amount:</strong> {transactionAmount}</p>
                <p><strong>Date:</strong> {transactionDate}</p>
                <p><strong>Status:</strong> {transactionStatus}</p>
                <p><strong>Remarks:</strong> {transactionRemarks}</p>
            </div>

            <p>If you have any questions regarding this transaction, please feel free to contact our support team at <strong>bankingcorporate011@gmail.com</strong>.</p>
            <p>Thank you for using <strong>CIB Portal</strong>!</p>
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

            using (var smtpClient = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential("bankingcorporate011@gmail.com", "uwlnioczhmkmvztb")
            })
            {
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
                    Console.WriteLine("Sending Email...");
                    await smtpClient.SendMailAsync(mailMessage);
                    Console.WriteLine("Email Sent");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }
            }
        }

        public async void SendTransactionStatusEmail(string toEmail, string bankName, string transactionStatus, string accountNumber, string transactionId, decimal transactionAmount, DateTime transactionDate, string transactionRemarks)
        {
            string subject = "Transaction Status Update";


            var body = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Transaction Status Update</title>
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
        .details {{
            background-color: #f9f9f9;
            padding: 15px;
            border: 1px solid #dddddd;
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
            Transaction Status Update
        </div>
        <div class=""content"">
            <h1>Dear Customer,</h1>
                <p>Your transaction for the {bankName} Bank Account with <strong>Transaction ID {transactionId}</strong> has been <strong>{transactionStatus}</strong>.</p>
            <p>Here are the details of your transaction:</p>

            <div class=""details"">
                <h3>Transaction Details:</h3>
                <p><strong>Account Number:</strong> {accountNumber}</p>
                <p><strong>Transaction ID:</strong> {transactionId}</p>
                <p><strong>Amount:</strong> {transactionAmount}</p>
                <p><strong>Date:</strong> {transactionDate}</p>
                <p><strong>Status:</strong> {transactionStatus}</p>
                <p><strong>Remarks:</strong> {transactionRemarks}</p>
            </div>

            <p>If you have any questions regarding this transaction, please feel free to contact our support team at <strong>bankingcorporate011@gmail.com</strong>.</p>
            <p>Thank you for using <strong>CIB Portal</strong>!</p>
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
                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine("Email Sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public async void SendTransactionStatusEmailReceived(string toEmail, string bankName, string accountNumber, string transactionId, decimal transactionAmount, DateTime transactionDate, string transactionRemarks,string senderName , decimal salaryPayments, decimal accountPayables, decimal taxes, decimal emergencyFunds, decimal investmentsAndGrowth)
        {
            string subject = "Funds Credited Alert";
            decimal salaryPaymentsVar = transactionAmount * (salaryPayments / 100);
            decimal accountPayablesVar = transactionAmount * (accountPayables / 100);
            decimal taxesVar = transactionAmount * ( taxes / 100);
            decimal emergencyFundsVar = transactionAmount * (emergencyFunds / 100);
            decimal investmentsAndGrowthVar = transactionAmount * (investmentsAndGrowth / 100);

            var body = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Funds Credited Alert</title>
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
        .details {{
            background-color: #f9f9f9;
            padding: 15px;
            border: 1px solid #dddddd;
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
            Funds Credited Alert
        </div>
        <div class=""content"">
            <h1>Dear Customer,</h1>
            <p>Your {bankName} Account with Account Number <strong>{accountNumber}</strong> have been credited with <strong>{transactionAmount}</strong></p>
            <p>Below are the details of the transaction:</p>

            <div class=""details"">
                <h3>Transaction Details:</h3>
                <p><strong>Transaction ID:</strong> {transactionId}</p>
                <p><strong>Credited Amount:</strong> {transactionAmount}</p>
                <p><strong>Date:</strong> {transactionDate}</p>
                <p><strong>Sender Name:</strong> {senderName}</p>
                <p><strong>Remarks:</strong> {transactionRemarks}</p>
            </div>
            <div class=""details"">
                <h3> Funds Split - Up:</h3>
                <p><strong> Salary Payments:</strong>{salaryPaymentsVar}</p>
                <p><strong> Account Payables:</strong> {accountPayablesVar}</p>
                <p><strong> Taxes:</strong > {taxesVar}</p>
                <p><strong> Emergency Funds:</strong> {emergencyFundsVar}</p>
                <p><strong> Investments and Growth:</strong> {investmentsAndGrowthVar}</p>
            </div>

            <p> If you have any questions regarding this transaction, please feel free to contact our support team at<strong> bankingcorporate011@gmail.com </strong>.</p>
            <p> Thank you for using <strong> CIB Portal </strong>!</p>
        </div>

        <div class=""footer"">
            Best regards,<br>
            <strong>Atharv & Amit</strong><br>
            CIB Team<br>
            <a href = ""https://corporate-internet-banking.vercel.app/login"">Visit our website</a><br>
            [Support Contact Information]
        </div>
    </div>
</body>
</html>";

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
                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine("Email Sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }


        public async void SendKycApprovalEmail(string toEmail, string bankName, string kycId,string licenseAgreementUrl ,string financialStatementUrl,string annualReportUrl)
        {
            string subject = "KYC Approved - Dashboard Access Granted";

            var body = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>KYC Approval</title>
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
        .details {{
            background-color: #f9f9f9;
            padding: 15px;
            border: 1px solid #dddddd;
        }}
        .footer {{
            margin-top: 30px;
            font-size: 12px;
            color: #777777;
            text-align: center;
        }}
        a {{
            color: #0d6efd;
            text-decoration: none;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            KYC Approved
        </div>
        <div class=""content"">
            <h1>Dear {bankName} Team,</h1>
            <p>We are pleased to inform you that your KYC with <strong>ID {kycId}</strong> has been approved.</p>
            <p>You can now log in to the <strong>Corporate Internet Banking Portal</strong> and start accessing your dashboard for various banking operations and services.</p>

            <div class=""details"">
                <h3>KYC Details:</h3>
                <p><strong>Bank Name:</strong> {bankName}</p>
                <p><strong>KYC ID:</strong> {kycId}</p>

                <h3>Uploaded Documents:</h3>
                <p><strong>License Agreement:</strong> <a href=""{licenseAgreementUrl}"">Download</a></p>
                <p><strong>Financial Statement:</strong> <a href=""{financialStatementUrl}"">Download</a></p>
                <p><strong>Annual Report:</strong> <a href=""{annualReportUrl}"">Download</a></p>
            </div>

            <p>If you encounter any issues while accessing your dashboard, please reach out to our support team at <strong>bankingcorporate011@gmail.com</strong>.</p>
            <p>Thank you for choosing <strong>CIB Portal</strong>!</p>
        </div>

        <div class=""footer"">
            Best regards,<br>
            <strong>Atharv & Amit</strong><br>
            CIB Team<br>
            <a href=""https://corporate-internet-banking.vercel.app/login"">Login to your dashboard</a><br>
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
                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine("Email Sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async void SendClientKycApprovalEmail(string toEmail, string clientName, string kycId,
    string powerOfAttorneyUrl, string bankAccessUrl, string mouUrl)
        {
            string subject = "Client KYC Approval";

            var body = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Client KYC Approval</title>
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
        .details {{
            background-color: #f9f9f9;
            padding: 15px;
            border: 1px solid #dddddd;
        }}
        .footer {{
            margin-top: 30px;
            font-size: 12px;
            color: #777777;
            text-align: center;
        }}
        a {{
            color: #0d6efd;
            text-decoration: none;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            Client KYC Approved
        </div>
        <div class=""content"">
            <h1>Dear {clientName},</h1>
            <p>We are happy to inform you that your KYC with <strong>ID {kycId}</strong> has been successfully approved.</p>
            <p>You can now log in to the <strong>Corporate Internet Banking Portal</strong> to access your dashboard for banking services and operations.</p>

            <div class=""details"">
                <h3>KYC Details:</h3>
                <p><strong>Client Name:</strong> {clientName}</p>
                <p><strong>KYC ID:</strong> {kycId}</p>

                <h3>Uploaded Documents:</h3>
                <p><strong>Power of Attorney:</strong> <a href=""{powerOfAttorneyUrl}"">Download</a></p>
                <p><strong>Bank Access:</strong> <a href=""{bankAccessUrl}"">Download</a></p>
                <p><strong>MOU:</strong> <a href=""{mouUrl}"">Download</a></p>
            </div>

            <p>If you need any assistance while accessing your dashboard, feel free to reach out to our support team at <strong>bankingcorporate011@gmail.com</strong>.</p>
            <p>Thank you for choosing <strong>CIB Portal</strong>!</p>
        </div>

        <div class=""footer"">
            Best regards,<br>
            <strong>Atharv & Amit</strong><br>
            CIB Team<br>
            <a href=""https://corporate-internet-banking.vercel.app/login"">Login to your dashboard</a><br>
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
                Console.WriteLine("Sending Email...");
                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine("Email Sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async void SendKycRejectedEmail(string toEmail, string bankName, string kycId, string remark)
        {
            string subject = "KYC Rejected";

            var body = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Bank KYC Rejection</title>
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
            background-color: #dc3545;
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
        .details {{
            background-color: #f9f9f9;
            padding: 15px;
            border: 1px solid #dddddd;
        }}
        .footer {{
            margin-top: 30px;
            font-size: 12px;
            color: #777777;
            text-align: center;
        }}
        a {{
            color: #0d6efd;
            text-decoration: none;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            Bank KYC Rejected
        </div>
        <div class=""content"">
            <h1>Dear { bankName},</h1>
            <p>We regret to inform you that your KYC application with <strong>ID {kycId}</strong> has been rejected.</p>
            <p>Please re-register your application with the corrected documents.</p>

            <div class=""details"">
                <h4>Rejection Reasons:</h4><strong>: {remark}</strong>
            </div>

            <p>If you have any questions regarding the rejection, feel free to contact our support team at <strong>bankingcorporate011@gmail.com</strong>.</p>
            <p>Thank you for choosing <strong>CIB Portal</strong>! We look forward to receiving your updated KYC application.</p>
        </div>

        <div class=""footer"">
            Best regards,<br>
            <strong>Atharv & Amit</strong><br>
            CIB Team<br>
            <a href=""https://corporate-internet-banking.vercel.app/login"">Login to your dashboard</a><br>
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
                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine("Email Sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async void SendClientKycRejectedEmail(string toEmail, string clientName, string kycId,string remark)
        {
            string subject = "KYC Rejected";

            var body = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Client KYC Rejection</title>
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
            background-color: #dc3545;
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
        .details {{
            background-color: #f9f9f9;
            padding: 15px;
            border: 1px solid #dddddd;
        }}
        .footer {{
            margin-top: 30px;
            font-size: 12px;
            color: #777777;
            text-align: center;
        }}
        a {{
            color: #0d6efd;
            text-decoration: none;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            Client KYC Rejected
        </div>
        <div class=""content"">
            <h1>Dear {clientName},</h1>
            <p>We regret to inform you that your KYC application with <strong>ID {kycId}</strong> has been rejected.</p>
            <p>Please re-register your application with the corrected documents.</p>

            <div class=""details"">
                <h3>Rejection Reasons:</h3><strong>: {remark} </strong>
            </div>

            <p>If you have any questions regarding the rejection, feel free to contact our support team at <strong>bankingcorporate011@gmail.com</strong>.</p>
            <p>Thank you for choosing <strong>CIB Portal</strong>! We look forward to receiving your updated KYC application.</p>
        </div>

        <div class=""footer"">
            Best regards,<br>
            <strong>Atharv & Amit</strong><br>
            CIB Team<br>
            <a href=""https://corporate-internet-banking.vercel.app/login"">Login to your dashboard</a><br>
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
                Console.WriteLine("Sending Email...");
                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine("Email Sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public async void SendSalaryCreditedEmail(string toEmail, string clientName, string transactionId, decimal amount, string bankName, string transactionDate)
        {
            string subject = "Salary Credited for This Month";

            var body = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Salary Credited Notification</title>
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
        .details {{
            background-color: #f9f9f9;
            padding: 15px;
            border: 1px solid #dddddd;
        }}
        .footer {{
            margin-top: 30px;
            font-size: 12px;
            color: #777777;
            text-align: center;
        }}
        a {{
            color: #0d6efd;
            text-decoration: none;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            Salary Credited for This Month
        </div>
        <div class=""content"">
            <h1>Dear {clientName},</h1>
            <p>We are pleased to inform you that your salary for this month has been successfully credited to your account.</p>
            
            <div class=""details"">
                <h3>Transaction Details:</h3>
                <ul>
                    <li><strong>Transaction ID:</strong> {transactionId}</li>
                    <li><strong>Amount:</strong> ₹{amount}</li>
                    <li><strong>Account Number:</strong> {bankName}</li>
                    <li><strong>Transaction Date:</strong> {transactionDate}</li>
                </ul>
            </div>

            <p>You can check your account balance and recent transactions by logging into your <strong>CIB Portal</strong>.</p>
            <p>If you have any questions or concerns regarding the transaction, please contact our support team at <strong>bankingcorporate011@gmail.com</strong>.</p>
        </div>

        <div class=""footer"">
            Best regards,<br>
            <strong>Atharv & Amit</strong><br>
            CIB Team<br>
            <a href=""https://corporate-internet-banking.vercel.app/login"">Login to your dashboard</a><br>
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
                Console.WriteLine("Sending Email...");
                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine("Email Sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task<string> GetBankDetails(int id)
        {
            Bank bank = await _context.Banks.Where(b => b.BankId == id).FirstOrDefaultAsync();
            return bank.BankName;
        }
    }
}
