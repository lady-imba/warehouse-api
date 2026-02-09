using Microsoft.EntityFrameworkCore;
using Warehouse.API.Models;

namespace Warehouse.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Roll> Rolls { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Roll>()
                .HasIndex(r => r.AddedDate);
                
            modelBuilder.Entity<Roll>()
                .HasIndex(r => r.RemovedDate);
                
            modelBuilder.Entity<Roll>()
                .HasIndex(r => r.Length);
                
            modelBuilder.Entity<Roll>()
                .HasIndex(r => r.Weight);
        }
    }
}