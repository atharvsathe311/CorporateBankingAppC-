using System.Collections.Generic;
using System.Threading.Tasks;
using CorporateBankingApp.GlobalException;
using CorporateBankingApp.Models;
using CorporateBankingApp.Repositories;

namespace CorporateBankingApp.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _transactionRepository.GetAllAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null)
            {
                throw new CustomException("Transaction not found");
            }
            return transaction;
        }

        public async Task CreateTransactionAsync(Transaction transaction)
        {
            await _transactionRepository.AddAsync(transaction);
        }

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            await _transactionRepository.UpdateAsync(transaction);
        }

        public async Task DeleteTransactionAsync(int id)
        {
            await _transactionRepository.DeleteAsync(id);
        }
    }
}
