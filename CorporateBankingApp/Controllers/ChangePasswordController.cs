using CorporateBankingApp.Data;
using CorporateBankingApp.DTO;
using CorporateBankingApp.Models;
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
        private readonly CorporateBankAppDbContext _dbContext;

        public ChangePasswordController(IChangePasswordService userService, CorporateBankAppDbContext dbContext)
        {
            _userService = userService;
            _dbContext = dbContext;
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

        [HttpPost("CheckVerifiedUser")]
        public async Task<ActionResult> CheckVerifiedUser([FromBody] CheckVerifiedUserDTO checkVerifiedUserDTO)
        {
            var user = _dbContext.UserLogins.Where(u => u.LoginUserName == checkVerifiedUserDTO.Username).FirstOrDefault();

            if (user == null)
            {
                return BadRequest(new { message = "User not registered" });
            }

            if(user.UserType == UserType.Client)
            {
                var client = _dbContext.Clients.Where(c => c.CompanyEmail == checkVerifiedUserDTO.Email && c.UserLogin.Id == user.Id).FirstOrDefault();
                return Ok(new { message = "Client Verified" , value = user});
            }

            if (user.UserType == UserType.Bank)
            {
                var client = _dbContext.Banks.Where(c => c.BankEmail == checkVerifiedUserDTO.Email && c.UserLogin.Id == user.Id).FirstOrDefault();
                return Ok(new { message = "Bank Verified" , value = user });
            }

            if (user.UserType == UserType.SuperAdmin)
            {
                return BadRequest(new { message = "Admin's Password Cannot be reset" });
            }

            return BadRequest(new { message = "Email Not Verified" });
        }


        [HttpPost("ResetPasswordVerifiedUser")]
        public async Task<ActionResult> ResetPasswordVerifiedUser([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            var user = _dbContext.UserLogins.Where(u => u.Id == resetPasswordDTO.UserId).FirstOrDefault();

            if (user == null)
            {
                return BadRequest(new { message = "User not registered" });
            }

            if(user != null)
            {
                user.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(resetPasswordDTO.Password);
                _dbContext.SaveChanges();
                return Ok(new { message = "Password Reset Successfully" });
            }

            return BadRequest(new { message = "Password Reset Failed" });
        }
    }
}
