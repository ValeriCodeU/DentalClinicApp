using DentalClinicApp.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DentalClinicApp.Infrastructure.Configuration
{
	internal class ProcedureConfiguration : IEntityTypeConfiguration<DentalProcedure>
	{
		public void Configure(EntityTypeBuilder<DentalProcedure> builder)
		{
            builder.HasData(CollectionOfProcedures());
		}

        private List<DentalProcedure> CollectionOfProcedures()
        {
            var procedures = new List<DentalProcedure>()
            {
                new DentalProcedure()
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
                },

                 new DentalProcedure()
                {
                    Id = 2,
                    Name = "Dental implant",
                    Description = "Bridge made and fitted after losing teeth",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    DentistId = 1,
                    PatientId = 1,
                    Cost = 300,
                    IsActive = true,
                },
            };

            return procedures;
        }
    }
}
