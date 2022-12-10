using DentalClinicApp.Core.Models.Dentists;
using System.ComponentModel.DataAnnotations;

namespace DentalClinicApp.Core.Models.Appointments
{
    public class AppointmentFormModel
    {
        public int Id { get; set; }

        [Display(Name = "Select a date")]

        public DateTime StartDate { get; set; } = DateTime.Today;

        public string? Details { get; set; }

        public bool Status { get; set; }

        //public PatientServiceModel Patient { get; set; }
       
        //public IEnumerable<DentistModel> Dentists { get; set; } = new List<DentistModel>();
    }
}
