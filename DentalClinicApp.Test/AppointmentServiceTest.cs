using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Appointments;
using DentalClinicApp.Core.Services;
using DentalClinicApp.Infrastructure.Data;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DentalClinicApp.Test
{
    public class AppointmentServiceTest
	{
        private IRepository repo;
        private IAppointmentService appointmentService;
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

        public async Task AcceptAppointmentByIdAsync_ShouldReturnTrue_WithValidId()
        {
            var repo = new Repository(dbContext);
            appointmentService = new AppointmentService(repo);

            await repo.AddAsync(new Appointment()
            {
                Id = 1,
                StartDateTime = DateTime.ParseExact("23/12/2022 14:00:00", "dd/MM/yyyy HH:mm:ss", null),
                PatientId = 1,
                DentistId = 1,
                IsActive = true,
                Status = true
            });

            await repo.SaveChangesAsync();

            var appointmentAccepted = await appointmentService.AcceptAppointmentByIdAsync(1);

            Assert.That(appointmentAccepted, Is.True);
        }

        [Test]

        public async Task PostponeAppointmentByIdAsync_ShouldReturnTrue_WithValidId()
        {
            var repo = new Repository(dbContext);
            appointmentService = new AppointmentService(repo);

            await repo.AddAsync(new Appointment()
            {
                Id = 1,
                StartDateTime = DateTime.ParseExact("23/12/2022 14:00:00", "dd/MM/yyyy HH:mm:ss", null),
                PatientId = 1,
                DentistId = 1,
                IsActive = false,
                Status = false
            });

            await repo.SaveChangesAsync();

            var appointmentPostponed = await appointmentService.PostponeAppointmentByIdAsync(1);

            Assert.That(appointmentPostponed, Is.True);
        }


        [Test]

        public async Task AcceptAppointmentByIdAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            appointmentService = new AppointmentService(repo);

            await repo.AddAsync(new Appointment()
            {
                Id = 1,
                StartDateTime = DateTime.ParseExact("23/12/2022 14:00:00", "dd/MM/yyyy HH:mm:ss", null),
                PatientId = 1,
                DentistId = 1,
                IsActive = true,
                Status = true
            });

            await repo.SaveChangesAsync();

            var appointment = await repo.GetByIdAsync<Appointment>(1);

            Assert.That(appointment.Status, Is.True);
            Assert.That(appointment.IsActive, Is.True);
        }

        [Test]

        public async Task PostponeAppointmentByIdAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            appointmentService = new AppointmentService(repo);

            await repo.AddAsync(new Appointment()
            {
                Id = 1,
                StartDateTime = DateTime.ParseExact("23/12/2022 14:00:00", "dd/MM/yyyy HH:mm:ss", null),
                PatientId = 1,
                DentistId = 1,
                IsActive = false,
                Status = false
            });

            await repo.SaveChangesAsync();

            var appointment = await repo.GetByIdAsync<Appointment>(1);

            Assert.That(appointment.Status, Is.False);
            Assert.That(appointment.IsActive, Is.False);
        }

        [Test]

        public async Task CreateAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            appointmentService = new AppointmentService(repo);

            var model = new AppointmentFormModel()
            {
                Details = "If I'm late, I'll call",
                Status = true,
                StartDate = DateTime.ParseExact("30/01/2023 15:00:00", "dd/MM/yyyy HH:mm:ss", null),
            };

            await appointmentService.CreateAsync(model, 1, 1);

            var appointment = await repo.GetByIdAsync<Appointment>(1);

            Assert.That(appointment.DentistId, Is.EqualTo(1));
            Assert.That(appointment.PatientId, Is.EqualTo(1));
            Assert.That(appointment.Status, Is.True);
            Assert.That(appointment.StartDateTime, Is.EqualTo(DateTime.ParseExact("30/01/2023 15:00:00", "dd/MM/yyyy HH:mm:ss", null)));
        }



        [TearDown]

        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
