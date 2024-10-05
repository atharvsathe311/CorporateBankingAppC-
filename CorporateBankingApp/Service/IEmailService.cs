namespace CorporateBankingApp.Service
{
    public interface IEmailService
    {
        void SendEmail(string toEmail, string subject, string body);
        void SendNewRegistrationMail(string toEmail,string companyName,string loginUsername,string password);
    }
}
