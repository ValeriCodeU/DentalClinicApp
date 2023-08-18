using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Services;
using DentalClinicApp.Infrastructure.Data;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;
using DentalClinicApp.Infrastructure.Data.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DentalClinicApp.Test
{
    [TestFixture]

    public class PatientServiceTest
    {
        private IRepository repo;
        private IPatientService patientService;
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

        public async Task IsExistsByIdAsync_ShouldReturnTrue_WithValidId()
        {
            var repo = new Repository(dbContext);
            patientService = new PatientService(repo);

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

            await repo.SaveChangesAsync();

            var patientExist = await patientService.IsExistsByIdAsync(new Guid("da24feae-ab42-4702-bbf9-9c5361aee8d6"));

            Assert.That(patientExist, Is.True);
        }

        [Test]

        public async Task CreatePatientAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            patientService = new PatientService(repo);

            await repo.AddAsync(new Patient()
            {
                Id = 2,
                UserId = new Guid("da24feae-ab42-4702-bbf9-9c5361aee8d6"),
                DentistId = 1,
            });
            
            await repo.SaveChangesAsync();

            var patientsCountBefore = await repo.AllReadonly<Patient>().CountAsync();
            await patientService.CreatePatientAsync(new Guid("da24feae-ab42-4702-bbf9-9c5361aee8d6"), 1);

            var patientsCountAfter = await repo.AllReadonly<Patient>().CountAsync();
            var patientCollection = await repo.AllReadonly<Patient>().ToListAsync();

            Assert.That(patientsCountBefore + 1, Is.EqualTo(patientsCountAfter));
            Assert.That(patientCollection.Any(p => p.Id == 2), Is.True);
        }

        [Test]

        public async Task GetPersonalDentistIdAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            patientService = new PatientService(repo);

            await repo.AddAsync(new Patient()
            {
                Id = 2,
                UserId = new Guid("da24feae-ab42-4702-bbf9-9c5361aee8d6"),
                DentistId = 1,
            });

            await repo.SaveChangesAsync();

            var dentistId = await patientService.GetPersonalDentistIdAsync(2);
            Assert.That(dentistId, Is.EqualTo(1));
        }

        [Test]

        public async Task GetPatientsAsync_ShouldReturnCorrectPatient()
        {
            var repo = new Repository(dbContext);
            patientService = new PatientService(repo);

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

            await repo.SaveChangesAsync();

            var result = await patientService.GetPatientsAsync(new Guid("da24feae-ab42-4702-bbf9-9c5361aee8d6"));

            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.Where(x => x.Id == 1).Select(x => x.Name).First, Is.EqualTo("Gencho Genchev"));
            Assert.That(result.Where(x => x.Id == 1).Select(x => x.Id).First, Is.EqualTo(1));

        }

        [Test]

        public async Task GetPatientIdAsync_ShouldReturnCorrectId()
        {
            var repo = new Repository(dbContext);
            patientService = new PatientService(repo);

            await repo.AddAsync(new Patient()
            {
                Id = 10,
                UserId = new Guid("da24feae-ab42-4702-bbf9-9c5361aee8d6"),
                DentistId = 1,
            });

            await repo.SaveChangesAsync();

            var patientId = await patientService.GetPatientIdAsync(new Guid("da24feae-ab42-4702-bbf9-9c5361aee8d6"));

            Assert.That(patientId, Is.EqualTo(10));
        }


        [Test]

        public async Task GetUserIdByPatientId_ShouldReturnCorrectGuidUserId()
        {
            var repo = new Repository(dbContext);
            patientService = new PatientService(repo);

            await repo.AddAsync(new Patient()
            {
                Id = 10,
                UserId = new Guid("da24feae-ab42-4702-bbf9-9c5361aee8d6"),
                DentistId = 1,
            });

            await repo.SaveChangesAsync();

            var patientId = await patientService.GetUserIdByPatientId(10);

            Assert.That(patientId, Is.EqualTo(new Guid("da24feae-ab42-4702-bbf9-9c5361aee8d6")));
        }

        [Test]

        public async Task GetMyPatientsAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            patientService = new PatientService(repo);

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

            await repo.SaveChangesAsync();

            var result = await patientService.GetMyPatientsAsync(1);

            Assert.IsNotNull(result);
            Assert.That(result.Patients.Count, Is.EqualTo(1));
            Assert.That(result.Patients.Where(x => x.Id == 1).Select(x => x.Id).First, Is.EqualTo(1));
            Assert.That(result.Patients.Where(x => x.Id == 1).Select(x => x.FirstName).First, Is.EqualTo("Gencho"));
            Assert.That(result.Patients.Where(x => x.Id == 1).Select(x => x.LastName).First, Is.EqualTo("Genchev"));
            Assert.That(result.Patients.Where(x => x.Id == 1).Select(x => x.PhoneNumber).First, Is.EqualTo("9999999999999"));
            Assert.That(result.Patients.Where(x => x.Id == 1).Select(x => x.Email).First, Is.EqualTo("gencho@mail.com"));
        }

        [Test]

        public async Task PatientDetailsByIdAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            patientService = new PatientService(repo);

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

            await repo.AddAsync(new Patient()
            {
                Id = 1,
                UserId = new Guid("94a79c1d-5a55-4260-815a-d5b827d93a1d"),
                DentistId = 1,
            });

            await repo.AddAsync(new DentalProblem()
            {
                Id = 1,
                DiseaseName = "Sensitive to cold",
                DiseaseDescription = "Pain when consuming cold drinks",
                AlergyDescription = null,
                DentalStatus = "45",
                PatientId = 1,
                IsActive = true,
            });

            await repo.SaveChangesAsync();

            var result = await patientService.PatientDetailsByIdAsync(1);

            Assert.NotNull(result);
            Assert.That(result.PatientProblems.Count, Is.EqualTo(1));
            Assert.That(result.FirstName, Is.EqualTo("Gencho"));
            Assert.That(result.LastName, Is.EqualTo("Genchev"));
            Assert.That(result.PhoneNumber, Is.EqualTo("9999999999999"));
            Assert.That(result.Email, Is.EqualTo("gencho@mail.com"));
            Assert.That(result.PatientProblems.Count, Is.EqualTo(1));
            Assert.That(result.PatientProblems.Select(x => x.DiseaseName).First, Is.EqualTo("Sensitive to cold"));
        }

        [Test]

        public async Task PatientAttendanceDetailsByIdAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            patientService = new PatientService(repo);

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

            await repo.AddAsync(new Patient()
            {
                Id = 1,
                UserId = new Guid("94a79c1d-5a55-4260-815a-d5b827d93a1d"),
                DentistId = 1,
            });

            var date = DateTime.Now;

            await repo.AddAsync(new Attendance()
            {
                Id = 1,
                ClinicRemarks = "You need a filling, a root canal, or treatment of your gums to replace tissue lost at the root.",
                IsActive = true,
                Diagnosis = "Cavities and worn tooth enamel",
                PatientId = 1,
                DentistId = 1,
                Date = date,
            });

            await repo.SaveChangesAsync();

            var result = await patientService.PatientAttendanceDetailsByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.That(result.PatientAttendances.Count, Is.EqualTo(1));
            Assert.That(result.FirstName, Is.EqualTo("Gencho"));
            Assert.That(result.LastName, Is.EqualTo("Genchev"));
            Assert.That(result.PhoneNumber, Is.EqualTo("9999999999999"));
            Assert.That(result.Email, Is.EqualTo("gencho@mail.com"));
            Assert.That(result.PatientAttendances.Select(x => x.Id).First, Is.EqualTo(1));
            Assert.That(result.PatientAttendances.Select(x => x.ClinicRemarks).First, Is.EqualTo("You need a filling, a root canal, or treatment of your gums to replace tissue lost at the root."));
            Assert.That(result.PatientAttendances.Select(x => x.Diagnosis).First, Is.EqualTo("Cavities and worn tooth enamel"));
            Assert.That(result.PatientAttendances.Select(x => x.Date).First, Is.EqualTo(date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
        }

        [Test]

        public async Task PatientProcedureDetailsByIdAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            patientService = new PatientService(repo);


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

            await repo.AddAsync(new Patient()
            {
                Id = 1,
                UserId = new Guid("94a79c1d-5a55-4260-815a-d5b827d93a1d"),
                DentistId = 1,
            });

            await repo.AddAsync(new DentalProcedure()
            {
                Id = 1,
                Name = "Pulling a tooth out",
                Description = "Classic tooth extraction",
                StartDate = DateTime.ParseExact("30/12/2022 15:00:00", "dd/MM/yyyy HH:mm:ss", null),
                EndDate = DateTime.ParseExact("30/01/2023 15:00:00", "dd/MM/yyyy HH:mm:ss", null),
                DentistId = 1,
                PatientId = 1,
                Cost = 100,
                IsActive = true,
            });

            await repo.SaveChangesAsync();

            var result = await patientService.PatientProcedureDetailsByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.That(result.PatientProcedures.Count, Is.EqualTo(1));
            Assert.That(result.FirstName, Is.EqualTo("Gencho"));
            Assert.That(result.LastName, Is.EqualTo("Genchev"));
            Assert.That(result.PhoneNumber, Is.EqualTo("9999999999999"));
            Assert.That(result.Email, Is.EqualTo("gencho@mail.com"));
            Assert.That(result.PatientProcedures.Select(x => x.Id).First, Is.EqualTo(1));
            Assert.That(result.PatientProcedures.Select(x => x.Description).First, Is.EqualTo("Classic tooth extraction"));
            Assert.That(result.PatientProcedures.Select(x => x.Name).First, Is.EqualTo("Pulling a tooth out"));
            Assert.That(result.PatientProcedures.Select(x => x.StartDate).First, Is.EqualTo("12/30/2022"));
            Assert.That(result.PatientProcedures.Select(x => x.EndDate).First, Is.EqualTo("01/30/2023"));
            Assert.That(result.PatientProcedures.Select(x => x.Cost).First, Is.EqualTo(100));
            Assert.That(result.PatientProcedures.Select(x => x.Note).First, Is.Null);
        }

        [TearDown]

        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
