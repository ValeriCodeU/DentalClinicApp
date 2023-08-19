using ShoppingListApp.Core.Models.Products.Enums;
using System.ComponentModel.DataAnnotations;

namespace DentalClinicApp.Core.Models.Attendances
{
    public class MyAttendancesQueryModel
	{
        public const int AttendancesPerPage = 10;        

        [Display(Name = "Search by text")]

        public string? SearchTerm { get; set; }

        public AttendanceSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalAttendancesCount { get; set; }        

        public IEnumerable<AttedanceServiceModel> Attendances { get; set; } = new List<AttedanceServiceModel>();
    }
}
