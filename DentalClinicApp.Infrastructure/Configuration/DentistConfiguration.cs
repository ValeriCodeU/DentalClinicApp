using DentalClinicApp.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DentalClinicApp.Infrastructure.Configuration
{
    internal class DentistConfiguration : IEntityTypeConfiguration<Dentist>
	{
		public void Configure(EntityTypeBuilder<Dentist> builder)
		{
            builder.HasData(CollectionOfDentists());
		}

        private List<Dentist> CollectionOfDentists()
        {
            var dentists = new List<Dentist>()
            {
                new Dentist()
                {
                    Id = 1,
                    UserId = new Guid("bfbcc7d7-2e7e-4d3c-b7fb-4b76f27cefe3"),
                    ManagerId = 1

                },

                 new Dentist()
                {
                    Id = 2,
                    UserId = new Guid("94a79c1d-5a55-4260-815a-d5b827d93a1d"),
                    ManagerId = 1

                },
            };

            return dentists;
        }
    }
}
