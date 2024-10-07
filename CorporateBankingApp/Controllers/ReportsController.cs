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

        public ReportsController(CorporateBankAppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetCreditPendingTransactionsClient/{id}")]
        public IActionResult GetCreditPendingTransactionsClient(int id, DateTime? startDate, DateTime? endDate)
        {
            var client = _context.Clients.FirstOrDefault(c => c.ClientId == id);
            if (client == null) return NotFound();

            var transactions = _context.Transactions.Where(t => t.ReceiverId == client.ClientId
                && (t.Status == StatusEnum.Submitted || t.Status == StatusEnum.InProcess));

            if (startDate.HasValue)
            {
                transactions = transactions.Where(t => t.DateTime.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                transactions = transactions.Where(t => t.DateTime.Date <= endDate.Value);
            }

            return Ok(transactions.ToList());
        }

        [HttpGet("GetCreditApprovedTransactionsClient/{id}")]
        public IActionResult GetCreditApprovedTransactionsClient(int id, DateTime? startDate, DateTime? endDate)
        {
            var client = _context.Clients.FirstOrDefault(c => c.ClientId == id);
            if (client == null) return NotFound();

            var transactions = _context.Transactions.Where(t => t.ReceiverId == client.ClientId
                && t.Status == StatusEnum.Approved);

            if (startDate.HasValue)
            {
                transactions = transactions.Where(t => t.DateTime.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                transactions = transactions.Where(t => t.DateTime.Date <= endDate.Value);
            }

            return Ok(transactions.ToList());
        }

        [HttpGet("GetCreditRejectedTransactionsClient/{id}")]
        public IActionResult GetCreditRejectedTransactionsClient(int id, DateTime? startDate, DateTime? endDate)
        {
            var client = _context.Clients.FirstOrDefault(c => c.ClientId == id);
            if (client == null) return NotFound();

            var transactions = _context.Transactions.Where(t => t.ReceiverId == client.ClientId
                && t.Status == StatusEnum.Rejected);

            if (startDate.HasValue)
            {
                transactions = transactions.Where(t => t.DateTime.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                transactions = transactions.Where(t => t.DateTime.Date <= endDate.Value);
            }

            return Ok(transactions.ToList());
        }

        [HttpGet("GetDebitPendingTransactionsClient/{id}")]
        public IActionResult GetDebitPendingTransactionsClient(int id, DateTime? startDate, DateTime? endDate)
        {
            var client = _context.Clients.FirstOrDefault(c => c.ClientId == id);
            if (client == null) return NotFound();

            var transactions = _context.Transactions.Where(t => t.SenderId == client.ClientId
                && (t.Status == StatusEnum.Submitted || t.Status == StatusEnum.InProcess));

            if (startDate.HasValue)
            {
                transactions = transactions.Where(t => t.DateTime.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                transactions = transactions.Where(t => t.DateTime.Date <= endDate.Value);
            }

            return Ok(transactions.ToList());
        }

        [HttpGet("GetDebitApprovedTransactionsClient/{id}")]
        public IActionResult GetDebitApprovedTransactionsClient(int id, DateTime? startDate, DateTime? endDate)
        {
            var client = _context.Clients.FirstOrDefault(c => c.ClientId == id);
            if (client == null) return NotFound();

            var transactions = _context.Transactions.Where(t => t.SenderId == client.ClientId
                && t.Status == StatusEnum.Approved);

            if (startDate.HasValue)
            {
                transactions = transactions.Where(t => t.DateTime.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                transactions = transactions.Where(t => t.DateTime.Date <= endDate.Value);
            }

            return Ok(transactions.ToList());
        }

        [HttpGet("GetDebitRejectedTransactionsClient/{id}")]
        public IActionResult GetDebitRejectedTransactionsClient(int id, DateTime? startDate, DateTime? endDate)
        {
            var client = _context.Clients.FirstOrDefault(c => c.ClientId == id);
            if (client == null) return NotFound();

            var transactions = _context.Transactions.Where(t => t.SenderId == client.ClientId
                && t.Status == StatusEnum.Rejected);

            if (startDate.HasValue)
            {
                transactions = transactions.Where(t => t.DateTime.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                transactions = transactions.Where(t => t.DateTime.Date <= endDate.Value);
            }

            return Ok(transactions.ToList());
        }

        [HttpGet("GetTotalCreditedAmountClient/{id}")]
        public IActionResult GetTotalCreditedAmount(int id, DateTime? startDate, DateTime? endDate)
        {
            var client = _context.Clients.FirstOrDefault(c => c.ClientId == id);
            if (client == null) return NotFound();

            var transactions = _context.Transactions.Where(t => t.ReceiverId == client.ClientId
                && t.Status == StatusEnum.Approved);

            if (startDate.HasValue)
            {
                transactions = transactions.Where(t => t.DateTime.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                transactions = transactions.Where(t => t.DateTime.Date <= endDate.Value);
            }

            //var totalCreditedAmount = transactions.Sum(t => int.Parse(t.Amount));
            long totalCreditedAmount = 0;

            foreach (var t in transactions)
            {
                totalCreditedAmount += int.Parse(t.Amount);
            }

            return Ok(totalCreditedAmount);
        }


        [HttpGet("GetTotalDebitedAmountClient/{id}")]
        public IActionResult GetTotalDebitedAmount(int id, DateTime? startDate, DateTime? endDate)
        {
            var client = _context.Clients.FirstOrDefault(c => c.ClientId == id);
            if (client == null) return NotFound();

            var transactions = _context.Transactions.Where(t => t.SenderId == client.ClientId
                && t.Status == StatusEnum.Approved);

            if (startDate.HasValue)
            {
                transactions = transactions.Where(t => t.DateTime.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                transactions = transactions.Where(t => t.DateTime.Date <= endDate.Value);
            }

            //var totalDebitedAmount = transactions.Sum(t => int.Parse(t.Amount));

            long totalDebitedAmount = 0;

            foreach (var t in transactions)
            {
                totalDebitedAmount += int.Parse(t.Amount);
            }

            return Ok(totalDebitedAmount);
        }

        [HttpGet("GetTotalBalanceClient/{id}")]
        public IActionResult GetTotalBalance(int id)
        {
            var client = _context.Clients.Include(c => c.BankAccount).FirstOrDefault(c => c.ClientId == id);
            if (client == null || client.BankAccount == null) return NotFound();

            var totalBalance = client.BankAccount.Balance;
            return Ok(totalBalance);
        }


        [HttpGet("GetFundAllocationPercentages/{id}")]
        public IActionResult GetFundAllocationPercentages(int id)
        {
            var client = _context.Clients.Include(c => c.BankAccount).FirstOrDefault(c => c.ClientId == id);
            if (client == null || client.BankAccount == null)
            {
                return NotFound();
            }

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
        public IActionResult GetTransactionDistribution(DateTime? startDate, DateTime? endDate)
        {
            var transactions = _context.Transactions.ToList().AsQueryable();

            if (startDate.HasValue)
            {
                transactions = transactions.Where(t => t.DateTime.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                transactions = transactions.Where(t => t.DateTime.Date <= endDate.Value.Date);
            }

            var approvedTransactions = transactions.Count(t => t.Status == StatusEnum.Approved);
            var pendingTransactions = transactions.Count(t => t.Status == StatusEnum.Submitted || t.Status == StatusEnum.InProcess);
            var rejectedTransactions = transactions.Count(t => t.Status == StatusEnum.Rejected);

            var transactionDistribution = new
            {
                TotalTransactions = transactions.Count(),
                ApprovedTransactions = approvedTransactions,
                PendingTransactions = pendingTransactions,
                RejectedTransactions = rejectedTransactions
            };

            return Ok(transactionDistribution);
        }

    }
}
