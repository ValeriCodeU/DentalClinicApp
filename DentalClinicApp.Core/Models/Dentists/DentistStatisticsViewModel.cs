using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Models.Dentists
{
    public class DentistStatisticsViewModel : DentistServiceModel
    {
        public int TotalPatientsCount { get; set; }

        public int TotalProceduresCount { get; set; }

        public int TotalAppointmentsCount { get; set; }

        public int TotalAttendancesCount { get; set; }
    }
}
