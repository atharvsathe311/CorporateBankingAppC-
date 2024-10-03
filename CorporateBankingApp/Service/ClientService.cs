using System.Collections.Generic;
using System.Threading.Tasks;
using CorporateBankingApp.GlobalException;
using CorporateBankingApp.Models;
using CorporateBankingApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CorporateBankingApp.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _clientRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Client>> GetAllSubmittedClientsAsync()
        {
            return await _clientRepository.GetAllSubmittedAsync();
        }

        public async Task<Client> GetClientByIdAsync(int id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
            {
                throw new CustomException("Client not found");
            }
            return client;
        }

        public async Task CreateClientAsync(Client client)
        {
            await _clientRepository.AddAsync(client);
        }

        public async Task UpdateClientAsync(Client client)
        {
            await _clientRepository.UpdateAsync(client);
        }

        public async Task DeleteClientAsync(int id)
        {
            await _clientRepository.DeleteAsync(id);
        }

        public FileDetail Upload(IFormFile file)
        {
            List<string> validExtensions = new List<string>()
            { ".jpeg",".jpg",".png",".gif"};
            string extension = Path.GetExtension(file.FileName);

            if (!validExtensions.Contains(extension))
            {
                return new FileDetail() { FileName = "Extension is not valid" };
            }
            long size = file.Length;
            if (size > (5 * 1024 * 1024))
            {
                return new FileDetail() { FileName = "Max size can't exeeds 5Mb" };
            }

            string fileName = Guid.NewGuid().ToString() + extension;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            FileStream fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
            file.CopyTo(fileStream);

            var fileDetails = new FileDetail()
            {
                FileName = fileName,
                FileExtension = extension,
                FilePath = path,
                DateUploaded = DateTime.Now
            };

            fileStream.Dispose();
            fileStream.Close();
            return fileDetails;
        }


        public async Task<BankAccount> GetClientBankAccount(int id)
        {
            return await _clientRepository.GetClientBankAccount(id);
        }

        public async Task AddTransaction(Transaction transaction)
        {
            _clientRepository.AddTransaction(transaction);
        }

    }
}
