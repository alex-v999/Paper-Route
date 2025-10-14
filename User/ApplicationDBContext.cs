using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Paper_Route.Models;
using Paper_Route.Models.Consultations;
using Paper_Route.Models.Documents;

namespace Paper_Route.User
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<WorkerProfile> WorkerProfiles { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<ConsultationType> ConsultationTypes { get; set; }
        public DbSet<AvailableSlot> AvailableSlots { get; set; } 
        public DbSet<SlotBlock> SlotBlocks { get; set; }

        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentCase> DocumentCases { get; set; }
        public DbSet<DocumentCaseFile> DocumentCaseFiles { get; set; }
        public DbSet<DocumentTypeEntity> DocumentTypes { get; set; }
        public DbSet<DocumentRequestType> DocumentRequestTypes { get; set; }

    }
}
