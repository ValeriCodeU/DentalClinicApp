using DentalClinicApp.Core.Models.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Models.Dentists
{
	public class DentistDetailsViewModel
	{
        public IEnumerable<DentistServiceModel> Dentists { get; set; } = new List<DentistServiceModel>();
    }
}
