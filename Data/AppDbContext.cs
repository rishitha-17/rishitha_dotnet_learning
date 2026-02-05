using Microsoft.EntityFrameworkCore;
using webapi_practice.Models.Entites;

namespace webapi_practice.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}