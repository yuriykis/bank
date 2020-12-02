using Microsoft.EntityFrameworkCore;
using Users.Service.Models;

namespace Users.Service.Persistance
{
    public class PrimaryContext : DbContext
    {
        public PrimaryContext(DbContextOptions<PrimaryContext> options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            base.OnModelCreating(modelBuilder);
        }
    }
}