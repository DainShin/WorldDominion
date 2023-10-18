using Microsoft.EntityFrameworkCore;

namespace WorldDominion.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Change to be your model(s) and table(s)
        public DbSet<Department> Departments { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}

// DbContext는 DB와 연결해주는 것