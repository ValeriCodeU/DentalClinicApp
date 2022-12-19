using DentalClinicApp.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Infrastructure.Configuration
{
	internal class PatientConfiguration : IEntityTypeConfiguration<Patient>
	{
		public void Configure(EntityTypeBuilder<Patient> builder)
		{
			builder.HasData(new Patient()
			{
				Id = 1,
				UserId = new Guid("da24feae-ab42-4702-bbf9-9c5361aee8d6"),
				DentistId = 1

			});
		}
	}
}
