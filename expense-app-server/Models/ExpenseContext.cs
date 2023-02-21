using Microsoft.EntityFrameworkCore;

namespace expense_app_server.Models
{
   
    public class ExpenseContext : DbContext
    {
        public ExpenseContext(DbContextOptions<ExpenseContext> options) :
                base(options)
        {

        }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data. Auto-increment PK’s must 
            // still be specified in seed data.
            modelBuilder.Entity<User>().HasData(
                new
                {
                    Id = 1
                    ,
                    Username = "Brayden"
                    ,
                    Password = "1234"
                    ,
                    Email = "test"
                    ,
                    ExternalId = "string"
                    ,
                    ExternalType = "string"
                }
            );
        }


    }
    
}
