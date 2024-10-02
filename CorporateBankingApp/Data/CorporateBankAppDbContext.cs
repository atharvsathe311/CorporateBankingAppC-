using Microsoft.EntityFrameworkCore;

namespace CorporateBankingApp.Data
{
    public class CorporateBankAppDbContext : DbContext
    {

        public CorporateBankAppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }
    }
}
