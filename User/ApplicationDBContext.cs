using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Paper_Route.Models;
using Paper_Route.Models.Consultations;

namespace Paper_Route.User
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<DocumentCase> DocumentCases { get; set; }
        public DbSet<WorkerProfile> WorkerProfiles { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<ConsultationType> ConsultationTypes { get; set; }
        public DbSet<AvailableSlot> AvailableSlots { get; set; } 
        public DbSet<SlotBlock> SlotBlocks { get; set; }
    }
}
