using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorporateBankingApp.Data;
using CorporateBankingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CorporateBankingApp.Repositories
{
    public class BankRepository : IBankRepository
    {
        private readonly CorporateBankAppDbContext _context;

        public BankRepository(CorporateBankAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bank>> GetAllAsync()
        {
            return await _context.Banks.ToListAsync();
        }

        public async Task<Bank> GetByIdAsync(int id)
        {
            return await _context.Banks.FindAsync(id);
        }

        public async Task AddAsync(Bank bank)
        {
            await _context.Banks.AddAsync(bank);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Bank bank)
        {
            _context.Banks.Update(bank);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var bank = await GetByIdAsync(id);
            if (bank != null)
            {
                _context.Banks.Remove(bank);
                await _context.SaveChangesAsync();
            }
        }
    }
}
