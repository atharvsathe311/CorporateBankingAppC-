
using Microsoft.AspNetCore.Mvc;
using CorporateBankingApp.Services;

namespace CorporateBankingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;

        public BankController(IBankService bankService)
        {
            _bankService = bankService;
        }

        // Define your endpoints here
    }
}