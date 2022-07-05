using Microsoft.EntityFrameworkCore;
namespace KYCService.Database
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<UserKYC> UserKYC { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
