using CorporateBankingApp.DTO;

namespace CorporateBankingApp.Service
{
    public interface IChangePasswordService
    {
        Task<bool> ChangePasswordAsync(int userId, string userType, ChangePasswordDTO changePasswordDTO);
    }
}
