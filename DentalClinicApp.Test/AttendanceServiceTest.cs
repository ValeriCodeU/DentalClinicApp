﻿using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Attendances;
using DentalClinicApp.Core.Services;
using DentalClinicApp.Infrastructure.Data;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;
using DentalClinicApp.Infrastructure.Data.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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

        public async Task AttendanceExistsAsync_ShouldReturnTrue_WithValidId()
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
                PatientId = 1                
            };

            var result = await attendanceService.CreateAsync(model, 1);

            var attendance = await repo.GetByIdAsync<Attendance>(1);

            Assert.That(result, Is.EqualTo(1));
            Assert.That(attendance.ClinicRemarks, Is.EqualTo("You need a root canal and a crown"));
            Assert.That(attendance.Diagnosis, Is.EqualTo("Fractured tooth"));            
            Assert.That(attendance.DentistId, Is.EqualTo(1));
            Assert.That(attendance.PatientId, Is.EqualTo(1));
        }

        [Test]

        public async Task DeleteAttendanceAsync_ShouldWorkCorrectly()
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

            var attendance = await repo.GetByIdAsync<Attendance>(3);

            var result = await attendanceService.DeleteAttendanceAsync(3);

            Assert.That(attendance.IsActive, Is.False);
            Assert.That(result, Is.True);
        }

        [Test]

        public async Task EditAttendanceAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            attendanceService = new AttendanceService(repo);

            await repo.AddAsync(new Attendance()
            {
                ClinicRemarks = "No Problem",
                IsActive = true,
                Diagnosis = "No diagnosis",
                PatientId = 1,
                DentistId = 1,
                Date = DateTime.Now,
            });
            await repo.SaveChangesAsync();

            var model = new AttendanceFormModel()
            {
                ClinicRemarks = "You need a root canal and a crown",
                Diagnosis = "Fractured tooth",
                PatientId = 3,
            };

            await attendanceService.EditAttendanceAsync(model, 1);

            var attendance = await repo.GetByIdAsync<Attendance>(1);

            Assert.That(attendance.PatientId, Is.EqualTo(3));
            Assert.That(attendance.ClinicRemarks, Is.EqualTo("You need a root canal and a crown"));
            Assert.That(attendance.Diagnosis, Is.EqualTo("Fractured tooth"));
            Assert.IsTrue(attendance.IsActive);
        }

        [Test]

        public async Task AllAttendancesByPatientIdAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            attendanceService = new AttendanceService(repo);

            await repo.AddAsync(new ApplicationUser()
            {
                Id = new Guid("da24feae-ab42-4702-bbf9-9c5361aee8d6"),
                FirstName = "Dimitar",
                LastName = "Georgiev",
                UserName = "dimitar",
                NormalizedUserName = "DIMITAR",
                Email = "dimitar@mail.com",
                NormalizedEmail = "DIMITAR@MAIL.COM",
                PhoneNumber = "1111111111111",
            });

            await repo.AddAsync(new Patient()
            {
                Id = 1,
                UserId = new Guid("da24feae-ab42-4702-bbf9-9c5361aee8d6"),
                DentistId = 1,
            });

            await repo.AddAsync(new Attendance()
            {
                Id = 1,
                ClinicRemarks = "No Problem",
                IsActive = true,
                Diagnosis = "No diagnosis",
                PatientId = 1,
                DentistId = 1,
                Date = DateTime.Now,
            });

            await repo.AddAsync(new Attendance()
            {
                Id = 2,
                ClinicRemarks = "You need a root canal and a crown",
                IsActive = true,
                Diagnosis = "Fractured tooth",
                PatientId = 1,
                DentistId = 1,
                Date = DateTime.Now,
            });

            var date = DateTime.Now;

            await repo.AddAsync(new Attendance()
            {
                Id = 3,
                ClinicRemarks = "You need a filling, a root canal, or treatment of your gums to replace tissue lost at the root.",
                IsActive = true,
                Diagnosis = "Cavities and worn tooth enamel",
                PatientId = 1,
                DentistId = 1,
                Date = date,
            });

            await repo.SaveChangesAsync();

            var result = await attendanceService.AllAttendancesByPatientIdAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result.Where(x => x.Id == 3).Select(p => p.ClinicRemarks).First, Is.EqualTo("You need a filling, a root canal, or treatment of your gums to replace tissue lost at the root."));
            Assert.That(result.Where(x => x.Id == 3).Select(p => p.Diagnosis).First, Is.EqualTo("Cavities and worn tooth enamel"));
            Assert.That(result.Where(x => x.Id == 3).Select(p => p.Id).First, Is.EqualTo(3));
            Assert.That(result.Where(x => x.Id == 3).Select(p => p.Date).First, Is.EqualTo(date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));
        }


        [Test]

        public async Task AttendanceDetailsByIdAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            attendanceService = new AttendanceService(repo);

            await repo.AddAsync(new ApplicationUser()
            {
                Id = new Guid("da24feae-ab42-4702-bbf9-9c5361aee8d6"),
                FirstName = "Dimitar",
                LastName = "Georgiev",
                UserName = "dimitar",
                NormalizedUserName = "DIMITAR",
                Email = "dimitar@mail.com",
                NormalizedEmail = "DIMITAR@MAIL.COM",
                PhoneNumber = "1111111111111",
            });

            await repo.AddAsync(new Patient()
            {
                Id = 1,
                UserId = new Guid("da24feae-ab42-4702-bbf9-9c5361aee8d6"),
                DentistId = 1,
            });

            var date = DateTime.Now;

            await repo.AddAsync(new Attendance()
            {
                Id = 3,
                ClinicRemarks = "You need a filling, a root canal, or treatment of your gums to replace tissue lost at the root.",
                IsActive = true,
                Diagnosis = "Cavities and worn tooth enamel",
                PatientId = 1,
                DentistId = 1,
                Date = date,
            });

            await repo.SaveChangesAsync();

            var attendanceDetails = await attendanceService.AttendanceDetailsByIdAsync(3);

            Assert.That(attendanceDetails, Is.Not.Null);
            Assert.That(attendanceDetails.Diagnosis, Is.EqualTo("Cavities and worn tooth enamel"));
            Assert.That(attendanceDetails.Patient.FirstName, Is.EqualTo("Dimitar"));
            Assert.That(attendanceDetails.Patient.LastName, Is.EqualTo("Georgiev"));
            Assert.That(attendanceDetails.Patient.Email, Is.EqualTo("dimitar@mail.com"));
            Assert.That(attendanceDetails.Patient.PhoneNumber, Is.EqualTo("1111111111111"));
            Assert.That(attendanceDetails.ClinicRemarks, Is.EqualTo("You need a filling, a root canal, or treatment of your gums to replace tissue lost at the root."));
            Assert.That(attendanceDetails.Id, Is.EqualTo(3));
            Assert.That(attendanceDetails.Date, Is.EqualTo(date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));
        }

        [Test]

        public async Task GetDentistAttendancesAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            attendanceService = new AttendanceService(repo);

            await repo.AddAsync(new ApplicationUser()
            {
                Id = new Guid("da24feae-ab42-4702-bbf9-9c5361aee8d6"),
                FirstName = "Dimitar",
                LastName = "Georgiev",
                UserName = "dimitar",
                NormalizedUserName = "DIMITAR",
                Email = "dimitar@mail.com",
                NormalizedEmail = "DIMITAR@MAIL.COM",
                PhoneNumber = "1111111111111",
            });

            await repo.AddAsync(new ApplicationUser()
            {
                Id = new Guid("94a79c1d-5a55-4260-815a-d5b827d93a1d"),
                FirstName = "Gencho",
                LastName = "Genchev",
                UserName = "gencho",
                NormalizedUserName = "GENCHO",
                Email = "gencho@mail.com",
                NormalizedEmail = "GENCHO@MAIL.COM",
                PhoneNumber = "9999999999999",
            });

            await repo.AddAsync(new Dentist()
            {
                Id = 1,
                UserId = new Guid("da24feae-ab42-4702-bbf9-9c5361aee8d6"),
                ManagerId = 1,
            });

            await repo.AddAsync(new Patient()
            {
                Id = 1,
                UserId = new Guid("94a79c1d-5a55-4260-815a-d5b827d93a1d"),
                DentistId = 1,
            });

            var date = DateTime.Now;

            await repo.AddAsync(new Attendance()
            {
                Id = 3,
                ClinicRemarks = "You need a filling, a root canal, or treatment of your gums to replace tissue lost at the root.",
                IsActive = true,
                Diagnosis = "Cavities and worn tooth enamel",
                PatientId = 1,
                DentistId = 1,
                Date = date,
            });

            await repo.SaveChangesAsync();

            var modelForTest = await attendanceService
                .GetAttendancesAsync(1, true, ShoppingListApp.Core.Models.Products.Enums.AttendanceSorting.Newest);

            Assert.That(modelForTest, Is.Not.Null);
            Assert.That(modelForTest.Attendances.Count, Is.EqualTo(1));
            Assert.That(modelForTest.Attendances.Where(x => x.Id == 3).Select(p => p.ClinicRemarks).First, Is.EqualTo("You need a filling, a root canal, or treatment of your gums to replace tissue lost at the root."));
            Assert.That(modelForTest.Attendances.Where(x => x.Id == 3).Select(p => p.Diagnosis).First, Is.EqualTo("Cavities and worn tooth enamel"));
            Assert.That(modelForTest.Attendances.Where(x => x.Id == 3).Select(p => p.Id).First, Is.EqualTo(3));
            Assert.That(modelForTest.Attendances.Where(x => x.Id == 3).Select(p => p.Date).First, Is.EqualTo(date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));
            Assert.That(modelForTest.Attendances.Where(x => x.Id == 3).Select(x => x.Patient.FirstName).First, Is.EqualTo("Gencho"));
            Assert.That(modelForTest.Attendances.Where(x => x.Id == 3).Select(x => x.Patient.LastName).First, Is.EqualTo("Genchev"));
            Assert.That(modelForTest.Attendances.Where(x => x.Id == 3).Select(x => x.Patient.Email).First, Is.EqualTo("gencho@mail.com"));
            Assert.That(modelForTest.Attendances.Where(x => x.Id == 3).Select(x => x.Patient.PhoneNumber).First, Is.EqualTo("9999999999999"));
        }

        [TearDown]

        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
