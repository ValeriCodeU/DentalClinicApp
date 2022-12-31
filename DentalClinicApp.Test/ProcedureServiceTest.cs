using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.DentalProcedures;
using DentalClinicApp.Core.Services;
using DentalClinicApp.Infrastructure.Data;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DentalClinicApp.Test
{
    public class ProcedureServiceTest
    {
        private IRepository repo;
        private IProcedureService procedureService;
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

        public async Task ProcedureExistsAsync_ShouldReturnTrue_WithValidId()
        {
            var repo = new Repository(dbContext);
            procedureService = new ProcedureService(repo);

            await repo.AddAsync(new DentalProcedure()
            {
                Id = 1,
                Name = "Pulling a tooth out",
                Description = "Classic tooth extraction",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                DentistId = 1,
                PatientId = 1,
                Cost = 100,
                IsActive = true,
            });

            await repo.SaveChangesAsync();

            var procedureExist = await procedureService.ProcedureExistsAsync(1);

            Assert.That(procedureExist, Is.True);
        }

        [Test]

        public async Task DeleteProcedureAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            procedureService = new ProcedureService(repo);

            await repo.AddAsync(new DentalProcedure()
            {
                Id = 1,
                Name = "Pulling a tooth out",
                Description = "Classic tooth extraction",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                DentistId = 1,
                PatientId = 1,
                Cost = 100,
                IsActive = true,
            });

            await repo.AddAsync(new DentalProcedure()
            {
                Id = 2,
                Name = "Procedure test",
                Description = "Description",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                DentistId = 1,
                PatientId = 2,
                Cost = 100,
                IsActive = true,
            });

            await repo.SaveChangesAsync();
            var procedure = await repo.GetByIdAsync<DentalProcedure>(2);

            await procedureService.DeleteProcedureAsync(2);

            Assert.That(procedure.IsActive, Is.False);

        }

        [Test]

        public async Task EditProcedureAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            procedureService = new ProcedureService(repo);

            await repo.AddAsync(new DentalProcedure()
            {
                Id = 1,
                Name = "Pulling a tooth out",
                Description = "Classic tooth extraction",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                DentistId = 1,
                PatientId = 1,
                Cost = 100,
                IsActive = true,
            });

            await repo.SaveChangesAsync();

            var model = new ProcedureFormModel()
            {
                Name = "Pulling a tooth out",
                Description = "Classic tooth extraction",
                StartDate = DateTime.Now.ToString(),
                EndDate = DateTime.Now.ToString(),
                PatientId = 1,
                Cost = 200,
            };

            await procedureService.EditProcedureAsync(model, 1);

            var procedure = await repo.GetByIdAsync<DentalProcedure>(1);

            Assert.That(procedure.Cost, Is.EqualTo(200));
        }

        [Test]

        public async Task CreateAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            procedureService = new ProcedureService(repo);

            var model = new ProcedureFormModel()
            {
                Name = "Pulling a tooth out",
                Description = "Classic tooth extraction",
                StartDate = DateTime.Now.ToString(),
                EndDate = DateTime.Now.ToString(),
                PatientId = 1,
                Cost = 200,
            };

            await procedureService.CreateAsync(model, 1);

            var procedure = await repo.GetByIdAsync<DentalProcedure>(1);

            Assert.That(procedure.DentistId, Is.EqualTo(1));
        }


        [TearDown]

        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
