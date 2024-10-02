using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateBankingApp.Models
{
    public class ClientKyc
    {
        public int ClientKycId { get; set; }
        public string CINNumber { get; set; }
        public string PanNumber { get; set; }
        public FileDetail PowerOfAttorney { get; set; }
        public FileDetail BankAccess { get; set; }
        public FileDetail MOU { get; set; }
 
    }
}
