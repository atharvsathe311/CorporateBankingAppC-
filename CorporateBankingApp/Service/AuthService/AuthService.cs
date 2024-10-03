using CorporateBankingApp.Data;
using CorporateBankingApp.Models.AuthModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
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
                if (loginRequests.Username != null && loginRequests.Password != null)
                {
                    var user = _context.UserLogins.FirstOrDefault(s => s.LoginUserName == loginRequests.Username && s.PasswordHash == loginRequests.Password);
                    if (user != null)
                    {
                        Console.WriteLine(user);
                        var client = _context.Clients.FirstOrDefault(s => s.UserLogin.Id == user.Id);
                        var Claims = new List<Claim>
                        {
                            new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                            new Claim("UserId",client.ClientId.ToString()),
                            new Claim("UserName",user.LoginUserName),
                            new Claim("UserType", user.UserType.ToString()),
                            new Claim("UserStatus",client.Status.ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            Claims,
                            expires: DateTime.UtcNow.AddMinutes(30),
                            signingCredentials: signIn);
                        var Token = new JwtSecurityTokenHandler().WriteToken(token);
                        return Token;
                    }
                    throw new Exception("Invalid Credentials");
                }
                throw new Exception("Empty Credentials");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}

