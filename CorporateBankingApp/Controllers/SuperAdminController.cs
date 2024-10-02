
using Microsoft.AspNetCore.Mvc;
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

        // Define your endpoints here
    }
}