using GroceryStoreAPI.Entity.Domain;
using Microsoft.EntityFrameworkCore;

namespace GroceryStoreAPI.Entity
{
    public class GroceryStoreDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=GroceryStoreDB.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customers");

            modelBuilder.Entity<Customer>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
        }
    }
}