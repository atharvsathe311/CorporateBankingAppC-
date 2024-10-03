using System.ComponentModel.DataAnnotations;

namespace CorporateBankingApp.Models
{
    public class SuperAdmin
    {
        [Key]
        public int AdminId { get; set; }
        public string Name { get; set; }
        public UserLogin UserLogin { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool isActive { get; set; }

    }
}
