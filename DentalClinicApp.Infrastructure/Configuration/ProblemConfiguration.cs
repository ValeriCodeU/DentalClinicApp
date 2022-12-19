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
	internal class ProblemConfiguration : IEntityTypeConfiguration<DentalProblem>
	{
        public void Configure(EntityTypeBuilder<DentalProblem> builder)
        {
            builder.HasData(CollectionOfProblems());
        }

        private List<DentalProblem> CollectionOfProblems()
        {
            var problems = new List<DentalProblem>()
            {
                new DentalProblem()
                {
                    Id = 1,
                    DiseaseName = "Cracked tooth",
                    DiseaseDescription = "Playing boxing without a mouth guard",
                    AlergyDescription = "drug allergy",
                    DentalStatus = "H-55",
                    PatientId = 1,
                    IsActive = true,

                },

                new DentalProblem()
                {
                    Id = 2,
                    DiseaseName = "Sensitive to cold",
                    DiseaseDescription = "Pain when consuming cold drinks",
                    AlergyDescription = "not reported",
                    DentalStatus = "H-62",
                    PatientId = 1,
                    IsActive= true,
                },

            };

            return problems;
        }
    }
}
