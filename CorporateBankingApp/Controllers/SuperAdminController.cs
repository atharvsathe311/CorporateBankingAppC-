using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CorporateBankingApp.Models;
using CorporateBankingApp.Services;

namespace CorporateBankingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuperAdminController : ControllerBase
    {
        private readonly ISuperAdminService _superAdminService;

        public SuperAdminController(ISuperAdminService superAdminService)
        {
            _superAdminService = superAdminService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SuperAdmin>>> GetAllSuperAdmins()
        {
            var superAdmins = await _superAdminService.GetAllSuperAdminsAsync();
            return Ok(superAdmins);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperAdmin>> GetSuperAdminById(int id)
        {
            var superAdmin = await _superAdminService.GetSuperAdminByIdAsync(id);
            if (superAdmin == null) return NotFound();
            return Ok(superAdmin);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSuperAdmin(SuperAdmin superAdmin)
        {
            await _superAdminService.CreateSuperAdminAsync(superAdmin);
            return CreatedAtAction(nameof(GetSuperAdminById), new { id = superAdmin.AdminId }, superAdmin);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSuperAdmin(int id, SuperAdmin superAdmin)
        {
            if (id != superAdmin.AdminId) return BadRequest();
            await _superAdminService.UpdateSuperAdminAsync(superAdmin);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSuperAdmin(int id)
        {
            await _superAdminService.DeleteSuperAdminAsync(id);
            return NoContent();
        }
    }
}
