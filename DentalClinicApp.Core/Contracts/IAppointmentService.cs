using DentalClinicApp.Core.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="patientId">Patient Id</param>
        /// <param name="dentistId">Dentist Id</param>
        /// <returns></returns>
        Task CreateAsync(AppointmentFormModel model, int patientId, int dentistId);

        /// <summary>
        /// Get appointments for dentist
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List of appointments</returns>
        Task<AppointmentDetailsViewModel> GetDentistAppointments(Guid userId);

        /// <summary>
        /// Get apppointment by Id
        /// </summary>
        /// <param name="id">Appointment Id</param>
        /// <returns>Get appointment data</returns>
        Task<AppointmentServiceModel> GetAppointmentByIdAsync(int id);

        /// <summary>
        ///  Аccept appointment
        /// </summary>
        /// <param name="id">Appointment Id</param>
        /// <returns>Boolean data type if appointment is accepted</returns>       
        Task<bool> AcceptAppointmentByIdAsync(int id);
        
        /// <summary>
        /// Postpone appointment
        /// </summary>
        /// <param name="id">Appointment Id</param>
        /// <returns>Boolean data type if appointment is postponed</returns>
        Task<bool> PostponeAppointmentByIdAsync(int id);

        /// <summary>
        /// Get appointments for patient
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List of appointments</returns>
        Task<AppointmentDetailsViewModel> GetPatientAppointments(Guid userId);
    }
}
