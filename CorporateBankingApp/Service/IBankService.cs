using System.Collections.Generic;
using System.Threading.Tasks;
using CorporateBankingApp.Models;

namespace CorporateBankingApp.Services
{
    public interface IBankService
    {
        Task<IEnumerable<Bank>> GetAllBanksAsync();
        Task<Bank> GetBankByIdAsync(int id);
        Task CreateBankAsync(Bank bank);
        Task UpdateBankAsync(Bank bank);
        Task DeleteBankAsync(int id);
        string ParseString(string input);
    }
}
