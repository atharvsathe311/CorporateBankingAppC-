using AutoMapper;
using CorporateBankingApp.Data;
using CorporateBankingApp.DTO;
using CorporateBankingApp.Models;
using CorporateBankingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CorporateBankingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllGetControllers : ControllerBase
    {
        private readonly CorporateBankAppDbContext _context;
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;
        public AllGetControllers(CorporateBankAppDbContext corporateBankAppDbContext,IClientService clientService,IMapper mapper) 
        {
            _context = corporateBankAppDbContext;
            _clientService = clientService;
            _mapper = mapper;
        }
        [HttpGet("GetAllTransactions/{id}")]
        public IEnumerable<Transaction> GetAllTransactions(int id)
        {
            return _context.Transactions.Where(s=> s.SenderId == id).ToList();
        }

        [HttpGet("GetAllBeneficiaries/{id}")]
        public IEnumerable<GetAllBeneficiary> GetAllBeneficiaries(int id)
        {
            List<GetAllBeneficiary> data = new List<GetAllBeneficiary>();
           Client client = _context.Clients.FirstOrDefault(c=>c.ClientId == id);

            Console.WriteLine(client.ClientId);

            if (client == null) { Console.WriteLine("Hello"); }


           //foreach(int cli in client.BeneficiaryLists)
           // {
           //     Client tempClient = _context.Clients.FirstOrDefault(s=>s.ClientId == cli);
           //     GetAllBeneficiary tempData = new GetAllBeneficiary()
           //     {
           //         Name = tempClient.CompanyName,
           //         BankAccountNumber = tempClient.BankAccount.AccountId.ToString(),
           //         BankIFSC = tempClient.BankAccount.BankId.ToString(),
           //         IsOutbound = tempClient.isBeneficiaryOutbound
           //     };
           //     data.Add(tempData);
           // }
            return data;
        }

        [HttpGet("GetBankSubmitted")]
        public IEnumerable<Bank> GetBankSubmitted()
        {
            return _context.Banks.Where(s=>s.Status == StatusEnum.InProcess).ToList();
        }

        [HttpGet("GetBankOnboarded")]
        public IEnumerable<Bank> GetBankOnboarded()
        {
            return _context.Banks.Where(s => s.Status == StatusEnum.Approved).ToList();
        }

        [HttpGet("GetBankRejected")]
        public IEnumerable<Bank> GetBankRejected()
        {
            return _context.Banks.Where(s => s.Status == StatusEnum.Rejected).ToList();
        }


        [HttpGet("GetTransaction")]
        public IEnumerable<Transaction> GetTransaction()
        {
            return _context.Transactions.Where(s => s.Status == StatusEnum.Submitted).ToList();
        }

        [HttpGet("GetTransactionApproved")]
        public IEnumerable<Transaction> GetTransactionApproved()
        {
            return _context.Transactions.Where(s => s.Status == StatusEnum.Approved).ToList();
        }

        [HttpGet("GetTransactionRejected")]
        public IEnumerable<Transaction> GetTransactionRejected()
        {
            return _context.Transactions.Where(s => s.Status == StatusEnum.Rejected).ToList();
        }

        [HttpGet("GetAllApprovedClient")]
        public async Task<ActionResult<IEnumerable<ViewSubmittedClientDTO>>> GetAllApprovedClient()
        {
            var clients = _context.Clients.Where(s=> s.Status == StatusEnum.Approved).ToList();
            var clientsToReturn = new List<ViewSubmittedClientDTO>();
            foreach (var client in clients)
            {
                Console.WriteLine(client);
                var submittedClient = _mapper.Map<ViewSubmittedClientDTO>(client);
                clientsToReturn.Add(submittedClient);
            }
            return Ok(clientsToReturn);
        }

        [HttpGet("GetAllSubmitted")]
        public async Task<ActionResult<IEnumerable<ViewSubmittedClientDTO>>> GetAllSubmittedClients()
        {
            var clients = _context.Clients.Where(s => s.Status == StatusEnum.Submitted || s.Status == StatusEnum.InProcess).ToList();
            var clientsToReturn = new List<ViewSubmittedClientDTO>();
            foreach (var client in clients)
            {
                var submittedClient = _mapper.Map<ViewSubmittedClientDTO>(client);
                clientsToReturn.Add(submittedClient);
            }
            return Ok(clientsToReturn);
        }

        [HttpGet("GetTransactionBank/{id}")]
        public IEnumerable<Transaction> GetTransactionBank(int id)
        {
            return _context.Transactions.Where(s => s.Status == StatusEnum.Submitted && ( s.SenderBankId == id || s.ReceiverBankId == id ) ).ToList();
        }

        [HttpGet("GetTransactionApprovedBank/{id}")]
        public IEnumerable<Transaction> GetTransactionApprovedBank(int id)
        {
            return _context.Transactions.Where(s => s.Status == StatusEnum.Approved && (s.SenderBankId == id || s.ReceiverBankId == id)).ToList();
        }

        [HttpGet("GetTransactionRejectedBank/{id}")]
        public IEnumerable<Transaction> GetTransactionRejectedBank(int id)
        {
            return _context.Transactions.Where(s => s.Status == StatusEnum.Rejected && (s.SenderBankId == id || s.ReceiverBankId == id)).ToList();
        }




    }


}
