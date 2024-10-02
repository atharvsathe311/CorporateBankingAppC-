using System.Collections.Generic;
using System.Threading.Tasks;
using CorporateBankingApp.GlobalException;
using CorporateBankingApp.Models;
using CorporateBankingApp.Repositories;

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
    }
}
