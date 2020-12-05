using Microsoft.EntityFrameworkCore;
using Transactions.Service.Models;

namespace Transactions.Service.Persistence
{
    public class PrimaryContext : DbContext
    {
        public PrimaryContext(DbContextOptions<PrimaryContext> options) : base(options)
        {
        }
        
        public DbSet<Transaction> Transactions { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>().ToTable("Transactions");
            base.OnModelCreating(modelBuilder);
        }
    }
}