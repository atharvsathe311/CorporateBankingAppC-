using CorporateBankingApp.Data;
using CorporateBankingApp.DTO;
using CorporateBankingApp.GlobalException;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CorporateBankingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FundAllocationController : ControllerBase
    {

        private readonly CorporateBankAppDbContext _context;

        public FundAllocationController(CorporateBankAppDbContext context)
        {
            _context = context;
        }

        [HttpPost("AllocateFundsToClientAccount")]
        public IActionResult AllocateFundsToClientAccount([FromBody] FundAllocationRequest request)
        {
            if (request.Id == 0)
            {
                throw new CustomException("Client not found");
            }

            var client = _context.Clients.Include(c => c.BankAccount).FirstOrDefault(c => c.ClientId == request.Id);

            if (client?.BankAccount == null)
            {
                throw new CustomException("Client does not have a Bank Account");
            }

            var account = client.BankAccount;

            decimal totalPercentage = request.InvestmentsAndGrowth + request.EmergencyFunds + request.AccountPayables + request.Taxes + request.SalaryPayments;

            if (totalPercentage == 100)
            {
                account.UpdateSplitPercentages(request.SalaryPayments, request.AccountPayables, request.Taxes, request.EmergencyFunds, request.InvestmentsAndGrowth);
                _context.SaveChanges();
                return Ok(new { message = "Account percentages updated successfully.", client, account});
            }
            else
            {
                return BadRequest(new { message = "Total percentage must equal 100." });
            }
        }


    }
}
