using coding_test.Data.Entities;
using Microsoft.EntityFrameworkCore; 

namespace coding_test.Data
{
    public class BankDBContext : DbContext
    {
        public BankDBContext(DbContextOptions<BankDBContext> options)
            : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasMany<Account>(g => g.Accounts);

            modelBuilder.Entity<Account>()
                .HasOne<Customer>(g => g.Customer);

            modelBuilder.Entity<Account>()
                .HasMany<Transaction>(g => g.Transactions);

            modelBuilder.Entity<Transaction>()
              .HasOne<Account>(g => g.Account);
        }


    }
}
