using DentalClinicApp.Core.Models.Dentists;
using System.ComponentModel.DataAnnotations;

namespace DentalClinicApp.Core.Models.Appointments
{
    public class AppointmentFormModel
    {
        public int Id { get; set; }

        [Display(Name = "Select date and time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]

        public DateTime StartDate { get; set; } = DateTime.Now;

        public string? Details { get; set; }

        public bool Status { get; set; }
        
    }
}
