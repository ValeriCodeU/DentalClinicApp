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
    public interface IPatientService
    {
        Task CreatePatientAsync(Guid userId, int dentistId);

        Task<IEnumerable<DentistModel>> GetDentistsAsync();

        Task<int> GetPatientIdAsync(Guid userId);

        Task<bool> IsExistsByIdAsync(Guid userId);

        Task<MyPatientsViewModel> GetMyPatientsAsync(Guid userId);

        Task<PatientDetailsViewModel> PatientDetailsByIdAsync(int id);

        //TODO
        Task<PatientDetailsViewModel> PatientAttendanceDetailsByIdAsync(int id);

        Task<int> GetPersonalDentistIdAsync(int patientId);

        Task<IEnumerable<PatientModel>> GetPatientsAsync(Guid userId);
    }
}