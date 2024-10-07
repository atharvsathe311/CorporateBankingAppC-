namespace CorporateBankingApp.Service
{
    public interface IEmailService
    {
        void SendEmail(string toEmail, string subject, string body);
        void SendNewRegistrationMail(string toEmail,string companyName,string loginUsername,string password);
        Task SendNewTransactionEmail(string toEmail, string BankName, string accountNumber, string transactionId, decimal transactionAmount, DateTime transactionDate, string transactionStatus, string transactionRemarks);
        void SendTransactionStatusEmail(string toEmail, string bankName, string transactionStatus, string accountNumber, string transactionId, decimal transactionAmount, DateTime transactionDate, string transactionRemarks);
        void SendTransactionStatusEmailReceived(string toEmail, string bankName, string accountNumber, string transactionId, decimal transactionAmount, DateTime transactionDate, string transactionRemarks, string senderName, decimal salaryPayments, decimal accountPayables, decimal taxes, decimal emergencyFunds, decimal investmentsAndGrowth);
        void SendKycApprovalEmail(string toEmail, string bankName, string kycId, string licenseAgreementUrl, string financialStatementUrl, string annualReportUrl);
        void SendClientKycApprovalEmail(string toEmail, string clientName, string kycId, string powerOfAttorneyUrl, string bankAccessUrl, string mouUrl);
        void SendKycRejectedEmail(string toEmail, string bankName, string kycId, string remark);
        void SendClientKycRejectedEmail(string toEmail, string clientName, string kycId, string remark);
        void SendSalaryCreditedEmail(string toEmail, string clientName, string transactionId, decimal amount, string bankName, string transactionDate);
        Task<string> GetBankDetails(int id);
    }
}
