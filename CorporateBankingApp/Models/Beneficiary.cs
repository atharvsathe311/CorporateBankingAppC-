using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace CorporateBankingApp.Models
{
    public class BeneficiaryList
    {
        public int BeneficiaryId { get; set; }
        [JsonIgnore]
        [ValidateNever]        
        public List<Client> ListofBeneficiaryInbound { get; set; }
    }
}
