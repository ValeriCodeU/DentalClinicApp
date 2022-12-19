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
	internal class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
	{
		public void Configure(EntityTypeBuilder<Appointment> builder)
		{
            builder.HasData(CollectionOfAppointments());
		}

        private List<Appointment> CollectionOfAppointments()
        {
            var appointments = new List<Appointment>()
            {
                new Appointment()
                {
                    Id = 1,
                    StartDateTime = DateTime.ParseExact("23/12/2022 14:00:00", "dd/MM/yyyy HH:mm:ss",null),
                    PatientId = 1,
                    DentistId = 1,
                    IsActive = true,
                    Status = true
                },

                 new Appointment()
                {
                    Id = 2,
                    StartDateTime = DateTime.ParseExact("30/12/2022 15:00:00", "dd/MM/yyyy HH:mm:ss",null),
                    PatientId = 1,
                    DentistId = 1,
                    Details = "If I'm late, I'll call",
                    IsActive= true,
                    Status = false
                },

            };

            return appointments;
        }
    }
}
