using CorporateBankingApp.Data;
using CorporateBankingApp.DTO;
using CorporateBankingApp.Models;
using CorporateBankingApp.Service;
using CorporateBankingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IEmailService _emailService;

        public PaymentController(CorporateBankAppDbContext corporateBankAppDbContext, IClientService clientService, ITransactionService transactionService,IEmailService emailService)
        {
            _corporateBankAppDbContext = corporateBankAppDbContext;
            _clientService = clientService;
            _transactionService = transactionService;
            _emailService = emailService;
        }

        [HttpPost("SinglePayment")]
        public async Task<string> SinglePayment([FromBody] PaymentDTO paymentDTO)
        {
            Transaction transaction = new Transaction();

            BankAccount sender = await _clientService.GetClientBankAccount(paymentDTO.SenderId);
            BankAccount receiver = await _clientService.GetClientBankAccount(paymentDTO.ReceiverId);
            Client client = await _clientService.GetClientByIdAsync(paymentDTO.SenderId);

            decimal amount = decimal.Parse(paymentDTO.Amount);
            transaction.SenderId = paymentDTO.SenderId;
            transaction.ReceiverId = paymentDTO.ReceiverId;
            transaction.SenderBankId = sender.BankId;
            transaction.ReceiverBankId = receiver.BankId;
            transaction.Amount = paymentDTO.Amount;
            transaction.DateTime = DateTime.Now;
            transaction.Status = StatusEnum.Submitted;
            transaction.Remarks = "Transaction Added " + paymentDTO.Remarks;


            if (sender.Balance >= amount + 500 && sender.AccountPayables >= amount + 500)
            {
                sender.Balance -= amount;
                sender.AccountPayables -= amount;
                sender.BlockedFunds += amount;

                await _clientService.AddTransaction(transaction);
                await _emailService.SendNewTransactionEmail(client.CompanyEmail,GetBankDetails(sender.BankId).ToString(),sender.AccountId.ToString(), transaction.TransactionId.ToString(),amount, transaction.DateTime,transaction.Status.ToString(),"Submitted");
                await _corporateBankAppDbContext.SaveChangesAsync();

                return "Transaction Added";
            }

            transaction.Status = StatusEnum.Rejected;
            transaction.Remarks = "Transaction Rejected Due to Insufficient Funds or Account Payable " + transaction.Remarks;
            await _clientService.AddTransaction(transaction);
            await _corporateBankAppDbContext.SaveChangesAsync();
            return "Transaction Rejected Due to Insufficient Funds or Account Payable";
        }

        [HttpPost("BulkPayment")]
        public async Task<string> BulkPayment([FromBody] List<PaymentDTO> paymentDTOs)
        {
            BankAccount sender = await _clientService.GetClientBankAccount(paymentDTOs[0].SenderId);
            Client client = await _clientService.GetClientByIdAsync(paymentDTOs[0].SenderId);


            foreach (PaymentDTO paymentDTO in paymentDTOs)
            {
                Transaction transaction = new Transaction();
                BankAccount receiver = await _clientService.GetClientBankAccount(paymentDTO.ReceiverId);

                decimal amount = decimal.Parse(paymentDTO.Amount);

                transaction.SenderId = paymentDTO.SenderId;
                transaction.ReceiverId = paymentDTO.ReceiverId;
                transaction.Amount = paymentDTO.Amount;
                transaction.DateTime = DateTime.Now;
                transaction.Status = StatusEnum.Submitted;
                transaction.Remarks = "Transaction Added " + paymentDTO.Remarks;
                transaction.SenderBankId = sender.BankId;
                transaction.ReceiverBankId = receiver.BankId;

                if (sender.Balance >= amount + 500 && sender.AccountPayables >= amount + 500)
                {
                    sender.Balance -= amount;
                    sender.AccountPayables -= amount;
                    sender.BlockedFunds += amount;
                    await _clientService.AddTransaction(transaction);
                    await _emailService.SendNewTransactionEmail(client.CompanyEmail, GetBankDetails(sender.BankId).ToString(), sender.AccountId.ToString(), transaction.TransactionId.ToString(), amount, transaction.DateTime, transaction.Status.ToString(), "Submitted");
                    continue;
                }

                transaction.Status = StatusEnum.Rejected;
                transaction.Remarks = "Transaction Rejected Due to Insufficient Funds or Account Payable" + paymentDTO.Remarks;
            }
            await _corporateBankAppDbContext.SaveChangesAsync();
            return "Bulk Payment Processed";
        }

        [HttpPost("SalaryPayment")]
        public async Task<string> SalaryPayment([FromBody] List<SalaryDTO> salaryDTOs)
        {
            BankAccount sender = await _clientService.GetClientBankAccount(salaryDTOs[0].SenderId);
            Client client = await _clientService.GetClientByIdAsync(salaryDTOs[0].SenderId);


            foreach (SalaryDTO salaryDto in salaryDTOs)
            {
                Transaction transaction = new Transaction();
                decimal amount = decimal.Parse(salaryDto.Amount);

                transaction.SenderId = salaryDto.SenderId;
                transaction.ReceiverId = salaryDto.ReceiverId;
                transaction.Amount = salaryDto.Amount;
                transaction.DateTime = DateTime.Now;
                transaction.Status = StatusEnum.Submitted;
                transaction.Remarks = "Salary To " + salaryDto.Name + " " + salaryDto.Remarks;
                transaction.SenderBankId = sender.BankId;
                transaction.ReceiverBankId = 99999;

                if (sender.Balance >= amount + 500 && sender.SalaryPayments >= amount + 500)
                {
                    sender.SalaryPayments -= amount;
                    sender.BlockedFunds += amount;
                    await _clientService.AddTransaction(transaction);
                    await _emailService.SendNewTransactionEmail(client.CompanyEmail, GetBankDetails(sender.BankId).ToString(), sender.AccountId.ToString(), transaction.TransactionId.ToString(), amount, transaction.DateTime, transaction.Status.ToString(), "Submitted");
                    continue;
                }

                transaction.Status = StatusEnum.Rejected;
                transaction.Remarks = "Transaction Rejected Due to Insufficient Salary " + transaction.Remarks;
            }
            await _corporateBankAppDbContext.SaveChangesAsync();
            return "Salary Payment Processed";
        }

        [HttpPost("SinglePaymentAccepted/{id}")]
        public async Task<string> SinglePaymentAccepted(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            BankAccount sender = await _clientService.GetClientBankAccount(transaction.SenderId);
            BankAccount receiver = await _clientService.GetClientBankAccount(transaction.ReceiverId);

            transaction.Status = StatusEnum.Approved;
            decimal amount = decimal.Parse(transaction.Amount);

            if (sender.BlockedFunds >= amount)
            {
                sender.BlockedFunds -= amount;
                receiver.AddFunds(amount);
                transaction.Remarks = "Payment Success";
            }
            else
            {
                transaction.Status = StatusEnum.Rejected;
                transaction.Remarks = "Blocked funds insufficient for transaction release";
            }

            await _corporateBankAppDbContext.SaveChangesAsync();
            return transaction.Status == StatusEnum.Approved ? "Payment Success" : "Payment Rejected";
        }

        [HttpPost("BulkPaymentAccepted")]
        public async Task<string> BulkPaymentAccepted([FromBody] List<int> ids)
        {
            foreach (int id in ids)
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(id);
                BankAccount sender = await _clientService.GetClientBankAccount(transaction.SenderId);
                Client client = await _clientService.GetClientByIdAsync(transaction.SenderId);
                transaction.Status = StatusEnum.Approved;

                decimal amount = decimal.Parse(transaction.Amount);
                if (transaction.Remarks.StartsWith("Salary"))
                {
                    if (sender.BlockedFunds >= amount)
                    {
                        sender.BlockedFunds -= amount;
                        transaction.Remarks += " Salary Payment Success";
                        _emailService.SendTransactionStatusEmail(client.CompanyEmail, GetBankDetails(sender.BankId).ToString(), "Approved", sender.AccountId.ToString(), transaction.TransactionId.ToString(), amount, transaction.DateTime, transaction.Remarks);
                    }
                    else
                    {
                        transaction.Status = StatusEnum.Rejected;
                        transaction.Remarks += " Insufficient blocked funds for salary release";
                        _emailService.SendTransactionStatusEmail(client.CompanyEmail, GetBankDetails(sender.BankId).ToString(), "Rejected", sender.AccountId.ToString(), transaction.TransactionId.ToString(), amount, transaction.DateTime, transaction.Remarks);
                    }
                }
                else
                {
                    BankAccount receiver = await _clientService.GetClientBankAccount(transaction.ReceiverId);
                    Client receiverClient = await _clientService.GetClientByIdAsync(transaction.ReceiverId);

                    if (sender.BlockedFunds >= amount)
                    {
                        sender.BlockedFunds -= amount;
                        receiver.AddFunds(amount);
                        transaction.Remarks += " Payment Success";
                        _emailService.SendTransactionStatusEmail(client.CompanyEmail, GetBankDetails(sender.BankId).ToString(), "Approved", sender.AccountId.ToString(), transaction.TransactionId.ToString(), amount, transaction.DateTime, transaction.Remarks);
                        _emailService.SendTransactionStatusEmailReceived(receiverClient.CompanyEmail, GetBankDetails(receiver.BankId).ToString(), receiver.AccountId.ToString(), transaction.TransactionId.ToString(), amount, transaction.DateTime, transaction.Remarks, client.CompanyName, receiver.SalaryPercentage, receiver.AccountPayablesPercentage, receiver.TaxesPercentage, receiver.EmergencyFundsPercentage, receiver.InvestmentsAndGrowthPercentage);
                    }
                    else
                    {
                        transaction.Status = StatusEnum.Rejected;
                        transaction.Remarks += " Insufficient blocked funds for payment release";
                        _emailService.SendTransactionStatusEmail(client.CompanyEmail, GetBankDetails(sender.BankId).ToString(), "Rejected", sender.AccountId.ToString(), transaction.TransactionId.ToString(), amount, transaction.DateTime, transaction.Remarks);
                    }
                }
            }

            await _corporateBankAppDbContext.SaveChangesAsync();
            return "Bulk Payment Status Updated";
        }


        [HttpPost("SalaryPaymentAccepted")]
        public async Task<string> SalaryPaymentAccepted([FromBody] List<int> ids)
        {
            foreach (int id in ids)
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(id);
                BankAccount sender = await _clientService.GetClientBankAccount(transaction.SenderId);
                transaction.Status = StatusEnum.Approved;
                decimal amount = decimal.Parse(transaction.Amount);

                // Ensure blocked funds do not go negative
                if (sender.BlockedFunds >= amount)
                {
                    sender.BlockedFunds -= amount;
                    transaction.Remarks += " Payment Success";
                }
                else
                {
                    transaction.Status = StatusEnum.Rejected;
                    transaction.Remarks += " Insufficient blocked funds for release";
                }
            }
            await _corporateBankAppDbContext.SaveChangesAsync();
            return "Salary Payment Status Updated";
        }

        [HttpPost("BulkPaymentRejected")]
        public async Task<string> BulkPaymentRejected([FromBody] RejectTransactionDTO ids)
        {
            foreach (int rejectTransaction in ids.TransactionId)
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(rejectTransaction);
                BankAccount sender = await _clientService.GetClientBankAccount(transaction.SenderId);
                Client client = await _clientService.GetClientByIdAsync(transaction.SenderId);
                decimal amount = decimal.Parse(transaction.Amount);

                if (transaction.Remarks.StartsWith("Salary"))
                {
                    sender.BlockedFunds -= amount;
                    sender.Balance += amount;
                    sender.SalaryPayments += amount;
                    _emailService.SendTransactionStatusEmail(client.CompanyEmail, GetBankDetails(sender.BankId).ToString(), "Rejected", sender.AccountId.ToString(), transaction.TransactionId.ToString(), amount, transaction.DateTime, transaction.Remarks);
                }
                else
                {
                    sender.BlockedFunds -= amount;
                    sender.Balance += amount;
                    sender.AccountPayables += amount;
                    _emailService.SendTransactionStatusEmail(client.CompanyEmail, GetBankDetails(sender.BankId).ToString(), "Rejected", sender.AccountId.ToString(), transaction.TransactionId.ToString(), amount, transaction.DateTime, transaction.Remarks);
                }
                transaction.Status = StatusEnum.Rejected;
                await _corporateBankAppDbContext.SaveChangesAsync();
            }
            return "Bulk Payment Rejections Processed";
        }

        [HttpGet("GetBank")]
        public async Task<string> GetBankDetails(int id)
        {
            Bank bank = await _corporateBankAppDbContext.Banks.Where(b => b.BankId == id).FirstOrDefaultAsync();
            return bank.BankName;
        }
    }

}

