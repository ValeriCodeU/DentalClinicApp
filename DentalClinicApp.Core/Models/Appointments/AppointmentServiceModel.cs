using DentalClinicApp.Core.Models.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Models.Appointments
{
    public class AppointmentServiceModel 
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public string? Details { get; set; }

        public bool Status { get; set; }

        public PatientServiceModel Patient { get; set; } = null!;
    }
}
