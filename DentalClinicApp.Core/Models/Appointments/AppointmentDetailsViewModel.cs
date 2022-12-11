using DentalClinicApp.Core.Models.Patients;

namespace DentalClinicApp.Core.Models.Appointments
{
    public class AppointmentDetailsViewModel
    {       

        public IEnumerable<AppointmentServiceModel> Appointments { get; set; } = new List<AppointmentServiceModel>();

    }
}
