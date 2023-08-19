using DentalClinicApp.Core.Models.Appointments;
using DentalClinicApp.Core.Models.Appointments.Enums;

namespace DentalClinicApp.Core.Contracts
{
    /// <summary>
    /// Manipulates appointment data
    /// </summary>
    public interface IAppointmentService
    {
        /// <summary>
        /// Create a new appointment
        /// </summary>
        /// <param name="model">Appointment form model</param>
        /// <param name="patientId">Patient identifier</param>
        /// <param name="dentistId">Dentist identifier</param>       
        Task CreateAsync(AppointmentFormModel model, int patientId, int dentistId);

        /// <summary>
        /// Get appointments for dentist
        /// </summary>
        /// <param name="userId">User globally unique identifier</param>
        /// <returns>Data query model for list of appointments</returns>
        Task<AppointmentQueryServiceModel> GetAppointmentsAsync(
            int clientId,
            bool isPatient,
            AppointmentSorting sorting = AppointmentSorting.Newest,
            string? status = null,
            string? searchTerm = null,
            int currentPage = 1,
            int appointmentsPerPage = 1
            );

        /// <summary>
        /// Get apppointment by identifier
        /// </summary>
        /// <param name="id">Appointment identifier</param>
        /// <returns>Get appointment data</returns>
        Task<AppointmentServiceModel> GetAppointmentByIdAsync(int id);

        /// <summary>
        ///  Аccept appointment
        /// </summary>
        /// <param name="id">Appointment identifier</param>
        /// <returns>Boolean data type if appointment is accepted</returns>       
        Task<bool> AcceptAppointmentByIdAsync(int id);

        /// <summary>
        /// Postpone appointment
        /// </summary>
        /// <param name="id">Appointment identifier</param>
        /// <returns>Boolean data type if appointment is postponed</returns>
        Task<bool> PostponeAppointmentByIdAsync(int id);       
    }
}
