using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace CorporateBankingApp.Models
{
    public class Bank
    {
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string BankIFSCCode { get; set; }
        public string BankAddress { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string BankEmail { get; set; }
        public BankKyc? BankKyc { get; set; }
        public List<Client>? ClientList { get; set; }
        public List<BankAccount>? BankAccounts { get; set; }
        public UserLogin UserLogin { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool isActive { get; set; }


    }
}
