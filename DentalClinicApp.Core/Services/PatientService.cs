using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Attendances;
using DentalClinicApp.Core.Models.DentalProblems;
using DentalClinicApp.Core.Models.Dentists;
using DentalClinicApp.Core.Models.Patients;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DentalClinicApp.Core.Services
{
    /// <summary>
    ///  Manipulates dentist data
    /// </summary>
    public class PatientService : IPatientService
    {
        private readonly IRepository repo;

        public PatientService(IRepository _repo)
        {
            repo = _repo;
        }

        /// <summary>
        /// Create a user as a patient
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dentistId"></param>
        /// <returns></returns>
        public async Task CreatePatientAsync(Guid userId, int dentistId)
        {
            var patient = new Patient()
            {
                UserId = userId,
                DentistId = dentistId
            };

            await repo.AddAsync<Patient>(patient);
            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Get all dentists in the system
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of dentists</returns> 
        public async Task<IEnumerable<DentistModel>> GetDentistsAsync()
        {

            return await repo.AllReadonly<Dentist>()
                .Select(d => new DentistModel()
                {
                    Id = d.Id,
                    Name = $"{d.User.FirstName} {d.User.LastName}"
                }).ToListAsync();
        }

        public async Task<IEnumerable<PatientModel>> GetPatientsAsync(Guid userId)
        {

            var patients = await repo.AllReadonly<Dentist>()
                .Where(d => d.User.Id == userId)
                .Where(d => d.User.IsActive)
                .Select(d => d.Patients
                .Select(p => new PatientModel()
                {
                    Id = p.Id,
                    Name = $"{p.User.FirstName} {p.User.LastName}"
                })
                .ToList())
                .FirstAsync();

            return patients;
        }

        /// <summary>
        /// Get the dentist's patients
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Data view model for list of patients</returns>
        public async Task<MyPatientsViewModel> GetMyPatientsAsync(Guid userId)
        {
            var patients = await repo.AllReadonly<Dentist>()
                .Where(d => d.UserId == userId && d.User.IsActive)                
                .Select(b => new MyPatientsViewModel()
                {
                    Patients = b.Patients
                    .Where(p => p.User.IsActive)
                    .Select(p => new PatientServiceModel
                    {
                        Id = p.Id,
                        FirstName = p.User.FirstName,
                        LastName = p.User.LastName,
                        PhoneNumber = p.User.PhoneNumber,
                        Email = p.User.Email,
                    })

                }).FirstAsync();

            return patients;

        }

        /// <summary>
        /// Get a patient identifier
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Integer Id for a patient</returns>
        public async Task<int> GetPatientIdAsync(Guid userId)
        {
            var patient = await repo.AllReadonly<Patient>().FirstAsync(u => u.UserId == userId);

            return patient.Id;
        }

        public async Task<int> GetPersonalDentistIdAsync(int patientId)
        {
            var result = await repo.GetByIdAsync<Patient>(patientId);

            return result.DentistId;
        }

        /// <summary>
        /// Check if the user exists as a patient
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Boolean data type if the user exists as a patient</returns>
        public async Task<bool> IsExistsByIdAsync(Guid userId)
        {
            return await repo.AllReadonly<Patient>().AnyAsync(p => p.UserId == userId && p.User.IsActive);
        }

        /// <summary>
        /// Get data for the patient's dental problems
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Data view model for lists of patient data</returns>
        public async Task<PatientDetailsViewModel> PatientDetailsByIdAsync(int id)
        {

            return await repo
                .AllReadonly<Patient>()
                .Where(p => p.User.IsActive)
                .Where(p => p.Id == id)
                .Select(p => new PatientDetailsViewModel()
                {

                    FirstName = p.User.FirstName,
                    LastName = p.User.LastName,
                    Email = p.User.Email,
                    PhoneNumber = p.User.PhoneNumber,
                    PatientProblems = p.DentalProblems
                    .Where(dp => dp.IsActive)
                    .Select(dp => new ProblemDetailsViewModel()
                    {
                        DiseaseName = dp.DiseaseName,
                        DiseaseDescription = dp.DiseaseDescription,
                        DentalStatus = dp.DentalStatus,
                        AlergyDescription = dp.AlergyDescription
                    })

                }).FirstAsync();

        }

        public async Task<PatientDetailsViewModel> PatientAttendanceDetailsByIdAsync(int id)
        {
            return await repo
               .AllReadonly<Patient>()
               .Where(p => p.User.IsActive)
               .Where(p => p.Id == id)
               .Select(p => new PatientDetailsViewModel()
               {

                   FirstName = p.User.FirstName,
                   LastName = p.User.LastName,
                   Email = p.User.Email,
                   PhoneNumber = p.User.PhoneNumber,
                   PatientAttendances = p.Attendances
                   .Where(pa => pa.IsActive)
                   .Select(pa => new AttedanceServiceModel()
                   {
                       Id = pa.Id,
                       ClinicRemarks = pa.ClinicRemarks,
                       Diagnosis = pa.Diagnosis,
                       Date = pa.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)
                   })

               }).FirstAsync();
        }

        public async Task<PatientDetailsViewModel> PatientProcedureDetailsByIdAsync(int id)
        {
            return await repo
                .AllReadonly<Patient>()
                .Where(p => p.User.IsActive && p.Id == id)
                .Select(p => new PatientDetailsViewModel()
                {
                    FirstName = p.User.FirstName,
                    LastName = p.User.LastName,
                    Email = p.User.Email,
                    PhoneNumber = p.User.PhoneNumber,
                    PatientProcedures = p.DentalProcedures
                    .Where(pp => pp.IsActive)
                    .Select(pp => new Models.DentalProcedures.ProcedureServiceModel()
                    {
                        Id = pp.Id,
                        Name = pp.Name,
                        Description = pp.Description,
                        StartDate = pp.StartDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                        EndDate = pp.EndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                        Cost = decimal.Parse(pp.Cost.ToString("F2")),
                    })


                }).FirstAsync();
        }

        public async Task<Guid> GetUserIdByPatientId(int id)
        {
            var patient = await repo.GetByIdAsync<Patient>(id);

            return patient.UserId;
        }
    }
}