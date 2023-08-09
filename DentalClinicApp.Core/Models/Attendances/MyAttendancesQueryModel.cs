using ShoppingListApp.Core.Models.Products.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DentalClinicApp.Core.Models.Attendances
{
	public class MyAttendancesQueryModel
	{
        public const int AttendancesPerPage = 10;

        public string? Patient { get; set; }

        [Display(Name = "Search by text")]

        public string? SearchTerm { get; set; }

        public AttendanceSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalAttendancesCount { get; set; }

        public IEnumerable<string> Patients = new List<string>();

        public IEnumerable<AttedanceServiceModel> Attendances { get; set; } = new List<AttedanceServiceModel>();
    }
}
