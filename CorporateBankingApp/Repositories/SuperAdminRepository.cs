using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorporateBankingApp.Data;
using CorporateBankingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CorporateBankingApp.Repositories
{
    public class SuperAdminRepository : ISuperAdminRepository
    {
        private readonly CorporateBankAppDbContext _context;

        public SuperAdminRepository(CorporateBankAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SuperAdmin>> GetAllAsync()
        {
            return await _context.SuperAdmins.ToListAsync();
        }

        public async Task<SuperAdmin> GetByIdAsync(int id)
        {
            return await _context.SuperAdmins.FindAsync(id);
        }

        public async Task AddAsync(SuperAdmin superAdmin)
        {
            superAdmin.UserLogin.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(superAdmin.UserLogin.PasswordHash);
            await _context.SuperAdmins.AddAsync(superAdmin);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SuperAdmin superAdmin)
        {
            _context.SuperAdmins.Update(superAdmin);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var superAdmin = await GetByIdAsync(id);
            if (superAdmin != null)
            {
                _context.SuperAdmins.Remove(superAdmin);
                await _context.SaveChangesAsync();
            }
        }
    }
}
