
using Microsoft.AspNetCore.Mvc;
using CorporateBankingApp.Services;

namespace CorporateBankingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        // Define your endpoints here
    }
}