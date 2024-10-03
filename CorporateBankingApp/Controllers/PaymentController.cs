using CorporateBankingApp.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CorporateBankingApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly CorporateBankAppDbContext _corporateBankAppDbContext;

        public PaymentController(CorporateBankAppDbContext corporateBankAppDbContext)
        {
            _corporateBankAppDbContext = corporateBankAppDbContext;
        }

        //[HttpPost("SinglePayment")]
        //public void Post([FromBody] )
        //{
            
        //}

        // PUT api/<PaymentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PaymentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
