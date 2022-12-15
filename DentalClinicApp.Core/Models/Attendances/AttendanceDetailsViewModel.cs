using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Models.Attendances
{
    public class AttendanceDetailsViewModel 
    {
        public int Id { get; set; }

        public string ClinicRemarks { get; set; }

        public string Diagnosis { get; set; }

        public string Date { get; set; }
    }
}
