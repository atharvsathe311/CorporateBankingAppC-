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
        private readonly AuthService _authService;

        public UserLoginController(AuthService authService)
        {
            _authService = authService;
        }

        // GET: api/<UserLoginController>
        //[HttpGet]
        //public string Get()
        //{

        //    //return ;
        //}

        // GET api/<UserLoginController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //POST api/<UserLoginController>
        [HttpPost]
        public string Post([FromBody] LoginRequests loginRequest)
        {
            return _authService.Login(loginRequest);
        }

        // PUT api/<UserLoginController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<UserLoginController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
