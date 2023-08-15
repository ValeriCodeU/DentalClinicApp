using DentalClinicApp.Core.Models.Attendances;
using DentalClinicApp.Core.Models.DentalProcedures.Enums;
using System.ComponentModel.DataAnnotations;

namespace DentalClinicApp.Core.Models.DentalProcedures
{
    public class MyProceduresQueryModel
	{
        public const int ProceduresPerPage = 10;        

        [Display(Name = "Search by text")]

        public string? SearchTerm { get; set; }

        public ProcedureSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalProceduresCount { get; set; }        

        public IEnumerable<ProcedureServiceModel> Procedures { get; set; } = new List<ProcedureServiceModel>();
    }
}
