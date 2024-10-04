namespace CorporateBankingApp.DTO
{
    public class BankKYCDTO
    {
        public int BankId { get; set; }
        public string LicenseNumber { get; set; }
        public string TINNumber { get; set; }
        public IFormFile LicenseAgreement { get; set; }
        public IFormFile FinancialStatement { get; set; }
        public IFormFile AnnualReport { get; set; }

    }
}