using System.Collections.Generic;
using System.Threading.Tasks;
using CorporateBankingApp.GlobalException;
using CorporateBankingApp.Models;
using CorporateBankingApp.Repositories;

namespace CorporateBankingApp.Services
{
    public class BankService : IBankService
    {
        private readonly IBankRepository _bankRepository;

        public BankService(IBankRepository bankRepository)
        {
            _bankRepository = bankRepository;
        }

        public async Task<IEnumerable<Bank>> GetAllBanksAsync()
        {
            return await _bankRepository.GetAllAsync();
        }

        public async Task<Bank> GetBankByIdAsync(int id)
        {
            var bank = await _bankRepository.GetByIdAsync(id);
            if (bank == null)
            {
                throw new CustomException("Bank not found");
            }
            return bank;
        }

        public async Task CreateBankAsync(Bank bank)
        {
            await _bankRepository.AddAsync(bank);
        }

        public async Task UpdateBankAsync(Bank bank)
        {
            await _bankRepository.UpdateAsync(bank);
        }

        public async Task DeleteBankAsync(int id)
        {
            await _bankRepository.DeleteAsync(id);
        }

        public string ParseString(string input)
        {
            int requiredLength = 4;
            input = input.Trim();

            string output = "";
            Random random = new Random();

            foreach (char c in input)
            {
                if (c == ' ')
                    continue;

                output += c;

                if (output.Length == requiredLength)
                    break;
            }

            while (output.Length < requiredLength)
            {
                char randomChar = (char)random.Next('A', 'Z' + 1);
                output += randomChar;
            }

            return output;
        }

    }
}
