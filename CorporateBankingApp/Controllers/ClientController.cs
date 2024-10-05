using Microsoft.AspNetCore.Mvc;
using CorporateBankingApp.Models;
using CorporateBankingApp.Services;
using CorporateBankingApp.DTO;
using CorporateBankingApp.Service;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using CorporateBankingApp.Data;
using System.Collections.Generic;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
//using CloudinaryDotNet;
//using CloudinaryDotNet.Actions;

namespace CorporateBankingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IBankService _bankService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CorporateBankAppDbContext _dbContext;
        private readonly Cloudinary _cloudinary;

        public ClientController(IClientService clientService, IEmailService emailService, IMapper mapper, IBankService bankService, IHttpContextAccessor httpContextAccessor, CorporateBankAppDbContext corporateBankAppDbContext, Cloudinary cloudinary)
        {
            _clientService = clientService;
            _emailService = emailService;
            _mapper = mapper;
            _bankService = bankService;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = corporateBankAppDbContext;
            _cloudinary = cloudinary;

        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Client>>> GetAllClients()
        {
            var clients = await _clientService.GetAllClientsAsync();
            return Ok(clients);
        }

        [HttpPost]
        public async Task<ActionResult> CreateClient([FromBody] NewClientDTO clientDTO)
        {
            if (clientDTO == null)
            {
                return BadRequest("Client data is required.");
            }

            Bank bank = await _bankService.GetBankByIdAsync(clientDTO.BankId);
            if (bank == null)
            {
                return NotFound($"Bank with ID {clientDTO.BankId} not found.");
            }

            var client = _mapper.Map<Client>(clientDTO);

            int count = await _clientService.GetCounter();

            UserLogin userLogin = new UserLogin
            {
                LoginUserName = clientDTO.CompanyName.Substring(0, 4) + count,
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("Admin@123"),
                UserType = UserType.Client
            };

            client.UserLogin = userLogin;
            client.CreatedAt = DateTime.Now;
            client.Status = StatusEnum.Submitted;
            client.isActive = true;
            client.BankId = clientDTO.BankId;
            client.BeneficiaryLists = new List<int>() { };
            await _clientService.CreateClientAsync(client);

            if (client.ClientId != 0) // Ensure ClientId is set
            {
                bank.ClientList.Add(client.ClientId);
            }
            else
            {
                return BadRequest("Failed to create client; ClientId is not set.");
            }

            if (!string.IsNullOrEmpty(clientDTO.CompanyEmail))
            {
                _emailService.SendNewRegistrationMail(clientDTO.CompanyEmail, clientDTO.CompanyName, client.UserLogin.LoginUserName, "Admin@123");
            }
            else
            {
                return BadRequest("Company email is required.");
            }

            return Ok(client);
        }

        [Authorize]
        [HttpPost("Upload-Kyc-Documents")]
        public async Task<IActionResult> UploadKYCDocuments(BankKYCDTO bankKyc)
        {
            Bank bank = await _bankService.GetBankByIdAsync(bankKyc.BankId);

            if (bankKyc.LicenseAgreement == null || bankKyc.FinancialStatement == null || bankKyc.AnnualReport == null)
            {
                return BadRequest("All documents are required.");
            }

            var uploadResult = await UploadToCloudinary(bankKyc.LicenseAgreement);
            if (uploadResult == null) return StatusCode(500, "License Agreement upload failed");

            var financialUploadResult = await UploadToCloudinary(bankKyc.FinancialStatement);
            if (financialUploadResult == null) return StatusCode(500, "Financial Statement upload failed");

            var reportUploadResult = await UploadToCloudinary(bankKyc.AnnualReport);
            if (reportUploadResult == null) return StatusCode(500, "Annual Report upload failed");

            bank.BankKyc = new BankKyc()
            {
                LicenseNumber = bankKyc.LicenseNumber,
                TaxpayerIdentificationNumber = bankKyc.TINNumber,
                LicenseRegulatorApprovalsOrLicenseAgreement = new FileDetail()
                {
                    FileName = bank.BankName + bank.BankId + "LRA",
                    FileExtension = ".jpg",
                    FilePath = uploadResult.SecureUrl.AbsoluteUri,
                    DateUploaded = DateTime.Now,
                    Status = StatusEnum.Approved
                },
                FinancialStatements = new FileDetail()
                {
                    FileName = bank.BankName + bank.BankId + "FinancialStatements",
                    FileExtension = ".jpg",
                    FilePath = financialUploadResult.SecureUrl.AbsoluteUri,
                    DateUploaded = DateTime.Now,
                    Status = StatusEnum.Approved
                },
                AnnualReports = new FileDetail()
                {
                    FileName = bank.BankName + bank.BankId + "AnnualReports",
                    FileExtension = ".jpg",
                    FilePath = reportUploadResult.SecureUrl.AbsoluteUri,
                    DateUploaded = DateTime.Now,
                    Status = StatusEnum.Approved
                }
            };
            bank.Status = StatusEnum.InProcess;
            _dbContext.SaveChanges();
            return Ok(new { message = "KYC documents uploaded successfully." });

        }

        [HttpPost("Upload-Kyc-Documents-Client")]
        public async Task<IActionResult> UploadKYCDocumentsClient(ClientKYCDTO clientKycDto)
        {
            Client client = await _clientService.GetClientByIdAsync(clientKycDto.ClientId);

            if (clientKycDto.PowerOfAttorney == null || clientKycDto.BankAccess == null || clientKycDto.MOU == null)
            {
                return BadRequest("All documents are required.");
            }

            var uploadResult1 = await UploadToCloudinary(clientKycDto.PowerOfAttorney);
            if (uploadResult1 == null) return StatusCode(500, "License Agreement upload failed");

            var uploadResult2 = await UploadToCloudinary(clientKycDto.BankAccess);
            if (uploadResult2 == null) return StatusCode(500, "Financial Statement upload failed");

            var uploadResult3 = await UploadToCloudinary(clientKycDto.MOU);
            if (uploadResult3 == null) return StatusCode(500, "Annual Report upload failed");

            //// Process further or return success
            ////return Ok(new
            ////{

            ////});

            client.ClientKyc = new ClientKyc()
            {
                CINNumber = clientKycDto.CINNumber,
                PanNumber = clientKycDto.PanNumber,
                PowerOfAttorney = new FileDetail()
                {
                    FileName = client.CompanyName + client.ClientId + "POA",
                    FileExtension = ".jpg",
                    FilePath = uploadResult1.SecureUrl.AbsoluteUri,
                    DateUploaded = DateTime.Now,
                    Status = StatusEnum.Approved
                },
                BankAccess = new FileDetail()
                {
                    FileName = client.CompanyName + client.ClientId + "BankAccess",
                    FileExtension = ".jpg",
                    FilePath = uploadResult2.SecureUrl.AbsoluteUri,
                    DateUploaded = DateTime.Now,
                    Status = StatusEnum.Approved
                },
                MOU = new FileDetail
                {
                    FileName = client.CompanyName + client.ClientId + "BankAccess",
                    FileExtension = ".jpg",
                    FilePath = uploadResult3.SecureUrl.AbsoluteUri,
                    DateUploaded = DateTime.Now,
                    Status = StatusEnum.Approved
                }
            };
            client.Status = StatusEnum.InProcess;
            _dbContext.SaveChanges();
            return Ok(new { message = "KYC documents uploaded successfully." });

        }

        private async Task<ImageUploadResult> UploadToCloudinary(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream)
                    };

                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
            }
            return uploadResult;
        }

        [HttpGet("AcceptBank/{id}")]
        public async Task<ActionResult> AcceptBank(int id)
        {
            Bank bank = await _bankService.GetBankByIdAsync(id);
            bank.Status = StatusEnum.Approved;
            _dbContext.SaveChanges();
            return Ok(bank);
        }

        [HttpGet("RejectBank/{id}")]
        public async Task<ActionResult> RejectBank(int id)
        {
            Bank bank = await _bankService.GetBankByIdAsync(id);
            bank.Status = StatusEnum.Rejected;
            _dbContext.SaveChanges();
            return Ok(bank);
        }


        [HttpGet("AcceptClient/{id}")]
        public async Task<ActionResult> OnboardClient(int id)
        {
            Client client = await _clientService.GetClientByIdAsync(id);

            Bank bank = await _bankService.GetBankByIdAsync(1);
            BankAccount bankAccount = new BankAccount() { Balance = 50000000, BlockedFunds = 0, CreatedAt = DateTime.Now };
            bank.BankAccounts.Add(bankAccount);
            client.BankAccount = bankAccount;
            client.Status = StatusEnum.Approved;
            _dbContext.SaveChanges();
            return Ok(client);
        }

        [HttpPost("RejectClient/{id}")]
        public async Task<ActionResult> RejectClient(int id)
        {
            Client client = await _clientService.GetClientByIdAsync(id);
            client.Status = StatusEnum.Rejected;
            client.isActive = false;
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpPost("NewBeneficiaryOutboundClient")]
        public async Task<ActionResult> NewBeneficiaryOutboundClient([FromBody] OutboundBeneficiaryDTO clientDTO)
        {
            Client client = new Client();

            client.CompanyName = clientDTO.FullName;
            client.CreatedAt = DateTime.Now;
            client.isActive = true;

            Bank bank = await _bankService.GetBankByIdAsync(1);

            BankAccount bankAccount = new BankAccount() { Balance = 50000000, BlockedFunds = 0, CreatedAt = DateTime.Now };

            if (bank.BankAccounts != null)
            {
                bank.BankAccounts.Add(bankAccount);
                client.BankAccount = bankAccount;
                client.isBeneficiaryOutbound = true;
                await _clientService.CreateClientAsync(client);
                return Ok(client);
            }
            bank.BankAccounts = new List<BankAccount> { bankAccount };
            client.BankAccount = bankAccount;
            client.isBeneficiaryOutbound = true;
            await _clientService.CreateClientAsync(client);
            return Ok(client);
        }

        [HttpPost("AddBeneficiary")]
        public async Task<ActionResult> AddBeneficiary([FromBody] AddBeneficiaryDTO addBeneficiaryDTO)
        {

            Client client = await _clientService.GetClientByIdAsync(addBeneficiaryDTO.Id);

            if (client == null)
            {
                return BadRequest("Null");
            }

            if (client.BeneficiaryLists != null)
            {
                client.BeneficiaryLists.Add(addBeneficiaryDTO.Ids);
                _dbContext.SaveChanges();
                return Ok("Saved");
            }

            client.BeneficiaryLists = new List<int>() { };
            client.BeneficiaryLists.Add(addBeneficiaryDTO.Ids);
            _dbContext.SaveChanges();

            return Ok("Saved");
        }

    }
}
