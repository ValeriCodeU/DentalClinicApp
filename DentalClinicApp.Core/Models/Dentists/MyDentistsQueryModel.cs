using DentalClinicApp.Core.Models.Dentists.Enums;
using System.ComponentModel.DataAnnotations;

namespace DentalClinicApp.Core.Models.Dentists
{
    public class MyDentistsQueryModel
    {
        public const int DentistsPerPage = 5;        

        [Display(Name = "Search by text")]

        public string? SearchTerm { get; set; }

        public DentistSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalDentistsCount { get; set; }        

        public IEnumerable<DentistServiceModel> Dentists { get; set; } = new List<DentistServiceModel>();
    }
}
