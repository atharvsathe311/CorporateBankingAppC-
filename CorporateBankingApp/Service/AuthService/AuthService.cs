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
    public class AuthService
    {
        private readonly CorporateBankAppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(CorporateBankAppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        //public bool AddUser(User user)
        //{
        //    try
        //    {
        //        _context.Users.Add(user);
        //        _context.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        public string Login(LoginRequests loginRequests)
        {
            try
            {
                if (loginRequests.Username != null && loginRequests.Password != null)
                {
                    var user = _context.UserLogins.Include("UserType").FirstOrDefault(s => s.LoginUserName == loginRequests.Username && s.PasswordHash == loginRequests.Password);
                    if (user != null)
                    {
                        Console.WriteLine(user);
                        var Claims = new List<Claim>
                        {
                            new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                            new Claim("UserId",user.Id.ToString()),
                            new Claim("UserName",user.LoginUserName),
                            new Claim("UserType", user.UserType.ToString()),
                        };

                        //var roles = user.UserType;
                        //foreach (var role in roles)
                        //{
                        //    Claims.Add(new Claim(ClaimTypes.Role, role.Name));
                        //}

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            Claims,
                            expires: DateTime.UtcNow.AddMinutes(10),
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

        //public bool AddRole(Role role)
        //{
        //    try
        //    {
        //        _context.Roles.Add(role);
        //        _context.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //public List<Role> GetRolesById(IEnumerable<int> roleIds)
        //{
        //    if (roleIds == null)
        //        return new List<Role>();

        //    var roles = _context.Roles.Where(r => roleIds.Contains(r.Id)).ToList();
        //    return roles;
        //}
    }
}
