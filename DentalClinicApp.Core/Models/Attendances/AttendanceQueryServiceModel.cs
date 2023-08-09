using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Models.Attendances
{
	public class AttendanceQueryServiceModel
	{
        public int TotalAttendancesCount { get; set; }

        public IEnumerable<AttedanceServiceModel> Attendances { get; set; } = new List<AttedanceServiceModel>();
    }
}
