using DentalClinicApp.Core.Models.Dentists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Models.Patients
{
    public class BecomePatientFormModel
    {
        public int Id { get; set; }

        public IEnumerable<DentistModel> Dentists { get; set; } = new List<DentistModel>();
    }
}