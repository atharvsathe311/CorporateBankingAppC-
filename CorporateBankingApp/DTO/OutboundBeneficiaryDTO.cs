using CorporateBankingApp.Models;

namespace CorporateBankingApp.DTO
{
    public class OutboundBeneficiaryDTO
    {
        public string CompanyName { get; set; }
        public BankAccount Account { get; set; }
    }

}
