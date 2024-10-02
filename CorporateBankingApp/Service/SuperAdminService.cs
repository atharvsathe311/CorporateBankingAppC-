using System.Collections.Generic;
using System.Threading.Tasks;
using CorporateBankingApp.GlobalException;
using CorporateBankingApp.Models;
using CorporateBankingApp.Repositories;

namespace CorporateBankingApp.Services
{
    public class SuperAdminService : ISuperAdminService
    {
        private readonly ISuperAdminRepository _superAdminRepository;

        public SuperAdminService(ISuperAdminRepository superAdminRepository)
        {
            _superAdminRepository = superAdminRepository;
        }

        public async Task<IEnumerable<SuperAdmin>> GetAllSuperAdminsAsync()
        {
            return await _superAdminRepository.GetAllAsync();
        }

        public async Task<SuperAdmin> GetSuperAdminByIdAsync(int id)
        {
            var superAdmin = await _superAdminRepository.GetByIdAsync(id);
            if (superAdmin == null)
            {
                throw new CustomException("SuperAdmin not found");
            }
            return superAdmin;
        }

        public async Task CreateSuperAdminAsync(SuperAdmin superAdmin)
        {
            await _superAdminRepository.AddAsync(superAdmin);
        }

        public async Task UpdateSuperAdminAsync(SuperAdmin superAdmin)
        {
            await _superAdminRepository.UpdateAsync(superAdmin);
        }

        public async Task DeleteSuperAdminAsync(int id)
        {
            await _superAdminRepository.DeleteAsync(id);
        }
    }
}
