using System.ComponentModel.DataAnnotations;

namespace Paper_Route.Models
{
    public class ProfileViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public IList<string> Roles { get; set; } = new List<string>();

    }
}
