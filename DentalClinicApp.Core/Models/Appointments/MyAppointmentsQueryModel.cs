using DentalClinicApp.Core.Models.Appointments.Enums;
using System.ComponentModel.DataAnnotations;

namespace DentalClinicApp.Core.Models.Appointments
{
    public class MyAppointmentsQueryModel
    {
        public const int AppointmentsPerPage = 6;

        [Display(Name = "Search by text")]

        public string? SearchTerm { get; set; }

        public AppointmentSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalAppointmentsCount { get; set; }

        public IEnumerable<AppointmentServiceModel> Appointments { get; set; } = new List<AppointmentServiceModel>();

    }
}
