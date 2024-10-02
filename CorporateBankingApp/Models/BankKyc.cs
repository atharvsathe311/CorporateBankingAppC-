using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;

namespace CorporateBankingApp.Models
{
    public class BankKyc
    {
        public int BankKycId { get; set; }
        public string LicenseNumber { get; set; }
        public FileDetail TaxpayerIdentificationNumber { get; set; }
        public FileDetail LicenseRegulatorApprovalsOrLicenseAgreement { get; set; }
        public FileDetail FinancialStatements{ get; set; }
        public FileDetail AnnualReports { get; set; }

    }
}
