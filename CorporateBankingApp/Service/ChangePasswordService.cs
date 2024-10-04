using BCrypt.Net;
using CorporateBankingApp.Data;
using CorporateBankingApp.DTO;

namespace CorporateBankingApp.Service
{
    public class ChangePasswordService : IChangePasswordService
    {
        private readonly CorporateBankAppDbContext _dbContext;

        public ChangePasswordService(CorporateBankAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string userType, ChangePasswordDTO changePasswordDTO)
        {
            var userLogin = await _dbContext.UserLogins.FindAsync(userId);

            if (userLogin == null || !VerifyPassword(changePasswordDTO.OldPassword, userLogin.PasswordHash))
            {
                return false;
            }

            if (changePasswordDTO.NewPassword != changePasswordDTO.ConfirmPassword)
            {
                return false; // New passwords do not match
            }

            userLogin.PasswordHash = HashPassword(changePasswordDTO.NewPassword); // Implement HashPassword method
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(enteredPassword, storedHash);
        }

        private string HashPassword(string password)
        {
            return  BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }
    }
}
