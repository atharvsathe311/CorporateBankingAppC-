
using Microsoft.AspNetCore.Mvc;
using CorporateBankingApp.Services;

namespace CorporateBankingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        // Define your endpoints here
    }
}