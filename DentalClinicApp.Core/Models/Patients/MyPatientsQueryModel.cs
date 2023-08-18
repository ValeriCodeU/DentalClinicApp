using DentalClinicApp.Core.Models.Patients.Enums;
using System.ComponentModel.DataAnnotations;

namespace DentalClinicApp.Core.Models.Patients
{
    public class MyPatientsQueryModel
    {
        public const int PatientsPerPage = 10;

        [Display(Name = "Search by text")]

        public string? SearchTerm { get; set; }

        public PatientSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalPatientsCount { get; set; }

        public IEnumerable<PatientServiceModel> Patients { get; set; } = new List<PatientServiceModel>();
    }
}