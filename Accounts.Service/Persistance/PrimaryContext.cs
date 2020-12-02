using Accounts.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Service.Persistance
{
    public class PrimaryContext : DbContext
    {
        public PrimaryContext(DbContextOptions<PrimaryContext> options) : base(options)
        {
        }
        
        public DbSet<Account> Accounts { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("Accounts");
            base.OnModelCreating(modelBuilder);
        }
    }
}