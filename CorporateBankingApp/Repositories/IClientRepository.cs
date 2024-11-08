using System.Collections.Generic;
using System.Threading.Tasks;
using CorporateBankingApp.Models;

namespace CorporateBankingApp.Repositories
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAllAsync();
        Task<IEnumerable<Client>> GetAllSubmittedAsync();
        Task<Client> GetByIdAsync(int id);
        Task AddAsync(Client client);
        Task UpdateAsync(Client client);
        Task DeleteAsync(int id);
        Task<BankAccount> GetClientBankAccount(int id);
        Task AddTransaction(Transaction transaction);
        Task<int> IncrementCounterAsync();
    }
}
