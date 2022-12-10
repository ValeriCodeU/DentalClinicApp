using DentalClinicApp.Core.Models.DentalProblems;

namespace DentalClinicApp.Core.Models.Patients
{
    public class PatientServiceModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Email { get; set; } = null!;
        
    }
}