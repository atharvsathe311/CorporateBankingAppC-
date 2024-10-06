using CorporateBankingApp.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateBankingApp.Models
{
    public class BankAccount
    {
        //[Key]
        //public int AccountId { get; set; }

        //[ForeignKey("BankId")]
        //public int BankId { get; set; }
        //public double Balance { get; set; }
        //public double BlockedFunds { get; set; }
        //public DateTime CreatedAt { get; set; }

        [Key]
        public int AccountId { get; set; }

        [ForeignKey("BankId")]
        public int BankId { get; set; }

        [Precision(18, 2)]
        public decimal Balance { get; set; }
        [Precision(18, 2)]

        public decimal BlockedFunds { get; set; }

        [Precision(18, 2)]
        public decimal SalaryPayments { get; set; }
        
        [Precision(18, 2)]
        public decimal AccountPayables { get; set; }

        [Precision(18, 2)]
        public decimal Taxes { get; set; }
        
        [Precision(18, 2)]
        public decimal EmergencyFunds { get; set; }
        
        [Precision(18, 2)]
        public decimal InvestmentsAndGrowth { get; set; }

        [Precision(18, 2)]
        public decimal SalaryPercentage { get; set; } = 35;

        [Precision(18, 2)]
        public decimal AccountPayablesPercentage { get; set; } = 20;

        [Precision(18, 2)]
        public decimal TaxesPercentage { get; set; } = 30;

        [Precision(18, 2)]
        public decimal EmergencyFundsPercentage { get; set; } = 5;

        [Precision(18, 2)]
        public decimal InvestmentsAndGrowthPercentage { get; set; } = 10;
        public DateTime CreatedAt { get; set; }
        public BankAccount(decimal balance)
        {
            Balance = balance;
            BlockedFunds = 0m;
            InitializeFundAllocations();
            CreatedAt = DateTime.Now;
        }
        private void InitializeFundAllocations()
        {
            SalaryPayments = Balance * (SalaryPercentage / 100);
            AccountPayables = Balance * (AccountPayablesPercentage / 100);
            Taxes = Balance * (TaxesPercentage / 100);
            EmergencyFunds = Balance * (EmergencyFundsPercentage / 100);
            InvestmentsAndGrowth = Balance * (InvestmentsAndGrowthPercentage / 100);
        }

        public void AddFunds(decimal amount)
        {
            Balance += amount;
            SalaryPayments += amount * (SalaryPercentage / 100);
            AccountPayables += amount * (AccountPayablesPercentage / 100);
            Taxes += amount * (TaxesPercentage / 100);
            EmergencyFunds += amount * (EmergencyFundsPercentage / 100);
            InvestmentsAndGrowth += amount * (InvestmentsAndGrowthPercentage / 100);
          
        }
        public void UpdateSplitPercentages(decimal salaryPercentage, decimal accountPayablesPercentage, decimal taxesPercentage, decimal emergencyFundsPercentage, decimal investmentsPercentage)
        {
            SalaryPercentage = salaryPercentage;
            AccountPayablesPercentage = accountPayablesPercentage;
            TaxesPercentage = taxesPercentage;
            EmergencyFundsPercentage = emergencyFundsPercentage;
            InvestmentsAndGrowthPercentage = investmentsPercentage;

            InitializeFundAllocations();
        }


    }
}
