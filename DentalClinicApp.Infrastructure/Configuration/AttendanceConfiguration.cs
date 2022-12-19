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
    internal class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.HasData(CollectionOfAttendances());
        }

        private List<Attendance> CollectionOfAttendances()
        {
            var attendances = new List<Attendance>()
            {
                new Attendance()
                {
                    Id = 1,
                    ClinicRemarks = "You need a filling, a root canal, or treatment of your gums to replace tissue lost at the root.",
                    IsActive = true,
                    Diagnosis = "Cavities and worn tooth enamel",
                    PatientId = 1,
                    DentistId = 1,
                    Date = DateTime.Now,


                },

                new Attendance()
                {
                    Id = 2,
                    ClinicRemarks = "You need a root canal and a crown",
                    IsActive = true,
                    Diagnosis = "Fractured tooth",
                    PatientId = 1,
                    DentistId = 1,
                    Date = DateTime.Now,
                },

                 new Attendance()
                {
                    Id = 3,
                    ClinicRemarks = "No Problem",
                    IsActive = true,
                    Diagnosis = "No diagnosis",
                    PatientId = 1,
                    DentistId = 1,
                    Date = DateTime.Now,
                },

            };

            return attendances;

        }
    }
}
