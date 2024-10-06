using CorporateBankingApp.Data;
using CorporateBankingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CorporateBankingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly CorporateBankAppDbContext _context;
        //private readonly H

        public ReportsController(CorporateBankAppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetCreditPendingTransactionsClient/{id}")]
        public IActionResult GetCreditPendingTransactionsClient(int id)
        {
            var client = _context.Clients.Where(c => c.ClientId == id).FirstOrDefault();

            var transactions = _context.Transactions.Where(t => t.ReceiverId == client.ClientId && t.Status == StatusEnum.Submitted || t.Status==StatusEnum.InProcess).ToList();
            return Ok(transactions);
        }

        [HttpGet("GetCreditApprovedTransactionsClient/{id}")]
        public IActionResult GetCreditApprovedTransactionsClient(int id)
        {
            var client = _context.Clients.Where(c => c.ClientId == id).FirstOrDefault();

            var transactions = _context.Transactions.Where(t => t.ReceiverId == client.ClientId && t.Status == StatusEnum.Approved).ToList();
            return Ok(transactions);
        }

        [HttpGet("GetCreditRejectedTransactionsClient/{id}")]
        public IActionResult GetCreditRejectedTransactionsClient(int id)
        {
            var client = _context.Clients.Where(c => c.ClientId == id).FirstOrDefault();

            var transactions = _context.Transactions.Where(t => t.ReceiverId == client.ClientId && t.Status == StatusEnum.Rejected).ToList();
            return Ok(transactions);
        }

        [HttpGet("GetDebitPendingTransactionsClient/{id}")]
        public IActionResult GetDebitPendingTransactionsClient(int id)
        {
            var client = _context.Clients.Where(c => c.ClientId == id).FirstOrDefault();

            var transactions = _context.Transactions.Where(t => t.SenderId == client.ClientId && t.Status == StatusEnum.Submitted || t.Status == StatusEnum.InProcess).ToList();
            return Ok(transactions);
        }

        [HttpGet("GetDebitApprovedTransactionsClient/{id}")]
        public IActionResult GetDebitApprovedTransactionsClient(int id)
        {
            var client = _context.Clients.Where(c => c.ClientId == id).FirstOrDefault();

            var transactions = _context.Transactions.Where(t => t.SenderId == client.ClientId && t.Status == StatusEnum.Approved).ToList();
            return Ok(transactions);
        }

        [HttpGet("GetDebitRejectedTransactionsClient/{id}")]
        public IActionResult GetDebitRejectedTransactionsClient(int id)
        {
            var client = _context.Clients.Where(c => c.ClientId == id).FirstOrDefault();

            var transactions = _context.Transactions.Where(t => t.SenderId == client.ClientId && t.Status == StatusEnum.Rejected).ToList();
            return Ok(transactions);
        }

        // GET api/<ReportsController>/5
        [HttpGet("GetCreditTransactionsClient/{id}")]
        public IActionResult GetCreditTransactionsClient(int id)
        {
            var client = _context.Clients.Where(c=>c.ClientId == id).FirstOrDefault();

            var transactions = _context.Transactions.Where(t => t.ReceiverId == client.ClientId && t.Status == StatusEnum.Approved).ToList();
            return Ok(transactions);
        }

        [HttpGet("GetDebitTransactionsClient/{id}")]
        public IActionResult GetDebitTransactionsClient(int id)
        {
            var client = _context.Clients.Where(c => c.ClientId == id).FirstOrDefault();

            var transactions = _context.Transactions.Where(t => t.SenderId == client.ClientId && t.Status == StatusEnum.Approved).ToList();
            return Ok(transactions);
        }

        [HttpGet("GetTotalCreditedAmountClient/{id}")]
        public IActionResult GetTotalCreditedAmount(int id)
        {
            var client = _context.Clients.Where(c => c.ClientId == id).FirstOrDefault();

            var transactions = _context.Transactions.Where(t => t.ReceiverId == client.ClientId && t.Status == StatusEnum.Approved).ToList();

            var totalCreditedAmount = 0;

            foreach (var transaction in transactions)
            {
                totalCreditedAmount += int.Parse(transaction.Amount);
            }
            return Ok(totalCreditedAmount);
        }

        [HttpGet("GetTotalDebitedAmountClient/{id}")]
        public IActionResult GetTotalDebitedAmount(int id)
        {
            var client = _context.Clients.Where(c => c.ClientId == id).FirstOrDefault();

            var transactions = _context.Transactions.Where(t => t.SenderId == client.ClientId && t.Status == StatusEnum.Approved).ToList();

            var totalDebitedAmount = 0;

            foreach (var transaction in transactions)
            {
                totalDebitedAmount += int.Parse(transaction.Amount);
            }

            Console.WriteLine(totalDebitedAmount);
            return Ok(totalDebitedAmount);
        }


        [HttpGet("GetTotalBalanceClient/{id}")]
        public IActionResult GetTotalBalance(int id)
        {
            var client = _context.Clients.Include(c => c.BankAccount).Where(c => c.ClientId == id).FirstOrDefault();

            var totalBalance = client.BankAccount.Balance;
            Console.WriteLine(totalBalance);
            return Ok(totalBalance);
        }

        [HttpGet("GetFundAllocationPercentages/{id}")]
        public IActionResult GetFundAllocationPercentages(int id)
        {
            var client = _context.Clients.Include(c => c.BankAccount).Where(c => c.ClientId == id).FirstOrDefault();

            if (client == null || client.BankAccount == null)
            {
                return NotFound();
            }

            // Assuming you have a way to get the allocation percentages
            var percentages = new List<decimal>
            {
                client.BankAccount.InvestmentsAndGrowthPercentage,
                client.BankAccount.EmergencyFundsPercentage,
                client.BankAccount.AccountPayablesPercentage,
                client.BankAccount.TaxesPercentage,
                client.BankAccount.SalaryPercentage
            };

            return Ok(percentages);
        }


        [HttpGet("GetClientsOnboardedAdmin")]
        public IActionResult GetClientsOnboarded()
        {
            var totalClients = _context.Clients.Count();
            var approvedClients = _context.Clients.Count(c => c.Status == StatusEnum.Approved);
            var pendingClients = _context.Clients.Count(c => c.Status == StatusEnum.Submitted || c.Status == StatusEnum.InProcess);
            var rejectedClients = _context.Clients.Count(c => c.Status == StatusEnum.Rejected);

            var clientDistribution = new
            {
                TotalClients = totalClients,
                ApprovedClients = approvedClients,
                PendingClients = pendingClients,
                RejectedClients = rejectedClients
            };

            return Ok(clientDistribution);
        }

        // Get the number of banks onboarded
        [HttpGet("GetBanksOnboardedAdmin")]
        public IActionResult GetBanksOnboarded()
        {
            var totalBanks = _context.Banks.Count();
            var approvedBanks = _context.Banks.Count(b => b.Status == StatusEnum.Approved);
            var pendingBanks = _context.Banks.Count(b => b.Status == StatusEnum.Submitted || b.Status == StatusEnum.InProcess);
            var rejectedBanks = _context.Banks.Count(b => b.Status == StatusEnum.Rejected);

            var bankDistribution = new
            {
                TotalBanks = totalBanks,
                ApprovedBanks = approvedBanks,
                PendingBanks = pendingBanks,
                RejectedBanks = rejectedBanks
            };

            return Ok(bankDistribution);
        }

        // Get transaction distribution for admin
        [HttpGet("GetTransactionDistributionAdmin")]
        public IActionResult GetTransactionDistribution()
        {
            var totalTransactions = _context.Transactions.Count();
            var approvedTransactions = _context.Transactions.Count(t => t.Status == StatusEnum.Approved);
            var pendingTransactions = _context.Transactions.Count(t => t.Status == StatusEnum.Submitted || t.Status == StatusEnum.InProcess);
            var rejectedTransactions = _context.Transactions.Count(t => t.Status == StatusEnum.Rejected);

            var transactionDistribution = new
            {
                TotalTransactions = totalTransactions,
                ApprovedTransactions = approvedTransactions,
                PendingTransactions = pendingTransactions,
                RejectedTransactions = rejectedTransactions
            };

            return Ok(transactionDistribution);
        }

    }
}
