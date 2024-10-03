using CorporateBankingApp.Data;
using CorporateBankingApp.DTO;
using CorporateBankingApp.Models;
using CorporateBankingApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace CorporateBankingApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly CorporateBankAppDbContext _corporateBankAppDbContext;
        private readonly IClientService _clientService;
        private readonly ITransactionService _transactionService;

        public PaymentController(CorporateBankAppDbContext corporateBankAppDbContext, IClientService clientService, ITransactionService transactionService)
        {
            _corporateBankAppDbContext = corporateBankAppDbContext;
            _clientService = clientService;
            _transactionService = transactionService;
        }

        [HttpPost("SinglePayment")]
        public async Task<string> SinglePayment([FromBody] PaymentDTO paymentDTO)
        {
            Transaction transaction = new Transaction();

            BankAccount sender = await _clientService.GetClientBankAccount(paymentDTO.SenderId);
            BankAccount receiver = await _clientService.GetClientBankAccount(paymentDTO.ReceiverId);

            double amount = Double.Parse(paymentDTO.Amount);

            transaction.SenderId = paymentDTO.SenderId;
            transaction.ReceiverId = paymentDTO.ReceiverId;
            transaction.Amount = paymentDTO.Amount;
            transaction.DateTime = DateTime.Now;
            transaction.Status = StatusEnum.Submitted;

            if (sender.Balance >= amount + 500)
            {
                sender.Balance -= amount;
                await _clientService.AddTransaction(transaction);
                _corporateBankAppDbContext.SaveChanges();

                return "Transaction Added";
            }
            transaction.Status = StatusEnum.Rejected;
            transaction.Remarks = "Transaction Rejected Due to Insufficient Funds";
            return "Transaction Rejected Due to Insufficient Funds";

        }

        [HttpPost("SinglePaymentAccepted/{id}")]
        public async Task<string> SinglePaymentAccepted(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            BankAccount sender = await _clientService.GetClientBankAccount(transaction.SenderId);
            BankAccount receiver = await _clientService.GetClientBankAccount(transaction.ReceiverId);
            transaction.Status = StatusEnum.Approved;
            double amount = Double.Parse(transaction.Amount);
            sender.BlockedFunds -= amount;
            receiver.Balance += amount;
            _corporateBankAppDbContext.SaveChanges();
            transaction.Remarks = "Payment Sucess";
            return "Payment Sucess";
        }


        [HttpPost("BulkPayment")]
        public async Task<string> BulkPayment([FromBody] List<PaymentDTO> paymentDTOs)
        {
            BankAccount sender = await _clientService.GetClientBankAccount(paymentDTOs[0].SenderId);

            foreach (PaymentDTO paymentDTO in paymentDTOs)
            {
                Transaction transaction = new Transaction();
                BankAccount receiver = await _clientService.GetClientBankAccount(paymentDTO.ReceiverId);

                double amount = Double.Parse(paymentDTO.Amount);

                transaction.SenderId = paymentDTO.SenderId;
                transaction.ReceiverId = paymentDTO.ReceiverId;
                transaction.Amount = paymentDTO.Amount;
                transaction.DateTime = DateTime.Now;
                transaction.Status = StatusEnum.Submitted;

                if (sender.Balance >= amount + 500)
                {
                    sender.Balance -= amount;
                    sender.BlockedFunds += amount;
                    await _clientService.AddTransaction(transaction);
                    transaction.Remarks = "Transaction Added";
                    continue;
                }
                transaction.Status = StatusEnum.Rejected;
                transaction.Remarks =  "Transaction Rejected Due to Insufficient Funds";
            }
            _corporateBankAppDbContext.SaveChanges();
            return "";
        }

        [HttpPost("BulkPaymentAccepted")]
        public async Task<string> BulkPaymentAccepted([FromBody] List<int> ids)
        {
            foreach (int id in ids)
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(id);
                BankAccount sender = await _clientService.GetClientBankAccount(transaction.SenderId);
                BankAccount receiver = await _clientService.GetClientBankAccount(transaction.ReceiverId);
                transaction.Status = StatusEnum.Approved;
                double amount = Double.Parse(transaction.Amount);
                sender.BlockedFunds -= amount;
                receiver.Balance += amount;
                transaction.Remarks = "Payment Sucess";
            }
            _corporateBankAppDbContext.SaveChanges();
            return "";
        }

        [HttpPost("SalaryPayment")]
        public async Task<string> SalaryPayment([FromBody] List<SalaryDTO> salaryDTOs)
        {
            BankAccount sender = await _clientService.GetClientBankAccount(salaryDTOs[0].SenderId);

            foreach (SalaryDTO salaryDto in salaryDTOs)
            {
                Transaction transaction = new Transaction();
                double amount = Double.Parse(salaryDto.Amount);

                transaction.SenderId = salaryDto.SenderId;
                transaction.ReceiverId = 0;
                transaction.Amount = salaryDto.Amount;
                transaction.DateTime = DateTime.Now;
                transaction.Status = StatusEnum.Submitted;
                transaction.Remarks = salaryDto.ReceiverBankAccountNumber + salaryDto.ReceiverBankAccountIFSCCode;

                if (sender.Balance >= amount + 500)
                {
                    sender.Balance -= amount;
                    sender.BlockedFunds += amount;
                    await _clientService.AddTransaction(transaction);
                    continue;
                }
                transaction.Status = StatusEnum.Rejected;
                transaction.Remarks = transaction.Remarks + "Transaction Rejected Due to Insufficient Funds";
            }
            _corporateBankAppDbContext.SaveChanges();
            return "";
        }

        [HttpPost("SalaryPaymentAccepted")]
        public async Task<string> Post([FromBody] List<int> ids)
        {
            foreach (int id in ids)
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(id);
                BankAccount sender = await _clientService.GetClientBankAccount(transaction.SenderId);
                transaction.Status = StatusEnum.Approved;
                double amount = Double.Parse(transaction.Amount);
                sender.BlockedFunds -= amount;
                transaction.Remarks = transaction.Remarks + "Payment Sucess";
            }
            _corporateBankAppDbContext.SaveChanges();
            return "";
        }

    }
}
