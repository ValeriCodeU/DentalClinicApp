using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Infrastructure.Data;
using DentalClinicApp.Infrastructure.Data.Common;
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

        [TearDown]

        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
