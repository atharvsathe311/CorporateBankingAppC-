﻿using CorporateBankingApp.Models;

namespace CorporateBankingApp.DTO
{
    public class OutboundBeneficiaryDTO
    {
        public int ClientId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
    }

}
