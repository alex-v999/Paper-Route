using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Paper_Route.User;

namespace Paper_Route.Models.Consultations
{
    public class Consultation
    {
        public int Id { get; set; }

        [Required]
        public int ConsultationTypeID { get; set; }

        [ForeignKey(nameof(ConsultationTypeID))]
        public ConsultationType Type { get; set; } = null!;

        [Required]
        public DateTime ScheduledDateTime { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        // Not required for client input; set server-side
        public string? UserId { get; set; }

        [BindNever]
        public ApplicationUser? ApplicationUser { get; set; }

        // Assigned later, so nullable
        public int? WorkerProfileID { get; set; }

        [BindNever]
        public WorkerProfile? WorkerProfile { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
