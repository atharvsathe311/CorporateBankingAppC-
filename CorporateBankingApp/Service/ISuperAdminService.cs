using System.Collections.Generic;
using System.Threading.Tasks;
using CorporateBankingApp.Models;

namespace CorporateBankingApp.Services
{
    public interface ISuperAdminService
    {
        Task<IEnumerable<SuperAdmin>> GetAllSuperAdminsAsync();
        Task<SuperAdmin> GetSuperAdminByIdAsync(int id);
        Task CreateSuperAdminAsync(SuperAdmin superAdmin);
        Task UpdateSuperAdminAsync(SuperAdmin superAdmin);
        Task DeleteSuperAdminAsync(int id);
    }
}
