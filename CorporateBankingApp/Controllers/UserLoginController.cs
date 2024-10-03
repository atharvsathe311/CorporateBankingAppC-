using CorporateBankingApp.Models.AuthModels;
using CorporateBankingApp.Service.AuthService;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CorporateBankingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserLoginController(IAuthService authService)
        {
            _authService = authService;
        }

        //POST api/<UserLoginController>
        [HttpPost]
        public string Post([FromBody] LoginRequests loginRequest)
        {
            return _authService.Login(loginRequest);
        }

    }
}
