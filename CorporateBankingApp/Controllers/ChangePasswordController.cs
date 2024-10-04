using CorporateBankingApp.DTO;
using CorporateBankingApp.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CorporateBankingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangePasswordController : ControllerBase
    {
        private readonly IChangePasswordService _userService; // Assume you have a service to handle user operations

        public ChangePasswordController(IChangePasswordService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            var userTypeClaim = User.FindFirst("UserType")?.Value;

            if (userIdClaim == null || userTypeClaim == null)
            {
                return Unauthorized(new { message = "Invalid token." });
            }

            var result = await _userService.ChangePasswordAsync(int.Parse(userIdClaim), userTypeClaim, changePasswordDTO);
            if (result)
            {
                return Ok(new { message = "Password changed successfully." });
            }
            return BadRequest(new { message = "Failed to change password." });
        }
    }
}
