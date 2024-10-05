using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CorporateBankingApp.DTO
{
    public class RejectTransactionDTO
    {
        public int TransactionId  { get; set; }
        public string TransactionRemark { get; set; }
    }
}
