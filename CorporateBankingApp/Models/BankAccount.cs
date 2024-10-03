using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateBankingApp.Models
{
    public class BankAccount
    {
        [Key]
        public int AccountId { get; set; }

        [ForeignKey("BankId")]
        public int BankId { get; set; }
        public double Balance { get; set; }
        public double BlockedFunds { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
