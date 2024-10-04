using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CorporateBankingApp.Models;
using CorporateBankingApp.Services;
using CorporateBankingApp.DTO;
using CorporateBankingApp.Service;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using CorporateBankingApp.Data;
using System;

namespace CorporateBankingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IBankService _bankService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CorporateBankAppDbContext _dbContext;

        public ClientController(IClientService clientService, IEmailService emailService, IMapper mapper, IBankService bankService, IHttpContextAccessor httpContextAccessor, CorporateBankAppDbContext corporateBankAppDbContext)
        {
            _clientService = clientService;
            _emailService = emailService;
            _mapper = mapper;
            _bankService = bankService;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = corporateBankAppDbContext;

        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Client>>> GetAllClients()
        {
            var clients = await _clientService.GetAllClientsAsync();
            return Ok(clients);
        }

        [HttpGet("GetAllSubmitted")]
        public async Task<ActionResult<IEnumerable<ViewSubmittedClientDTO>>> GetAllSubmittedClients()
        {
            var clients = await _clientService.GetAllClientsAsync();
            var clientsToReturn = new List<ViewSubmittedClientDTO>();
            foreach (var client in clients)
            {
                var submittedClient = _mapper.Map<ViewSubmittedClientDTO>(client);
                clientsToReturn.Add(submittedClient);
            }
            return Ok(clientsToReturn);
        }

        //[HttpGet("SendEmail")]
        //public void SendEmail()
        //{
        //    _emailService.SendEmail("atharvsathe0302@gmail.com", "Test Subject", "This is a test email sent using Gmail");
        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<Client>> GetClientById(int id)
        //{
        //    var client = await _clientService.GetClientByIdAsync(id);
        //    if (client == null) return NotFound();
        //    return Ok(client);
        //}

        [HttpPost]
        public async Task<ActionResult> CreateClient([FromForm] NewClientDTO clientDTO)
        {
            var client = _mapper.Map<Client>(clientDTO);

            int count = await _clientService.GetCounter();

            UserLogin userLogin = new UserLogin();
            userLogin.LoginUserName = clientDTO.CompanyName.Substring(0, 4) + client.ClientId;
            userLogin.PasswordHash = "Admin@123";
            userLogin.UserType = UserType.Client;

            client.UserLogin = userLogin;
            client.CreatedAt = DateTime.Now;
            client.Status = StatusEnum.Submitted;
            client.isActive = true;

            await _clientService.CreateClientAsync(client);

            string body = client.UserLogin.LoginUserName + client.UserLogin.PasswordHash + count;
            _emailService.SendEmail(clientDTO.CompanyEmail, "New Registration", body);

            return Ok(client);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateClient(int id, Client client)
        {
            if (id != client.ClientId) return BadRequest();
            await _clientService.UpdateClientAsync(client);
            return NoContent();
        }

        [Authorize]
        [HttpPost("Upload-Kyc-Documents")]
        public async Task<IActionResult> UploadKYCDocuments(IFormFile PowerOfAttorney, IFormFile BankAccess, IFormFile MOU, ClientKYCDTO clientKyc)
        {
            var user = _httpContextAccessor.HttpContext.User;
            var clientIdClaim = user.FindFirst("UserId").Value;

            Client client = await _clientService.GetClientByIdAsync(Int32.Parse(clientIdClaim));

            if (PowerOfAttorney == null || BankAccess == null || MOU == null)
            {
                return BadRequest("One or more files are missing.");
            }

            var uploadResultPowerOfAttorney = _clientService.Upload(PowerOfAttorney);
            var uploadResultBankAccess = _clientService.Upload(BankAccess);
            var uploadResultMOU = _clientService.Upload(MOU);

            if (uploadResultPowerOfAttorney.FileName == "Extension is not valid" || uploadResultBankAccess.FileName == "Extension is not valid" || uploadResultMOU.FileName == "Extension is not valid")
            {
                return BadRequest("One or more files have invalid extensions.");
            }

            if (uploadResultPowerOfAttorney.FileName == "Max size can't exceed 5MB" || uploadResultBankAccess.FileName == "Max size can't exceed 5MB" || uploadResultMOU.FileName == "Max size can't exceed 5MB")
            {
                return BadRequest("One or more files exceeded the size limit.");
            }

            client.ClientKyc.CINNumber = clientKyc.CINNumber;
            client.ClientKyc.PanNumber = clientKyc.PANNumber;
            client.ClientKyc.MOU = uploadResultMOU;
            client.ClientKyc.BankAccess = uploadResultBankAccess;
            client.ClientKyc.PowerOfAttorney = uploadResultPowerOfAttorney;

            client.Status = StatusEnum.InProcess;

            _dbContext.SaveChanges();
            return Ok(new { message = "KYC documents uploaded successfully." });

        }

        [HttpPost("AcceptClient/{id)}")]
        public async Task<ActionResult> OnboardClient(int id)
        {
            Client client = await _clientService.GetClientByIdAsync(id);

            Bank bank = await _bankService.GetBankByIdAsync(1);
            BankAccount bankAccount = new BankAccount() { Balance = 50000000, BlockedFunds = 0, CreatedAt = DateTime.Now };
            bank.BankAccounts.Add(bankAccount);
            client.BankAccount = bankAccount;
            client.Status = StatusEnum.Approved;
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpPost("RejectClient/{id)}")]
        public async Task<ActionResult> RejectClient(int id)
        {
            Client client = await _clientService.GetClientByIdAsync(id);
            client.Status = StatusEnum.Rejected;
            client.isActive = false;
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpPost("NewBeneficiaryOutboundClient")]
        public async Task<ActionResult> NewBeneficiaryOutboundClient([FromForm] OutboundBeneficiaryDTO clientDTO)
        {
            Client client = new Client();

            client.CompanyName = clientDTO.FullName;
            client.CreatedAt = DateTime.Now;
            client.isActive = true;

            Bank bank = await _bankService.GetBankByIdAsync(1);
           
            BankAccount bankAccount = new BankAccount() { Balance = 50000000, BlockedFunds = 0, CreatedAt = DateTime.Now };

            if (bank.BankAccounts != null)
            {
                bank.BankAccounts.Add(bankAccount);
                client.BankAccount = bankAccount;
                client.isBeneficiaryOutbound = true;
                await _clientService.CreateClientAsync(client);
                return Ok(client);
            }
            bank.BankAccounts = new List<BankAccount> { bankAccount };
            client.BankAccount = bankAccount;
            client.isBeneficiaryOutbound = true;
            await _clientService.CreateClientAsync(client);
            return Ok(client);
        }

        [HttpPost("AddBeneficiary")]
        public async Task<ActionResult> AddBeneficiary(int ids)
        {
            var user = _httpContextAccessor.HttpContext.User;
            var clientIdClaim = int.Parse(user.FindFirst("UserId").Value);

            //int clientIdClaim = 1;

            Client client = await _clientService.GetClientByIdAsync(clientIdClaim);

            if (client == null)
            {
                return BadRequest("Null");
            }

            if (client.BeneficiaryLists != null)
            {
                client.BeneficiaryLists.Add(ids);
                _dbContext.SaveChanges();
                return Ok("Saved");
            }

            client.BeneficiaryLists = new List<int> { ids };
            _dbContext.SaveChanges();
            
            return Ok("Saved");
        }

    }
}
