using AutoMapper;
using CloudinaryDotNet.Core;
using CorporateBankingApp.Data;
using CorporateBankingApp.DTO;
using CorporateBankingApp.Models;
using CorporateBankingApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using System.Collections;

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
        public AllGetControllers(CorporateBankAppDbContext corporateBankAppDbContext, IClientService clientService, IMapper mapper)
        {
            _context = corporateBankAppDbContext;
            _clientService = clientService;
            _mapper = mapper;
        }
        [HttpGet("GetAllTransactions/{id}")]
        public IEnumerable<Transaction> GetAllTransactions(int id)
        {
            return _context.Transactions.Where(s => s.SenderId == id).ToList();
        }

        [HttpGet("GetAllBeneficiaries/{id}")]
        public IEnumerable<GetAllBeneficiary> GetAllBeneficiaries(int id)
        {
            List<GetAllBeneficiary> data = new List<GetAllBeneficiary>();
            Client client = _context.Clients.FirstOrDefault(c => c.ClientId == id);

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

        //[HttpGet("GetBankSubmitted")]
        //public IEnumerable<Bank> GetBankSubmitted()
        //{
        //    return _context.Banks.Where(s => s.Status == StatusEnum.InProcess).ToList();
        //}

        [HttpGet("GetBankSubmitted")]
        public IActionResult GetBankSubmitted(int page = 1, int pageSize = 10, string searchTerm = "")
        {
            IQueryable<Bank> query = _context.Banks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s =>( s.Status == StatusEnum.InProcess || s.Status == StatusEnum.Submitted) &&
                    (s.BankName.Contains(searchTerm) ||
                     s.BankEmail.Contains(searchTerm)));
            }
            else
            {
                query = query.Where(s => s.Status == StatusEnum.InProcess || s.Status == StatusEnum.Submitted);
            }

            var totalCount = query.Count();

            var banks = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new
                {
                    b.BankId,
                    b.BankName,
                    b.BankEmail,
                    b.Status
                })
                .ToList();

            return Ok(new
            {
                Banks = banks,
                TotalCount = totalCount
            });
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


        //[HttpGet("GetTransaction")]
        //public IEnumerable<Transaction> GetTransaction()
        //{
        //    return _context.Transactions.Where(s => s.Status == StatusEnum.Submitted).ToList();
        //}

        [HttpGet("GetTransaction")]
        public IActionResult GetTransaction(int page = 1, int pageSize = 10, string searchTerm = "")
        {
            IQueryable<Transaction> query = _context.Transactions.AsQueryable();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s => s.Status == StatusEnum.Submitted);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s => (s.TransactionId.ToString().Contains(searchTerm) ||
                           s.SenderId.ToString().Contains(searchTerm) ||
                           s.ReceiverId.ToString().Contains(searchTerm) || 
                           s.Amount.ToString().Contains(searchTerm) ||
                           s.Remarks.ToString().Contains(searchTerm)) && 
                           s.Status == StatusEnum.Submitted);

            }

            var totalCount = query.Count();

            var transactions = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new
                {
                    t.TransactionId,
                    t.SenderId,
                    t.ReceiverId,
                    t.DateTime,
                    t.Amount,
                    t.Status,
                    t.Remarks
                })
                .ToList();

            return Ok(new
            {
                Transactions = transactions,
                TotalCount = totalCount
            });
        }

        [HttpGet("GetTransactionApproved")]
        public IActionResult GetTransactionApproved(int page = 1, int pageSize = 10, string searchTerm = "")
        {
            IQueryable<Transaction> query = _context.Transactions.AsQueryable();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s => s.Status == StatusEnum.Approved);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s => (s.TransactionId.ToString().Contains(searchTerm) ||
                            s.SenderId.ToString().Contains(searchTerm) ||
                           s.ReceiverId.ToString().Contains(searchTerm) ||
                           s.Amount.ToString().Contains(searchTerm) ||
                           s.Remarks.ToString().Contains(searchTerm)) && s.Status == StatusEnum.Approved);
            }

            var totalCount = query.Count();

            var transactions = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new
                {
                    t.TransactionId,
                    t.SenderId,
                    t.ReceiverId,
                    t.DateTime,
                    t.Amount,
                    t.Status,
                    t.Remarks
                })
                .ToList();

            return Ok(new
            {
                Transactions = transactions,
                TotalCount = totalCount
            });
        }

        [HttpGet("GetTransactionRejected")]
        public IActionResult GetTransactionRejected(int page = 1, int pageSize = 10, string searchTerm = "")
        {
            IQueryable<Transaction> query = _context.Transactions.AsQueryable();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s => s.Status == StatusEnum.Rejected);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s => (s.TransactionId.ToString().Contains(searchTerm) ||
                            s.SenderId.ToString().Contains(searchTerm) ||
                           s.ReceiverId.ToString().Contains(searchTerm) ||
                           s.Amount.ToString().Contains(searchTerm) ||
                           s.Remarks.ToString().Contains(searchTerm)) && s.Status == StatusEnum.Rejected);
            }

            var totalCount = query.Count();

            var transactions = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new
                {
                    t.TransactionId,
                    t.SenderId,
                    t.ReceiverId,
                    t.DateTime,
                    t.Amount,
                    t.Status,
                    t.Remarks
                })
                .ToList();

            return Ok(new
            {
                Transactions = transactions,
                TotalCount = totalCount
            });
        }

        [HttpGet("GetAllApprovedClient")]
        public async Task<ActionResult<IEnumerable<ViewSubmittedClientDTO>>> GetAllApprovedClient()
        {
            var clients = _context.Clients.Where(s => s.Status == StatusEnum.Approved).ToList();
            var clientsToReturn = new List<ViewSubmittedClientDTO>();
            foreach (var client in clients)
            {
                Console.WriteLine(client);
                var submittedClient = _mapper.Map<ViewSubmittedClientDTO>(client);
                clientsToReturn.Add(submittedClient);
            }
            return Ok(clientsToReturn);
        }

        [HttpGet("GetAllSubmittedClients")]
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

        [HttpGet("GetAllRejectedClient")]
        public async Task<ActionResult<IEnumerable<ViewSubmittedClientDTO>>> GetAllRejectedClient()
        {
            var clients = _context.Clients.Where(s => s.Status == StatusEnum.Rejected).ToList();
            var clientsToReturn = new List<ViewSubmittedClientDTO>();
            foreach (var client in clients)
            {
                Console.WriteLine(client);
                var submittedClient = _mapper.Map<ViewSubmittedClientDTO>(client);
                clientsToReturn.Add(submittedClient);
            }
            return Ok(clientsToReturn);
        }



        [HttpGet("GetTransactionBank/{id}")]
        public IEnumerable<Transaction> GetTransactionBank(int id)
        {
            return _context.Transactions.Where(s => s.Status == StatusEnum.Submitted && (s.SenderBankId == id || s.ReceiverBankId == id)).ToList();
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

        [HttpGet("GetTransactionClient/{id}")]
        public IEnumerable<Transaction> GetTransactionClient(int id)
        {
            return _context.Transactions.Where(s => s.Status == StatusEnum.Submitted && (s.SenderId == id)).ToList();
        }


        [HttpGet("GetTransactionApprovedClient/{id}")]
        public IEnumerable<Transaction> GetTransactionApprovedClient(int id)
        {
            return _context.Transactions.Where(s => s.Status == StatusEnum.Approved && (s.SenderId == id || s.ReceiverId == id)).ToList();
        }

        [HttpGet("GetTransactionRejectedClient/{id}")]
        public IEnumerable<Transaction> GetTransactionRejectedClient(int id)
        {
            return _context.Transactions.Where(s => s.Status == StatusEnum.Rejected && (s.SenderId == id)).ToList();
        }


        [HttpGet("GetAllApprovedClientBank/{id}")]
        public async Task<ActionResult<IEnumerable<ViewSubmittedClientDTO>>> GetAllApprovedClientBank(int id)
        {
            var clients = _context.Clients.Where(s => s.Status == StatusEnum.Approved && s.BankId == id).ToList();
            var clientsToReturn = new List<ViewSubmittedClientDTO>();
            foreach (var client in clients)
            {
                var submittedClient = _mapper.Map<ViewSubmittedClientDTO>(client);
                clientsToReturn.Add(submittedClient);
            }
            return Ok(clientsToReturn);
        }

        [HttpGet("GetAllSubmittedClientBank/{id}")]
        public async Task<ActionResult<IEnumerable<ViewSubmittedClientDTO>>> GetAllSubmittedBank(int id)
        {
            var clients = _context.Clients.Where(s => (s.Status == StatusEnum.Submitted || s.Status == StatusEnum.InProcess) && s.BankId == id).ToList();
            var clientsToReturn = new List<ViewSubmittedClientDTO>();
            foreach (var client in clients)
            {
                var submittedClient = _mapper.Map<ViewSubmittedClientDTO>(client);
                clientsToReturn.Add(submittedClient);
            }
            return Ok(clientsToReturn);
        }

        [HttpGet("GetAllRejectedClientBank/{id}")]
        public async Task<ActionResult<IEnumerable<ViewSubmittedClientDTO>>> GetAllRejectedClientBank(int id)
        {
            var clients = _context.Clients.Where(s => s.Status == StatusEnum.Rejected && s.BankId == id).ToList();
            var clientsToReturn = new List<ViewSubmittedClientDTO>();
            foreach (var client in clients)
            {
                var submittedClient = _mapper.Map<ViewSubmittedClientDTO>(client);
                clientsToReturn.Add(submittedClient);
            }
            return Ok(clientsToReturn);
        }

        // Dashboard Population
        [HttpGet("GetDashBoardBank/{id}")]
        public async Task<ActionResult> GetDashBoardBank(int id)
        {
            IEnumerable<Transaction> transactions = await _context.Transactions.Where(s => s.Status == StatusEnum.Rejected && (s.SenderBankId == id || s.ReceiverBankId == id)).ToListAsync();
            double balance = 0;
            Bank bank = await _context.Banks.Where(s => s.BankId == id).Include("BankAccounts").FirstOrDefaultAsync();

            foreach (BankAccount bankAccount in bank.BankAccounts)
            {
                balance += bankAccount.Balance;
            }
            var data = new
            {
                ClientOnboarded = bank.BankAccounts.Count(),
                TotalTransactions = transactions.Count(),
                TotalAccountBalances = balance
            };
            return Ok(data);
        }


        [HttpGet("GetDashBoardSuperAdmin")]
        public async Task<ActionResult> GetDashBoardSuperAdmin()
        {

            IEnumerable<Bank> banks = await _context.Banks.ToListAsync();
            IEnumerable<Client> clientsPending = await _context.Clients.Where(s => s.Status == StatusEnum.Submitted || s.Status == StatusEnum.InProcess).ToListAsync();
            IEnumerable<Transaction> transactions = await _context.Transactions.Where(s => s.Status == StatusEnum.Submitted).ToListAsync();

            var data = new
            {
                Banks = banks.Count(),
                ClientsPending = clientsPending.Count(),
                Transactions = transactions.Count()
            };
            return Ok(data);
        }

        [HttpGet("GetDashBoardClient/{id}")]
        public async Task<ActionResult> GetDashBoardClient(int id)
        {
            Client client = await _context.Clients.Include("BankAccount").SingleOrDefaultAsync(s => s.ClientId == id);

            IEnumerable<Transaction> transactionspend = await _context.Transactions.Where(s => s.Status == StatusEnum.Submitted && s.SenderId == id).ToListAsync();
            IEnumerable<Transaction> transactionsrej = await _context.Transactions.Where(s => s.Status == StatusEnum.Rejected && s.SenderId == id).ToListAsync();

            var data = new
            {
                MyBalance = client.BankAccount.Balance,
                FundsBlocked = client.BankAccount.BlockedFunds,
                TransactionsPending = transactionspend.Count(),
                TransactionsRejected = transactionsrej.Count()
            };
            return Ok(data);
        }





        // SUPERADMIN MODULE WORKING

        [HttpGet("GetKycDetailsBank/{id}")]
        public async Task<BankKyc> GetKycDetailsBank(int id)
        {
            var bank = await _context.Banks.Where(s => s.BankId == id).Include(b=>b.BankKyc).ThenInclude(b=>b.LicenseRegulatorApprovalsOrLicenseAgreement).Include(c=>c.BankKyc).ThenInclude(c=>c.FinancialStatements).Include(d => d.BankKyc).ThenInclude(d => d.AnnualReports).FirstOrDefaultAsync();
            return bank.BankKyc;
        }

        [HttpGet("GetKycDetailsClient/{id}")]
        public async Task<ClientKyc> GetKycDetailsClient(int id)
        {
            var client = await _context.Clients
    .Where(s => s.ClientId == id)
    .Include(c => c.ClientKyc) 
        .ThenInclude(c => c.PowerOfAttorney) 
    .Include(c => c.ClientKyc) 
        .ThenInclude(c => c.BankAccess)
    .Include(c => c.ClientKyc)
        .ThenInclude(c => c.MOU)
    .FirstOrDefaultAsync();
            return client.ClientKyc;
        }



        //Client Module 

        [HttpGet("GetClientForBeneficiary/{id}")]
        //[Authorize]
        public async Task<List<GetBeneficiaryDTO>> GetClientForBeneficiary(int id)
        {
            var clients = await _context.Clients
                .Where(s => s.Status == StatusEnum.Approved && s.ClientId != id)
                .ToListAsync();


            var beneficiaryDTOs = clients.Select(c => new GetBeneficiaryDTO
            {
                ClientId = c.ClientId,
                CompanyName = c.CompanyName
            }).ToList();

            return beneficiaryDTOs;
        }


        [HttpGet("GetBeneficiaryOfClient/{id}")]
        public async Task<List<Client>> GetBeneficiaryOfClient(int id)
        {

            var clients = await _context.Clients
                    .Where(s => s.ClientId == id).FirstOrDefaultAsync();

            List<Client> listClient = new List<Client>();
            foreach (var client in clients.BeneficiaryLists)
            {
                Client tempClient = _context.Clients.Include("BankAccount").Where(s=>s.ClientId == client).FirstOrDefault();
                listClient.Add(tempClient);
            }

            return listClient;
        }
    }


}
