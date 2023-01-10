using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Attendances;
using DentalClinicApp.Core.Services;
using DentalClinicApp.Infrastructure.Data;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DentalClinicApp.Test
{
    public class AttendanceServiceTest
	{
        private IRepository repo;
        private IAttendanceService attendanceService;
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
            attendanceService = new AttendanceService(repo);

            await repo.AddAsync(new Attendance()
            {
                Id = 3,
                ClinicRemarks = "No Problem",
                IsActive = true,
                Diagnosis = "No diagnosis",
                PatientId = 1,
                DentistId = 1,
                Date = DateTime.Now,
            });
            await repo.SaveChangesAsync();

            var problemExist = await attendanceService.AttendanceExistsAsync(3);

            Assert.That(problemExist, Is.True);
        }

        [Test]

        public async Task CreateAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            attendanceService = new AttendanceService(repo);

            var model = new AttendanceFormModel()
            {
                ClinicRemarks = "You need a root canal and a crown",
                Diagnosis = "Fractured tooth",
                PatientId = 1,
            };

            var result = await attendanceService.CreateAsync(model, 1);

            var attendance = await repo.GetByIdAsync<Attendance>(1);

            Assert.That(result, Is.EqualTo(1));
            Assert.That(attendance.DentistId, Is.EqualTo(1));
            Assert.That(attendance.PatientId, Is.EqualTo(1));
        }

        [TearDown]

        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
