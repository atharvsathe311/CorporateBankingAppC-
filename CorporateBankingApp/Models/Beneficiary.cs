using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CorporateBankingApp.Models
{
    public class BeneficiaryList
    {
        [Key]
        public int BeneficiaryId { get; set; }
        [JsonIgnore]
        [ValidateNever]        
        public List<Client> ListofBeneficiaryInbound { get; set; }
    }
}
