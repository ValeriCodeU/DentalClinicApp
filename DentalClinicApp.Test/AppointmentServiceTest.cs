using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Appointments;
using DentalClinicApp.Core.Services;
using DentalClinicApp.Infrastructure.Data;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;
using DentalClinicApp.Infrastructure.Data.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Frameworks;

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

        public async Task GetAppointmentByIdAsync_ShouldReturnCorrectAppointment()
        {
            var repo = new Repository(dbContext);
            appointmentService = new AppointmentService(repo);


            await repo.AddAsync(new ApplicationUser()
            {
                Id = new Guid("e28afed9-0de3-4ca6-aee8-28488401bca8"),
                FirstName = "Vasil",
                LastName = "Georgiev",
                UserName = "vasil",
                NormalizedUserName = "VASIL",
                Email = "vasil@mail.com",
                NormalizedEmail = "VASIL@MAIL.COM",
                PhoneNumber = "33333333333333",
            });

            await repo.AddAsync(new Patient()
            {
                Id = 1,
                UserId = new Guid("e28afed9-0de3-4ca6-aee8-28488401bca8"),
                DentistId = 1,
            });


            await repo.AddAsync(new Appointment()
            {
                Id = 15,
                StartDateTime = DateTime.ParseExact("23/12/2022 14:00:00", "dd/MM/yyyy HH:mm:ss", null),
                PatientId = 1,
                DentistId = 1,
            });

            await repo.SaveChangesAsync();

            var appointment = await appointmentService.GetAppointmentByIdAsync(15);

            Assert.That(appointment, Is.Not.Null);
            Assert.That(appointment.Status, Is.False);
            Assert.That(appointment.Id, Is.EqualTo(15));
            Assert.That(appointment.Details, Is.Null);
            Assert.That(appointment.StartDate, Is.EqualTo(DateTime.ParseExact("23/12/2022 14:00:00", "dd/MM/yyyy HH:mm:ss", null)));
            Assert.That(appointment.Patient.FirstName, Is.EqualTo("Vasil"));
            Assert.That(appointment.Patient.LastName, Is.EqualTo("Georgiev"));
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
