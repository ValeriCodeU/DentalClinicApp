using DentalClinicApp.Core.Models.Patients;
using System.ComponentModel.DataAnnotations;

namespace DentalClinicApp.Core.Models.DentalProcedures
{
	public class ProcedureServiceModel
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public string Description { get; set; } = null!;

		public string? Note { get; set; }

		public string StartDate { get; set; } = null!; 

		public string EndDate { get; set; } = null!;

        [Range(typeof(decimal), "0.00", "1000.00", ConvertValueInInvariantCulture = true)]

        public decimal Cost { get; set; }

        public PatientServiceModel Patient { get; set; } = null!;
    }
}
