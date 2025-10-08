using Paper_Route.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paper_Route.User
{
    public class WorkerProfile
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public string Specialization { get; set; } = string.Empty;
        public int MaxAppointmentsPerDay { get; set; } = 5;

    }

}