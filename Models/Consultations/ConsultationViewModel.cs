using System;
using System.ComponentModel.DataAnnotations;

namespace Paper_Route.Models.Consultations
{
    public class ConsultationViewModel
    {
        [Required]
        [Display(Name = "Type")]
        public int ConsultationTypeID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ScheduledDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan ScheduledTime { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
