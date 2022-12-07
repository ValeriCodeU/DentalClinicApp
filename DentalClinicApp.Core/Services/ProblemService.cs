using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.DentalProblems;
using DentalClinicApp.Core.Models.Patients;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DentalClinicApp.Core.Services
{
    public class ProblemService : IProblemService
    {
        private readonly IRepository repo;

        public ProblemService(IRepository _repo)
        {
            repo = _repo;
        }

        public async Task<IEnumerable<ProblemDetailsViewModel>> AllProblemsByPatientIdAsync(int patientId)
        {
            return await repo.AllReadonly<DentalProblem>()
                .Where(p => p.IsActive)
                .Where(p => p.PatientId == patientId)
                .Select(p => new ProblemDetailsViewModel()
                {
                    Id = p.Id,
                    DiseaseName = p.DiseaseName,
                    DentalStatus = p.DentalStatus,
                    DiseaseDescription = p.DiseaseDescription,
                    AlergyDescription = p.AlergyDescription

                }).ToListAsync();
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

        public async Task DeleteAsync(int problemId)
        {
            var problem = await repo.GetByIdAsync<DentalProblem>(problemId);
            problem.IsActive = false;

            await repo.SaveChangesAsync();
        }

        public async Task<ProblemDetailsViewModel> GetProblemByIdAsync(int problemId)
        {
            return await repo.AllReadonly<DentalProblem>()
               .Where(p => p.IsActive)
               .Where(p => p.Id == problemId)
               .Select(p => new ProblemDetailsViewModel()
               {
                   Id = p.Id,
                   DiseaseName = p.DiseaseName,                  
                   DiseaseDescription = p.DiseaseDescription,                 

               }).FirstAsync();
        }

        public async Task<ProblemDetailsViewModel> ProblemDetailsByIdAsync(int id)
        {
            return await repo.AllReadonly<DentalProblem>()
                .Where(p => p.IsActive)
                .Where(p => p.Id == id)
                .Select(p => new ProblemDetailsViewModel()
                {
                    Id = p.Id,
                    DiseaseName = p.DiseaseName,
                    DentalStatus = p.DentalStatus,
                    DiseaseDescription = p.DiseaseDescription,
                    AlergyDescription = p.AlergyDescription

                }).FirstAsync();
        }

        public async Task<bool> ProblemExistsAsync(int id)
        {
            return await repo.AllReadonly<DentalProblem>().AnyAsync(dp => dp.Id == id && dp.IsActive);
        }
    }
}