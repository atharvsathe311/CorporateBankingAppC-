using CorporateBankingApp.Data;
using CorporateBankingApp.Models;
using CorporateBankingApp.Models.AuthModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CorporateBankingApp.Service.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly CorporateBankAppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(CorporateBankAppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string Login(LoginRequests loginRequests)
        {
            try
            {
                if (string.IsNullOrEmpty(loginRequests.Username) || string.IsNullOrEmpty(loginRequests.Password))
                {
                    throw new Exception("Empty credentials");
                }

                var user = _context.UserLogins.FirstOrDefault(s => s.LoginUserName == loginRequests.Username);
                if (user == null)
                {
                    throw new Exception("Invalid credentials");
                }

                if (!BCrypt.Net.BCrypt.EnhancedVerify(loginRequests.Password, user.PasswordHash))
                {
                    throw new Exception("Invalid credentials");
                }

                // Initialize claims for JWT
                var claims = new List<Claim>
                {
                    new Claim("UserName", user.LoginUserName),
                };

                // Role-specific checks
                if (user.UserType == Models.UserType.Client)
                {
                    var client = _context.Clients.FirstOrDefault(s => s.UserLogin.Id == user.Id);
                    if (client != null)
                    {
                        claims.Add(new Claim("UserId", client.ClientId.ToString()));
                        claims.Add(new Claim("UserType", "Client"));
                        claims.Add(new Claim("UserStatus", client.Status.ToString()));
                    }
                }
                else if (user.UserType == Models.UserType.Bank)
                {
                    var bank = _context.Banks.FirstOrDefault(s => s.UserLogin.Id == user.Id);
                    if (bank != null)
                    {
                        claims.Add(new Claim("UserId", bank.BankId.ToString()));
                        claims.Add(new Claim("UserType", "Bank"));
                        claims.Add(new Claim("UserStatus", bank.Status.ToString()));
                    }
                }
                else
                {
                    var admin = _context.SuperAdmins.FirstOrDefault(s => s.UserLogin.Id == user.Id);
                    if (admin != null)
                    {
                        claims.Add(new Claim("UserId", admin.AdminId.ToString()));
                        claims.Add(new Claim("UserType", "SuperAdmin"));
                    }
                }

                // Generate JWT token
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: signIn
                );

                // Return the token
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                return "Login failed: " + ex.Message;
            }
        }
    }
}
