using DentalClinicApp.Core.Models.Attendances;
using DentalClinicApp.Core.Models.DentalProblems;
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

		Task<bool> AttendanceExistsAsync(int id);

		Task<AttedanceServiceModel> AttendanceDetailsByIdAsync(int id);

		Task EditAttendanceAsync(AttendanceFormModel model, int attendanceId);

		Task<bool> DeleteAttendanceAsync(int id);

        Task<IEnumerable<AttendanceDetailsViewModel>> AllAttendancesByPatientIdAsync(int patientId);

        
    }
}
