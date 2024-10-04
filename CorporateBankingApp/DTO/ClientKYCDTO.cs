using CorporateBankingApp.Models;

namespace CorporateBankingApp.DTO
{
    public class ClientKYCDTO
    {
        public int ClientId { get; set; }
        public string CINNumber { get; set; }
        public string PanNumber { get; set; }
        public IFormFile PowerOfAttorney { get; set; }
        public IFormFile BankAccess { get; set; }
        public IFormFile MOU { get; set; }
    }
}
