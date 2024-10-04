using CorporateBankingApp.Models;
using Microsoft.AspNetCore.Http;

namespace CorporateBankingApp.DTO
{
    public class NewClientDTO
    {
        public string CompanyName { get; set; }
        public string FounderName { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public int BankId { get; set; }
    }
}
