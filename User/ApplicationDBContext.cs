using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Paper_Route.User
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<DocumentCase> DocumentCases { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<WorkerProfile> WorkerProfiles { get; set; }
    }
}
