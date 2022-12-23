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


        [TearDown]

        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
