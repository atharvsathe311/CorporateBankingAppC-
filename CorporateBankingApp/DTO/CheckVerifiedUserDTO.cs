using System.ComponentModel.DataAnnotations;

namespace CorporateBankingApp.DTO
{
    public class CheckVerifiedUserDTO
    {
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
