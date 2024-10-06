namespace CorporateBankingApp.Models
{
    public class BalanceSplitUp
    {
        public decimal SalaryPayments { get; set; }
        public decimal AccountPayables { get; set; }
        public decimal Taxes { get; set; }
        public decimal EmergencyFunds { get; set; }
        public decimal InvestmentsAndGrowth { get; set; }

        public BalanceSplitUp()
        {
            SalaryPayments = 0.35m;         
            AccountPayables = 0.20m;        
            Taxes = 0.30m;                  
            EmergencyFunds = 0.05m;         
            InvestmentsAndGrowth = 0.10m; 
        }
    }
}