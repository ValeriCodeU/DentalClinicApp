using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.DentalProcedures;
using DentalClinicApp.Core.Services;
using DentalClinicApp.Infrastructure.Data;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;
using DentalClinicApp.Infrastructure.Data.Identity;
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
            var procedureDeleted = await repo.GetByIdAsync<DentalProcedure>(2);
            var procedureNotDeleted = await repo.GetByIdAsync<DentalProcedure>(1);

            var result = await procedureService.DeleteProcedureAsync(2);

            Assert.That(procedureDeleted.IsActive, Is.False);
            Assert.That(procedureNotDeleted.IsActive, Is.True);
            Assert.That(result, Is.True);
        }

        [Test]

        public async Task EditProcedureAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            procedureService = new ProcedureService(repo);

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
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                DentistId = 1,
                PatientId = 1,
                Cost = 100,
                IsActive = true,
            });

            await repo.SaveChangesAsync();

            var startDate = DateTime.Now.ToString();
            var endDate = DateTime.Now.ToString();

            var model = new ProcedureFormModel()
            {
                Name = "Pulling a tooth out",
                Description = "Classic tooth extraction",
                StartDate = startDate,
                EndDate = endDate,
                PatientId = 1,
                Cost = 200,
            };

            await procedureService.EditProcedureAsync(model, 1);

            var procedure = await repo.GetByIdAsync<DentalProcedure>(1);

            Assert.That(procedure.Cost, Is.EqualTo(200));
            Assert.That(procedure.Id, Is.EqualTo(1));
            Assert.That(procedure.Name, Is.EqualTo("Pulling a tooth out"));
            Assert.That(procedure.Description, Is.EqualTo("Classic tooth extraction"));
            Assert.That(procedure.PatientId, Is.EqualTo(1));
            Assert.That(procedure.DentistId, Is.EqualTo(1));            
            Assert.That(procedure.StartDate, Is.EqualTo(DateTime.Parse(startDate)));            
            Assert.That(procedure.EndDate, Is.EqualTo(DateTime.Parse(endDate)));            
            Assert.That(procedure.Patient.User.FirstName, Is.EqualTo("Gencho"));
            Assert.That(procedure.Patient.User.LastName, Is.EqualTo("Genchev"));
            Assert.That(procedure.Patient.User.Email, Is.EqualTo("gencho@mail.com"));
            Assert.That(procedure.Patient.User.PhoneNumber, Is.EqualTo("9999999999999"));
        }

        [Test]

        public async Task CreateAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            procedureService = new ProcedureService(repo);

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

            await repo.SaveChangesAsync();

            var startDate = DateTime.Now.ToString();
            var endDate = DateTime.Now.ToString();

            var model = new ProcedureFormModel()
            {
                Name = "Pulling a tooth out",
                Description = "Classic tooth extraction",
                StartDate = startDate,
                EndDate = endDate,
                PatientId = 1,
                Cost = 200,
            };

            await procedureService.CreateAsync(model, 1);

            var procedure = await repo.GetByIdAsync<DentalProcedure>(1);

            Assert.That(procedure.Cost, Is.EqualTo(200));
            Assert.That(procedure.Id, Is.EqualTo(1));
            Assert.That(procedure.Name, Is.EqualTo("Pulling a tooth out"));
            Assert.That(procedure.Description, Is.EqualTo("Classic tooth extraction"));
            Assert.That(procedure.PatientId, Is.EqualTo(1));
            Assert.That(procedure.DentistId, Is.EqualTo(1));
            Assert.That(procedure.StartDate, Is.EqualTo(DateTime.Parse(startDate)));
            Assert.That(procedure.EndDate, Is.EqualTo(DateTime.Parse(endDate)));
            Assert.That(procedure.Patient.User.FirstName, Is.EqualTo("Gencho"));
            Assert.That(procedure.Patient.User.LastName, Is.EqualTo("Genchev"));
            Assert.That(procedure.Patient.User.Email, Is.EqualTo("gencho@mail.com"));
            Assert.That(procedure.Patient.User.PhoneNumber, Is.EqualTo("9999999999999"));
        }

        [Test]

        public async Task ProcedureDetailsByIdAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            procedureService = new ProcedureService(repo);

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

            var procedureDetails = await procedureService.ProcedureDetailsByIdAsync(1);

            Assert.That(procedureDetails, Is.Not.Null);
            Assert.That(procedureDetails.Id, Is.EqualTo(1));           
            Assert.That(procedureDetails.Name, Is.EqualTo("Pulling a tooth out"));
            Assert.That(procedureDetails.Description, Is.EqualTo("Classic tooth extraction"));
            Assert.That(procedureDetails.Cost, Is.EqualTo(100));
            Assert.That(procedureDetails.StartDate, Is.EqualTo("30/12/2022"));
            Assert.That(procedureDetails.EndDate, Is.EqualTo("30/01/2023"));            
            Assert.That(procedureDetails.Patient.Id, Is.EqualTo(1));
            Assert.That(procedureDetails.Patient.FirstName, Is.EqualTo("Dimitar"));
            Assert.That(procedureDetails.Patient.LastName, Is.EqualTo("Georgiev"));
            Assert.That(procedureDetails.Patient.Email, Is.EqualTo("dimitar@mail.com"));
            Assert.That(procedureDetails.Patient.PhoneNumber, Is.EqualTo("1111111111111"));
        }

        [Test]

        public async Task GetDentistProceduresAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            procedureService = new ProcedureService(repo);

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

            var modelForTest = await procedureService
                .GetProceduresAsync(1, true, Core.Models.DentalProcedures.Enums.ProcedureSorting.StartDate, null);

            Assert.That(modelForTest.Procedures, Is.Not.Null);
            Assert.That(modelForTest.Procedures.Count, Is.EqualTo(1));
            Assert.That(modelForTest.Procedures.Where(x => x.Id == 1).Select(x => x.Name).First, Is.EqualTo("Pulling a tooth out"));
            Assert.That(modelForTest.Procedures.Where(x => x.Id == 1).Select(x => x.Description).First, Is.EqualTo("Classic tooth extraction"));
            Assert.That(modelForTest.Procedures.Where(x => x.Id == 1).Select(x => x.Cost).First, Is.EqualTo(100));
            Assert.That(modelForTest.Procedures.Where(x => x.Id == 1).Select(x => x.StartDate).First, Is.EqualTo("30/12/2022"));            
            Assert.That(modelForTest.Procedures.Where(x => x.Id == 1).Select(x => x.EndDate).First, Is.EqualTo("30/01/2023"));
            Assert.That(modelForTest.Procedures.Where(x => x.Id == 1).Select(x => x.Patient.FirstName).First, Is.EqualTo("Gencho"));
            Assert.That(modelForTest.Procedures.Where(x => x.Id == 1).Select(x => x.Patient.LastName).First, Is.EqualTo("Genchev"));
            Assert.That(modelForTest.Procedures.Where(x => x.Id == 1).Select(x => x.Patient.Email).First, Is.EqualTo("gencho@mail.com"));
            Assert.That(modelForTest.Procedures.Where(x => x.Id == 1).Select(x => x.Patient.PhoneNumber).First, Is.EqualTo("9999999999999"));      
            Assert.That(modelForTest.Procedures.Where(x => x.Id == 1).Select(x => x.Note).First, Is.Null);
        }

        [Test]

        public async Task AllProceduresByPatientIdAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            procedureService = new ProcedureService(repo);

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

            var result = await procedureService.AllProceduresByPatientIdAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.Where(x => x.Id == 1).Select(p => p.Name).First, Is.EqualTo("Pulling a tooth out"));
            Assert.That(result.Where(x => x.Id == 1).Select(p => p.Description).First, Is.EqualTo("Classic tooth extraction"));
            Assert.That(result.Where(x => x.Id == 1).Select(p => p.Id).First, Is.EqualTo(1));
            Assert.That(result.Where(x => x.Id == 1).Select(p => p.Cost).First, Is.EqualTo(100));
            Assert.That(result.Where(x => x.Id == 1).Select(p => p.StartDate).First, Is.EqualTo("30/12/2022"));
            Assert.That(result.Where(x => x.Id == 1).Select(p => p.EndDate).First, Is.EqualTo("30/01/2023"));
            Assert.That(result.Where(x => x.Id == 1).Select(p => p.Id).First, Is.EqualTo(1));
        }

        [TearDown]

        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
