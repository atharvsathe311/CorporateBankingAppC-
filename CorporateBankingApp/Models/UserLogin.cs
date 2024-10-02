using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateBankingApp.Models
{
    public class UserLogin
    {
        public int Id { get; set; }
        public string LoginUserName { get; set; }
        public string PasswordHash { get; set; }
        public UserType UserType { get; set; }
    }
}
