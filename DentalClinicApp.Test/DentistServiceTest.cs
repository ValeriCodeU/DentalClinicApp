using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Services;
using DentalClinicApp.Infrastructure.Data;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;
using DentalClinicApp.Infrastructure.Data.Identity;
using Microsoft.EntityFrameworkCore;


namespace DentalClinicApp.Test
{
    [TestFixture]

    public class DentistServiceTest
    {
        private IRepository repo;
        private IDentistService dentistService;
        private DentalClinicDbContext dbContext;

        [SetUp]

        public void SetUp()
        {
            var contextOptions = new DbContextOptionsBuilder<DentalClinicDbContext>()
                .UseInMemoryDatabase("DentalDB")
                .Options;

            dbContext = new DentalClinicDbContext(contextOptions);

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }

        [Test]

        public async Task IsExistsByIdAsync_ShouldReturnTrue_WithValidId()
        {
            var repo = new Repository(dbContext);
            dentistService = new DentistService(repo);

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

            await repo.AddAsync(new Dentist()
            {
                Id = 2,
                UserId = new Guid("e28afed9-0de3-4ca6-aee8-28488401bca8"),
                ManagerId = 1,
            });

            await repo.SaveChangesAsync();

            var dentistExist = await dentistService.IsExistsByIdAsync(new Guid("e28afed9-0de3-4ca6-aee8-28488401bca8"));

            Assert.That(dentistExist, Is.True);
        }

        [Test]

        public async Task GetDentistIdAsync_ShouldReturnCorrectResult()
        {
            var repo = new Repository(dbContext);
            dentistService = new DentistService(repo);

            await repo.AddAsync(new Dentist()
            {
                Id = 3,
                UserId = new Guid("e28afed9-0de3-4ca6-aee8-28488401bca8"),
                ManagerId = 1,
            });

            await repo.SaveChangesAsync();

            var dentistId = await dentistService.GetDentistIdAsync(new Guid("e28afed9-0de3-4ca6-aee8-28488401bca8"));

            Assert.That(dentistId, Is.EqualTo(3));
        }

        [Test]
        public async Task GetManagerOfDentistAsync_ShouldReturnCorrectResult()
        {
            var repo = new Repository(dbContext);
            dentistService = new DentistService(repo);

            await repo.AddAsync(new Manager()
            {
                Id = 11,
                UserId = new Guid("48787569-f841-4832-8528-1f503a8427cf"),
            });

            await repo.SaveChangesAsync();

            var managerId = await dentistService.GetManagerOfDentistAsync(new Guid("48787569-f841-4832-8528-1f503a8427cf"));

            Assert.That(managerId, Is.EqualTo(11));
        }

        [Test]

        public async Task AddUserAsDentistAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            dentistService = new DentistService(repo);

            var result = await dentistService.AddUserAsDentistAsync(new Guid("e28afed9-0de3-4ca6-aee8-28488401bca8"), 1);

            Assert.That(result, Is.True);
        }

        [Test]

        public async Task GetAllManagedDentistsAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            dentistService = new DentistService(repo);

            await repo.AddAsync(new ApplicationUser()
            {
                Id = new Guid("48787569-f841-4832-8528-1f503a8427cf"),
                FirstName = "Lionel",
                LastName = "Scaloni",
                UserName = "lionel",
                NormalizedUserName = "LIONEL",
                Email = "lionel@mail.com",
                NormalizedEmail = "LIONEL@MAIL.COM",
                PhoneNumber = "222222222222222",
            });

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


            await repo.AddAsync(new Manager()
            {
                Id = 1,
                UserId = new Guid("48787569-f841-4832-8528-1f503a8427cf")
            });


            await repo.AddAsync(new Dentist()
            {
                Id = 2,
                UserId = new Guid("e28afed9-0de3-4ca6-aee8-28488401bca8"),
                ManagerId = 1,
            });

            await repo.SaveChangesAsync();

            var result = await dentistService.GetAllManagedDentistsAsync(new Guid("48787569-f841-4832-8528-1f503a8427cf"));

            Assert.IsNotNull(result);
            Assert.That(result.Dentists.Count, Is.EqualTo(1));
            Assert.That(result.Dentists.Where(x => x.Id == 2).Select(x => x.PhoneNumber).First, Is.EqualTo("33333333333333"));
            Assert.That(result.Dentists.Where(x => x.Id == 2).Select(x => x.Email).First, Is.EqualTo("vasil@mail.com"));
            Assert.That(result.Dentists.Where(x => x.Id == 2).Select(x => x.FirstName).First, Is.EqualTo("Vasil"));
            Assert.That(result.Dentists.Where(x => x.Id == 2).Select(x => x.LastName).First, Is.EqualTo("Georgiev"));
        }

        [Test]

        public async Task GetStatisticsAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            dentistService = new DentistService(repo);

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

            await repo.AddAsync(new Dentist()
            {
                Id = 1,
                UserId = new Guid("e28afed9-0de3-4ca6-aee8-28488401bca8"),
                ManagerId = 1,
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

            await repo.AddAsync(new ApplicationUser()
            {
                Id = new Guid("94a79c1d-5a55-4260-815a-d5b827d93a1d"),
                FirstName = "Gencho",
                LastName = "Genchev",
                UserName = "gencho",
                NormalizedUserName = "GENCHO",
                Email = "gencho@mail.com",
                NormalizedEmail = "GENCHO@MAIL.COM",
                PhoneNumber = "999999999"
            });

            await repo.AddAsync(new Patient()
            {
                Id = 1,
                UserId = new Guid("94a79c1d-5a55-4260-815a-d5b827d93a1d"),
                DentistId = 1,
            });

            await repo.AddAsync(new Attendance()
            {
                Id = 1,
                ClinicRemarks = "You need a filling, a root canal, or treatment of your gums to replace tissue lost at the root.",
                IsActive = true,
                Diagnosis = "Cavities and worn tooth enamel",
                PatientId = 1,
                DentistId = 1,
                Date = DateTime.Now,
            });


            await repo.SaveChangesAsync();

            var result = await dentistService.GetStatisticsAsync(1);

            Assert.That(result.TotalProceduresCount, Is.EqualTo(1));
            Assert.That(result.TotalPatientsCount, Is.EqualTo(1));
            Assert.That(result.TotalAttendancesCount, Is.EqualTo(1));
        }


        [TearDown]

        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
