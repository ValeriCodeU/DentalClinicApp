using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Models.Patients
{
    public class MyPatientsViewModel
    {
        public IEnumerable<PatientServiceModel> Patients { get; set; } = new List<PatientServiceModel>();
    }
}
