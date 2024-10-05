using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CorporateBankingApp.Models;
using CorporateBankingApp.Services;
using CorporateBankingApp.DTO;
using AutoMapper;
using CorporateBankingApp.Service;

namespace CorporateBankingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;
        private readonly IMapper _mapper;
        private readonly IClientService _clientService;
        private readonly IEmailService _emailService;

        public BankController(IBankService bankService, IMapper mapper, IClientService clientService,IEmailService emailService)
        {
            _bankService = bankService;
            _mapper = mapper;
            _clientService = clientService;
            _emailService = emailService;
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
        public async Task<ActionResult> CreateBank(BankDTO bankDto)
        {
            var bank = _mapper.Map<Bank>(bankDto);

            int count = await _clientService.GetCounter();

            bank.Status = StatusEnum.Submitted;
            UserLogin userLogin = new UserLogin()
            {
                LoginUserName = bankDto.BankName.Substring(0, 4) + count,
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("Admin@123"),
                UserType = UserType.Bank,
            };
            bank.UserLogin = userLogin;
            bank.CreatedAt = DateTime.Now;
            bank.isActive = true;

            await _bankService.CreateBankAsync(bank);
            _emailService.SendNewRegistrationMail(bankDto.BankEmail, bankDto.BankName, userLogin.LoginUserName, "Admin@123");
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
