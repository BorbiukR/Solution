using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Solution.DAL.Entities;

namespace Solution.DAL
{
    public class SolulitonContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<TransactionInfo> TransactionInfos { get; set; }

        public SolulitonContext(DbContextOptions<SolulitonContext> options): base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}