using System.Collections.Generic;
using System.Threading.Tasks;
using CorporateBankingApp.Models;

namespace CorporateBankingApp.Repositories
{
    public interface IBankRepository
    {
        Task<IEnumerable<Bank>> GetAllAsync();
        Task<Bank> GetByIdAsync(int id);
        Task AddAsync(Bank bank);
        Task UpdateAsync(Bank bank);
        Task DeleteAsync(int id);
    }
}
