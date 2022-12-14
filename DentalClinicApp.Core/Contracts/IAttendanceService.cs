using DentalClinicApp.Core.Models.Attendances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Contracts
{
	public interface IAttendanceService
	{
		Task<int> CreateAsync(AttendanceFormModel model, int dentistId);

		Task<IEnumerable<AttedanceServiceModel>> GetDentistAttendancesAsync(Guid userId);
    }
}
