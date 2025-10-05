using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Paper_Route.Models
{
    public class UserClaim
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        // Paths to images, separated by ';'
        public string? Attachments { get; set; }

        [BindNever]
        public string UserId { get; set; }

    }
}
