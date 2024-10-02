namespace CorporateBankingApp.Models
{
    public class BeneficiaryList
    {
        public int BeneficiaryId { get; set; }
        public List<Client> ListofBeneficiaryInbound { get; set; }
    }
}
