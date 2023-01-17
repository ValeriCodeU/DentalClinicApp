using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.DentalProblems;
using DentalClinicApp.Core.Services;
using DentalClinicApp.Infrastructure.Data;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using NuGet.Frameworks;

namespace DentalClinicApp.Test
{
    public class ProblemServiceTest
    {
        private IRepository repo;
        private IProblemService problemService;
        private DentalClinicDbContext dbContext;

        [SetUp]

        public void Setup()
        {
            var contextOptions = new DbContextOptionsBuilder<DentalClinicDbContext>()
                .UseInMemoryDatabase("DentalDb")
                .Options;

            dbContext = new DentalClinicDbContext(contextOptions);

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }

        [Test]

        public async Task ProblemExistsAsync_ShouldReturnTrue_WithValidId()
        {
            var repo = new Repository(dbContext);
            problemService = new ProblemService(repo);

            await repo.AddAsync(new DentalProblem()
            {
                Id = 1,
                DiseaseName = "Cracked tooth",
                DiseaseDescription = "Playing boxing without a mouth guard",
                AlergyDescription = "drug allergy",
                DentalStatus = "55",
                PatientId = 1,
                IsActive = true,
            });
            await repo.SaveChangesAsync();

            var problemExist = await problemService.ProblemExistsAsync(1);

            Assert.That(problemExist, Is.True);
        }

        [Test]

        public async Task CreateAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            problemService = new ProblemService(repo);


            var model = new ProblemFormModel()
            {
                DiseaseName = "Cracked tooth",
                DiseaseDescription = "Playing boxing without a mouth guard",
                AlergyDescription = "drug allergy",
                DentalStatus = "55",
            };

            await problemService.CreateAsync(3, model);
            var problem = await repo.GetByIdAsync<DentalProblem>(1);

            Assert.That(problem.Id, Is.EqualTo(1));
            Assert.That(problem.DentalStatus, Is.EqualTo("55"));
            Assert.That(problem.IsActive, Is.True);
            Assert.That(problem.PatientId, Is.EqualTo(3));
            Assert.That(problem.AlergyDescription, Is.EqualTo("drug allergy"));
            Assert.That(problem.DiseaseDescription, Is.EqualTo("Playing boxing without a mouth guard"));
        }

        [Test]

        public async Task DeleteAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            problemService = new ProblemService(repo);

            await repo.AddAsync(new DentalProblem()
            {
                DiseaseName = "Cracked tooth",
                DiseaseDescription = "Playing boxing without a mouth guard",
                AlergyDescription = "drug allergy",
                DentalStatus = "55",
                PatientId = 1,
                IsActive = true,
            });

            await repo.SaveChangesAsync();

            var problem = await repo.GetByIdAsync<DentalProblem>(1);

            var result = await problemService.DeleteAsync(1);

            Assert.That(problem.IsActive, Is.False);
            Assert.That(result, Is.True);
        }

        [Test]

        public async Task GetProblemByIdAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            problemService = new ProblemService(repo);

            await repo.AddAsync(new DentalProblem()
            {
                DiseaseName = "Cracked tooth",
                DiseaseDescription = "Playing boxing without a mouth guard",
                AlergyDescription = "drug allergy",
                DentalStatus = "55",
                PatientId = 1,
                IsActive = true,
            });

            await repo.SaveChangesAsync();

            var problem = await problemService.GetProblemByIdAsync(1);


            Assert.That(problem, Is.Not.Null);
            Assert.That(problem.Id, Is.EqualTo(1));
        }

        [Test]

        public async Task EditProblemAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            problemService = new ProblemService(repo);

            await repo.AddAsync(new DentalProblem()
            {
                DiseaseName = "Cracked tooth",
                DiseaseDescription = "Playing boxing without a mouth guard",
                AlergyDescription = "drug allergy",
                DentalStatus = "55",
                PatientId = 1,
                IsActive = true,
            });

            await repo.SaveChangesAsync();

            var model = new ProblemFormModel()
            {
                DiseaseName = "Sensitive to cold",
                DiseaseDescription = "Pain when consuming cold drinks",
                AlergyDescription = null,
                DentalStatus = "45",                
            };

            await problemService.EditProblemAsync(model, 1);

            var problem = await repo.GetByIdAsync<DentalProblem>(1);

            Assert.That(problem.DentalStatus, Is.EqualTo("45"));
            Assert.That(problem.DiseaseName, Is.EqualTo("Sensitive to cold"));
            Assert.That(problem.DiseaseDescription, Is.EqualTo("Pain when consuming cold drinks"));
            Assert.That(problem.AlergyDescription, Is.Null);
        }

        [Test]

        public async Task ProblemDetailsByIdAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            problemService = new ProblemService(repo);
          

            await repo.AddAsync(new DentalProblem()
            {
                DiseaseName = "Sensitive to cold",
                DiseaseDescription = "Pain when consuming cold drinks",
                AlergyDescription = null,
                DentalStatus = "45",
                PatientId = 1,
                IsActive = true,
            });


            await repo.SaveChangesAsync();

            var problemDetails = await problemService.ProblemDetailsByIdAsync(1);

            Assert.That(problemDetails.DentalStatus, Is.EqualTo("45"));
            Assert.That(problemDetails.Id, Is.EqualTo(1));
            Assert.That(problemDetails.DiseaseName, Is.EqualTo("Sensitive to cold"));
        }


        [TearDown]

        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
