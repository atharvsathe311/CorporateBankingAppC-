namespace CorporateBankingApp.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime DateTime { get; set; }
        public string Amount { get; set; }
        public StatusEnum Status { get; set; }
        public string Remarks { get; set; }
        public int SenderBankId { get; set; }
        public int ReceiverBankId { get; set; }
    }
}
