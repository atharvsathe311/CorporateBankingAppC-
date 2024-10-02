namespace CorporateBankingApp.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime DateTime { get; set; }
        public string Amount { get; set; }

    }
}
