using DentalClinicApp.Core.Contracts;
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


        [TearDown]

        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
