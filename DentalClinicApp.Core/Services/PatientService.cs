using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.DentalProblems;
using DentalClinicApp.Core.Models.Dentists;
using DentalClinicApp.Core.Models.Patients;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;
using DentalClinicApp.Infrastructure.Data.Identity;
using Microsoft.EntityFrameworkCore;

namespace DentalClinicApp.Core.Services
{
    public class PatientService : IPatientService
    {
        private readonly IRepository repo;

        public PatientService(IRepository _repo)
        {
            repo = _repo;
        }

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

        public async Task<IEnumerable<DentistModel>> GetDentistAsync()
        {

            return await repo.AllReadonly<Dentist>().Select(d => new DentistModel()
            {
                Id = d.Id,
                Name = $"{d.User.FirstName} {d.User.LastName}"
            }).ToListAsync();
        }

        public async Task<MyPatientsViewModel> GetMyPatients(Guid userId)
        {

            //var patients = await repo.AllReadonly<Patient>()
            //    .Where(d => d.DentistId == dentist.Id)
            //    .Select(p => new PatientServiceModel()
            //    {
            //        FirstName = p.User.FirstName,
            //        LastName = p.User.LastName,
            //        Email = p.User.Email,
            //        PhoneNumber = p.User.PhoneNumber

            //    }).ToListAsync();


            var patients = await repo.AllReadonly<Dentist>()
                .Where(d => d.UserId == userId)
                .Select(b => new MyPatientsViewModel()
                {
                    Patients = b.Patients.Select(p => new PatientServiceModel
                    {
                        Id = p.Id,
                        FirstName = p.User.FirstName,
                        LastName = p.User.LastName,
                        PhoneNumber = p.User.PhoneNumber,
                        Email = p.User.Email,
                        //PatientProblems = p.DentalProblems.Select(dp => new ProblemDetailsViewModel()
                        //{
                        //    DiseaseName = dp.DiseaseName,
                        //    DiseaseDescription = dp.DiseaseDescription,
                        //    DentalStatus = dp.DentalStatus,
                        //    AlergyDescription = dp.AlergyDescription
                        //}).ToList()
                    })

                }).FirstAsync();

            return patients;

        }

        public async Task<int> GetPatientIdAsync(Guid userId)
        {
            var patient = await repo.AllReadonly<Patient>().FirstAsync(u => u.UserId == userId);

            return patient.Id;
        }

        public async Task<bool> IsExistsByIdAsync(Guid userId)
        {
            return await repo.AllReadonly<Patient>().AnyAsync(p => p.UserId == userId);
        }

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
                    }).ToList()

                }).FirstAsync();

            //return await repo
            //    .AllReadonly<DentalProblem>()
            //    .Where(dp => dp.IsActive)
            //    .Where(dp => dp.Id == id)
            //    .Select(dp => new PatientDetailsViewModel()
            //    {
            //        Id = dp.Id,
            //        DiseaseDescription = dp.DiseaseDescription,
            //        DiseaseName = dp.DiseaseName,
            //        DentalStatus = dp.DentalStatus,
            //        AlergyDescription = dp.AlergyDescription,
            //        //Patient = new PatientServiceModel()
            //        //{
            //        //    FirstName = dp.Patient.User.FirstName,
            //        //    LastName = dp.Patient.User.LastName,
            //        //    Email = dp.Patient.User.Email,
            //        //    PhoneNumber = dp.Patient.User.PhoneNumber
            //        //}
            //    }).FirstAsync();

        }
    }
}