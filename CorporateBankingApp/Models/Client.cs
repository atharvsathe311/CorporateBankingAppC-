
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CorporateBankingApp.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string CompanyName { get; set; }
        public string? FounderName { get; set; }
        public string? Address { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? CompanyEmail { get; set; }
        public string? CompanyPhone { get; set; }
        public UserLogin? UserLogin { get; set; }
        public ClientKyc? ClientKyc { get; set; }
        public DateTime CreatedAt { get; set; }
        public StatusEnum? Status { get; set; }
        public BankAccount? BankAccount { get; set; }
        public List<BeneficiaryList>? BeneficiaryLists { get; set; }
        public bool isActive { get; set; }
        public bool isBeneficiaryOutbound { get; set; } = false;
    }
}
