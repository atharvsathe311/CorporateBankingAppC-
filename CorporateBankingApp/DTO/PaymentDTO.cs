namespace CorporateBankingApp.DTO
{
    public class PaymentDTO
    {
        public string Amount { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Remarks { get; set; }
    }

    public class SalaryDTO
    {
        public string Amount { get; set; }
        public int SenderId { get; set; }
        public int ReceiverBankAccountNumber { get; set; }
        public string ReceiverBankAccountIFSCCode { get; set; }
        public string Remarks { get; set; }
    }
}
