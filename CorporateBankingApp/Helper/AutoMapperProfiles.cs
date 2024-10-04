using AutoMapper;
using CorporateBankingApp.DTO;
using CorporateBankingApp.Models;

namespace CorporateBankingApp.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Client, NewClientDTO>().ReverseMap();
            CreateMap<ViewSubmittedClientDTO, Client>().ReverseMap();
            CreateMap<Client,OutboundBeneficiaryDTO>().ReverseMap();
            CreateMap<Bank,BankDTO>().ReverseMap();
        }
    }
}
 