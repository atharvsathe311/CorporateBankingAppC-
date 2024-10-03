using CorporateBankingApp.Models.AuthModels;

namespace CorporateBankingApp.Service.AuthService
{
    public interface IAuthService
    {
        string Login(LoginRequests loginRequests);
    }
}
