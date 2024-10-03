using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorporateBankingApp.Data;
using CorporateBankingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CorporateBankingApp.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly CorporateBankAppDbContext _context;

        public ClientRepository(CorporateBankAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<IEnumerable<Client>> GetAllSubmittedAsync()
        {
            return await _context.Clients.Where(c => c.Status == StatusEnum.Submitted && c.isActive == true).ToListAsync();
        }

        public async Task<Client> GetByIdAsync(int id)
        {
            return await _context.Clients
       .Include(c => c.UserLogin)
       .Include(c => c.ClientKyc)
       .Include(c => c.BankAccount)
       .Include(c => c.BeneficiaryLists)
       .FirstOrDefaultAsync(c => c.ClientId == id);

        }

        public async Task AddAsync(Client client)
        {
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Client client)
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var client = await GetByIdAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<BankAccount> GetClientBankAccount(int id)
        {
            Client client = await _context.Clients.Include(c => c.BankAccount).FirstOrDefaultAsync(c => c.ClientId == id);
            return client.BankAccount;
        }

        public async Task AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
        }


        public async Task<int> IncrementCounterAsync()
        {
            var counter = await _context.ClientCounter.FirstOrDefaultAsync();

            if (counter == null)
            {
                // Initialize counter if not present
                counter = new Counter { CounterValue = 1001 };
                _context.ClientCounter.Add(counter);
            }
            else
            {
                // Increment existing counter
                counter.CounterValue += 1;
                _context.ClientCounter.Update(counter);
            }

            await _context.SaveChangesAsync();
            return counter.CounterValue;
        }
    }
}
