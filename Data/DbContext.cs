using Microsoft.EntityFrameworkCore;
using policy_management.Entities;

namespace policy_management.Data
{
    public class PolicyDbContext : DbContext
    {
        public PolicyDbContext(DbContextOptions<PolicyDbContext> options) : base(options)
        {
        }

        public DbSet<Policy> Policies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PolicyEnrollment> PolicyEnrollments { get; set; }

    }
}