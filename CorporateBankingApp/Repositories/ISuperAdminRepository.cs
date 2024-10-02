using System.Collections.Generic;
using System.Threading.Tasks;
using CorporateBankingApp.Models;

namespace CorporateBankingApp.Repositories
{
    public interface ISuperAdminRepository
    {
        Task<IEnumerable<SuperAdmin>> GetAllAsync();
        Task<SuperAdmin> GetByIdAsync(int id);
        Task AddAsync(SuperAdmin superAdmin);
        Task UpdateAsync(SuperAdmin superAdmin);
        Task DeleteAsync(int id);
    }
}
