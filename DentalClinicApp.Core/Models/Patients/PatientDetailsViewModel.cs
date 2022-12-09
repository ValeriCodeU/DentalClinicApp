using DentalClinicApp.Core.Models.DentalProblems;

namespace DentalClinicApp.Core.Models.Patients
{
	public class PatientDetailsViewModel : PatientServiceModel
	{
		public IEnumerable<ProblemDetailsViewModel> PatientProblems { get; set; } = new List<ProblemDetailsViewModel>();
	}
}
