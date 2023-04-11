using DentalClinicApp.Core.Models.Dentists;
using DentalClinicApp.Core.Models.Patients;
using DentalClinicApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Contracts
{
    /// <summary>
    ///  Manipulates patient data
    /// </summary>
    public interface IPatientService
    {
        /// <summary>
        /// Create a user as a patient
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dentistId"></param>
        /// <returns></returns>
        Task CreatePatientAsync(Guid userId, int dentistId);

        /// <summary>
        /// Get all dentists in the system
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of dentists</returns> 
        Task<IEnumerable<DentistModel>> GetDentistsAsync();

        /// <summary>
        /// Get a patient Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Integer Id for a patient</returns>
        Task<int> GetPatientIdAsync(Guid userId);

        /// <summary>
        /// Check if the patient exists
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Boolean data type if patient exists</returns>
        Task<bool> IsExistsByIdAsync(Guid userId);

        Task<MyPatientsViewModel> GetMyPatientsAsync(Guid userId);

        Task<PatientDetailsViewModel> PatientDetailsByIdAsync(int id);

        Task<PatientDetailsViewModel> PatientAttendanceDetailsByIdAsync(int id);

        Task<PatientDetailsViewModel> PatientProcedureDetailsByIdAsync(int id);

        Task<int> GetPersonalDentistIdAsync(int patientId);

        Task<IEnumerable<PatientModel>> GetPatientsAsync(Guid userId);

        Task<Guid> GetUserIdByPatientId(int id);
    }
}