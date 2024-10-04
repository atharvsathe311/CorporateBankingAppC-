namespace CorporateBankingApp.Models
{
    public class GetAllBeneficiary
    {
        public string Name { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankIFSC {  get; set; }
        public bool IsOutbound { get; set; }
    }
}
