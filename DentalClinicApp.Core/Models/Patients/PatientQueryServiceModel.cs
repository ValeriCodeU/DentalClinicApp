namespace DentalClinicApp.Core.Models.Patients
{
    public class PatientQueryServiceModel
	{
        public int TotalPatientsCount { get; set; }

        public IEnumerable<PatientServiceModel> Patients { get; set; } = new List<PatientServiceModel>();
    }
}
