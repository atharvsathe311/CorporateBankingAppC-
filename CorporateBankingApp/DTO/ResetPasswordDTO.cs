using System.ComponentModel.DataAnnotations;

namespace CorporateBankingApp.DTO
{
    public class ResetPasswordDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
