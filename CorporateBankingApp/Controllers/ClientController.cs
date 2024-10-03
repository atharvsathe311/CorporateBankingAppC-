using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CorporateBankingApp.Models;
using CorporateBankingApp.Services;
using CorporateBankingApp.DTO;
using CorporateBankingApp.Service;
using AutoMapper;

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
        public ClientController(IClientService clientService, IEmailService emailService ,IMapper mapper ,IBankService bankService)
        {
            _clientService = clientService;
            this._emailService = emailService;
            _mapper = mapper;
            _bankService = bankService; 

        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Client>>> GetAllClients()
        {
            var clients = await _clientService.GetAllClientsAsync();
            return Ok(clients);
        }

        [HttpGet("SendEmail")]
        public void SendEmail()
        {
            _emailService.SendEmail("atharvsathe0302@gmail.com", "Test Subject", "This is a test email sent using Gmail");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClientById(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null) return NotFound();
            return Ok(client);
        }

        [HttpPost]
        public async Task<ActionResult> CreateClient([FromForm] NewClientDTO clientDTO)
        {
            var client = _mapper.Map<Client>(clientDTO);

            UserLogin userLogin = new UserLogin();
            userLogin.LoginUserName = clientDTO.CompanyName.Substring(0, 4) + client.ClientId;
            userLogin.PasswordHash = "Admin@123";
            userLogin.UserType = UserType.Client;
            
            client.UserLogin = userLogin;
            client.CreatedAt = DateTime.Now;
            client.Status = StatusEnum.Submitted;
            client.isActive = true;

            await _clientService.CreateClientAsync(client);

            string body = client.UserLogin.LoginUserName + client.UserLogin.PasswordHash + client.ClientId;
            _emailService.SendEmail("atharvsathe0302@gmail.com", "New Registration",body);

            return CreatedAtAction(nameof(GetClientById), new { id = client.ClientId }, client);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateClient(int id, Client client)
        {
            if (id != client.ClientId) return BadRequest();
            await _clientService.UpdateClientAsync(client);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteClient(int id)
        {
            await _clientService.DeleteClientAsync(id);
            return NoContent();
        }

    }
}
