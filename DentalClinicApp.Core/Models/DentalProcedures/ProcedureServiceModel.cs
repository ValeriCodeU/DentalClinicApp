using DentalClinicApp.Core.Models.Patients;

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

		public decimal Cost { get; set; }

        public PatientServiceModel Patient { get; set; } = null!;
    }
}
