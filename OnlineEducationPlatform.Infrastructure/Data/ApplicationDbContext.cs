using Microsoft.EntityFrameworkCore;
using OnlineEducationPlatform.Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace OnlineEducationPlatform.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
        }
    }
}
