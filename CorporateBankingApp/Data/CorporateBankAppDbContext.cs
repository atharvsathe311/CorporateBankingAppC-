using CorporateBankingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CorporateBankingApp.Data
{
    public class CorporateBankAppDbContext : DbContext
    {
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<SuperAdmin> SuperAdmins { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public CorporateBankAppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }
    }
}
