using DentalClinicApp.Core.Models.Attendances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Models.Patients
{
    public class PatientCardViewModel
    {
        public int Id { get; set; }

        public IEnumerable<AttendanceDetailsViewModel> CardAttendances { get; set; } = new List<AttendanceDetailsViewModel>();
    }
}
