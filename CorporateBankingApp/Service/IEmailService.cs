namespace CorporateBankingApp.Service
{
    public interface IEmailService
    {
        void SendEmail(string toEmail, string subject, string body);
    }
}
