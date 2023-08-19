namespace DentalClinicApp.Core.Models.Appointments
{
    public class AppointmentQueryServiceModel
    {
        public int TotalAppointmentsCount { get; set; }

        public IEnumerable<AppointmentServiceModel> Appointments { get; set; } = new List<AppointmentServiceModel>();
    }
}
