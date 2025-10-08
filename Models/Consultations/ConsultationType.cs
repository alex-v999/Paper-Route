using System.ComponentModel.DataAnnotations;

namespace Paper_Route.Models.Consultations
{
    public class ConsultationType
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public int DefaultDurationMinutes { get; set; } = 60;
        public string Description { get; internal set; }
    }
}
