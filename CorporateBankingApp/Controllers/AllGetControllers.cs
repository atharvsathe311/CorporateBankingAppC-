using CorporateBankingApp.Data;
using CorporateBankingApp.Models;
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
        public AllGetControllers(CorporateBankAppDbContext corporateBankAppDbContext) 
        {
            _context = corporateBankAppDbContext;
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
    }


}
