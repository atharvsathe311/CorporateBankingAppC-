using CorporateBankingApp.Models;
using Microsoft.Identity.Client;

namespace CorporateBankingApp.DTO
{
    public class ViewSubmittedClientDTO
    {
        public IFormFile Logo { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public StatusEnum Status { get; set; }
    }
}
