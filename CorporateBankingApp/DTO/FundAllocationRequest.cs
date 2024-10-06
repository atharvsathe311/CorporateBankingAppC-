namespace CorporateBankingApp.DTO
{
    public class FundAllocationRequest
    {
        public int Id { get; set; }
        public decimal InvestmentsAndGrowth { get; set; }
        public decimal EmergencyFunds { get; set; }
        public decimal AccountPayables { get; set; }
        public decimal Taxes { get; set; }
        public decimal SalaryPayments { get; set; }
    }
}
