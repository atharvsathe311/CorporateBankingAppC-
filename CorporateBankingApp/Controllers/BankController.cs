using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CorporateBankingApp.Models;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bank>>> GetAllBanks()
        {
            var banks = await _bankService.GetAllBanksAsync();
            return Ok(banks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Bank>> GetBankById(int id)
        {
            var bank = await _bankService.GetBankByIdAsync(id);
            if (bank == null) return NotFound();
            return Ok(bank);
        }

        [HttpPost]
        public async Task<ActionResult> CreateBank(Bank bank)
        {
            await _bankService.CreateBankAsync(bank);
            return CreatedAtAction(nameof(GetBankById), new { id = bank.BankId }, bank);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBank(int id, Bank bank)
        {
            if (id != bank.BankId) return BadRequest();
            await _bankService.UpdateBankAsync(bank);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBank(int id)
        {
            await _bankService.DeleteBankAsync(id);
            return NoContent();
        }
    }
}
