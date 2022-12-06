using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.DentalProblems;
using DentalClinicApp.Core.Models.Patients;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;

namespace DentalClinicApp.Core.Services
{
    public class ProblemService : IProblemService
    {
        private readonly IRepository repo;

        public ProblemService(IRepository _repo)
        {
            repo = _repo;
        }

        public async Task CreateAsync(int patientId, ProblemFormModel model)
        {


            var problem = new DentalProblem()
            {
                DiseaseName = model.DiseaseName,
                DiseaseDescription = model.DiseaseDescription,
                AlergyDescription = model.AlergyDescription,
                DentalStatus = model.DentalStatus,
                PatientId = patientId,
            };

            await repo.AddAsync<DentalProblem>(problem);
            await repo.SaveChangesAsync();
        }

        //public async Task<PatientServiceModel> GetPatientInfo(Guid userId)
        //{

        //}
        //public async Task<PatientServiceModel> GetPatientInfo(Guid userId)
        //{
        //    var user = await repo.GetByIdAsync<ApplicationUser>(userId);



        //    var model = new PatientServiceModel()
        //    {
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        PhoneNumber = user.PhoneNumber,
        //        Email = user.Email,
        //    };

        //    return model;


        //}
    }
}
