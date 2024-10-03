using CorporateBankingApp.Models;

namespace CorporateBankingApp.DTO
{
    public class OutboundBeneficiary
    {
        public string CompanyName { get; set; }
        public BankAccount Account { get; set; }
    }
}
