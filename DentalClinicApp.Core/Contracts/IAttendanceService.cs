using DentalClinicApp.Core.Models.Attendances;
using DentalClinicApp.Core.Models.DentalProblems;
using ShoppingListApp.Core.Models.Products.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Contracts
{
	/// <summary>
	/// Manipulates attendance data
	/// </summary>
	public interface IAttendanceService
	{
        /// <summary>
        /// Create a new attendance
        /// </summary>
        /// <param name="model">Attendance form model</param>
        /// <param name="dentistId">Dentist identifier</param>
        /// <returns></returns>
        Task<int> CreateAsync(AttendanceFormModel model, int dentistId);

        /// <summary>
        /// Get attendance for dentist
        /// </summary>
        /// <param name="userId">User globally unique identifier</param>
        /// <returns>List of attendances</returns>
        //Task<IEnumerable<AttedanceServiceModel>> GetDentistAttendancesAsync(Guid userId);

        //with query search and paging

        Task<AttendanceQueryServiceModel> GetDentistAttendancesAsync(
            int dentistId,            
            AttendanceSorting sorting = AttendanceSorting.Newest,
            string? searchTerm = null,
            int currentPage = 1,
            int attendancesPerPage = 1            
            );

        /// <summary>
        /// Check if the attendance exists
        /// </summary>
        /// <param name="id">Attendance identifier</param>
        /// <returns>Boolean data type if attendance exists</returns>
        Task<bool> AttendanceExistsAsync(int id);

        /// <summary>
        /// Attendance details for patient
        /// </summary>
        /// <param name="id">Attendance identifier</param>
        /// <returns>Attendance data</returns>
        Task<AttedanceServiceModel> AttendanceDetailsByIdAsync(int id);

        /// <summary>
        /// Update attendance
        /// </summary>
        /// <param name="model">Attendance form model</param>
        /// <param name="attendanceId">Attendance identifier</param>
        /// <returns></returns>
        Task EditAttendanceAsync(AttendanceFormModel model, int attendanceId);

        /// <summary>
        /// Delete attendance
        /// </summary>
        /// <param name="id">Attendance identifier</param>
        /// <returns>Boolean data type if the attendance has been deleted</returns>
        Task<bool> DeleteAttendanceAsync(int id);

        /// <summary>
        /// Get all attendance details for patient
        /// </summary>
        /// <param name="patientId">Patient identifier</param>
        /// <returns>List of attendance details for patient</returns>
        Task<IEnumerable<AttendanceDetailsViewModel>> AllAttendancesByPatientIdAsync(int patientId);


      

    }
}
