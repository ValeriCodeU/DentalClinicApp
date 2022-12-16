using DentalClinicApp.Core.Models.Patients;

namespace DentalClinicApp.Core.Models.Attendances
{
    public class AttedanceServiceModel
    {
        public int Id { get; set; }

        public string ClinicRemarks { get; set; } = null!;

        public string Diagnosis { get; set; } = null!;

        public string Date { get; set; } = null!;

        public PatientServiceModel Patient { get; set; } = null!;
    }
}
