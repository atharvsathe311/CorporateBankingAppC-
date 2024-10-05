using CorporateBankingApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CorporateBankingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyProfileController : ControllerBase
    {
        private readonly CorporateBankAppDbContext _context;
        //private readonly H

        public MyProfileController(CorporateBankAppDbContext context)
        {
            _context = context;
        }

        // GET: api/<MyProfileController>
        [HttpGet("My-Profile")]
        [Authorize]
        public IActionResult GetProfile()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            var userTypeIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserType");

            if (userIdClaim == null || userTypeIdClaim == null)
            {
                return BadRequest("User ID or User Type not found in token.");
            }

            var userId = userIdClaim.Value;
            var userTypeId = userTypeIdClaim.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID not found in token.");
            }

            switch (userTypeId)
            {
                case "SuperAdmin": // SuperAdmin
                    var superAdmin = _context.SuperAdmins.Include(c=>c.UserLogin)
                        .FirstOrDefault(a => a.AdminId.ToString() == userId);
                    if (superAdmin == null) return NotFound();
                    return Ok(superAdmin);

                case "Bank": // Bank
                    var bank = _context.Banks.Include(c=>c.UserLogin)
                        .FirstOrDefault(b => b.BankId == int.Parse(userId));
                    if (bank == null) return NotFound();
                    return Ok(bank);

                case "Client": // Client
                    var client = _context.Clients.Include(c => c.UserLogin).Include("BankAccount")
                        .FirstOrDefault(c => c.ClientId == int.Parse(userId));
                    if (client == null) return NotFound();
                    return Ok(client);

                default:
                    return BadRequest("Invalid user type.");
            }
        }

    }
}
