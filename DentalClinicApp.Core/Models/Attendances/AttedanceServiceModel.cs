using DentalClinicApp.Core.Models.Patients;

namespace DentalClinicApp.Core.Models.Attendances
{
    public class AttedanceServiceModel
    {
        public int Id { get; set; }

        public string ClinicRemarks { get; set; }

        public string Diagnosis { get; set; }

        public PatientServiceModel Patient { get; set; }
    }
}
