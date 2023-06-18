using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Attendances;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DentalClinicApp.Core.Services
{
	/// <summary>
	/// Manipulates attendance data
	/// </summary>
	public class AttendanceService : IAttendanceService
	{
		private readonly IRepository repo;

		public AttendanceService(IRepository _repo)
		{
			repo = _repo;
		}

        /// <summary>
        /// Get all attendance details for patient
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>List of attendance details for patient</returns>
        public async Task<IEnumerable<AttendanceDetailsViewModel>> AllAttendancesByPatientIdAsync(int patientId)
		{
			return await repo.AllReadonly<Patient>()
				.Where(p => p.Id == patientId)
				.Where(p => p.User.IsActive)
				.Select(a => a.Attendances
				.Where(a => a.IsActive)
				.Select(a => new AttendanceDetailsViewModel()
				{
					ClinicRemarks = a.ClinicRemarks,
					Diagnosis = a.Diagnosis,
					Date = a.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
					Id = a.Id,

				})).FirstAsync();
		}

        /// <summary>
        /// Attendance details for patient
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Attendance data</returns>
        public async Task<AttedanceServiceModel> AttendanceDetailsByIdAsync(int id)
		{
			return await repo.AllReadonly<Attendance>()
				.Where(a => a.Id == id)
				.Where(a => a.IsActive)
				.Select(a => new AttedanceServiceModel()
				{
					Id = a.Id,
					ClinicRemarks = a.ClinicRemarks,
					Diagnosis = a.Diagnosis,
					Date = a.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
					Patient = new Models.Patients.PatientServiceModel()
					{
						Id = a.Patient.Id,
						FirstName = a.Patient.User.FirstName,
						LastName = a.Patient.User.LastName,
						Email = a.Patient.User.Email,
						PhoneNumber = a.Patient.User.PhoneNumber
					}

				}).FirstAsync();
		}

        /// <summary>
        /// Check if the attendance exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean data type if attendance exists</returns>
        public async Task<bool> AttendanceExistsAsync(int id)
		{
            return await repo.AllReadonly<Attendance>().AnyAsync(a => a.Id == id && a.IsActive);
        }

        /// <summary>
        /// Create a new attendance
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dentistId"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(AttendanceFormModel model, int dentistId)
		{
			var attendance = new Attendance()
			{
				ClinicRemarks = model.ClinicRemarks,
				Diagnosis = model.Diagnosis,
				PatientId = model.PatientId,
				DentistId = dentistId,
				Date = DateTime.Now
            };

			await repo.AddAsync<Attendance>(attendance);
			await repo.SaveChangesAsync();

			return attendance.Id;
		}

        /// <summary>
        /// Delete attendance
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean data type if the attendance has been deleted</returns>
        public async Task<bool> DeleteAttendanceAsync(int id)
		{
			var attendance = await repo.GetByIdAsync<Attendance>(id);

			attendance.IsActive = false;
			await repo.SaveChangesAsync();

			return true;
		}

        /// <summary>
        /// Update attendance
        /// </summary>
        /// <param name="model"></param>
        /// <param name="attendanceId"></param>
        /// <returns></returns>
        public async Task EditAttendanceAsync(AttendanceFormModel model, int attendanceId)
		{
			var attendance = await repo.GetByIdAsync<Attendance>(attendanceId);

			attendance.ClinicRemarks = model.ClinicRemarks;
			attendance.Diagnosis = model.Diagnosis;
			attendance.Date = DateTime.Now;
			attendance.PatientId = model.PatientId;

			await repo.SaveChangesAsync();
		}

        /// <summary>
        /// Get attendance for dentist
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of attendances</returns>
        public async Task<IEnumerable<AttedanceServiceModel>> GetDentistAttendancesAsync(Guid userId)
		{
			return await repo.AllReadonly<Dentist>()
				.Where(d => d.UserId == userId)
				.Where(d => d.User.IsActive)
				.Select(d => d.Attendances
				.Where(d => d.IsActive)
				.OrderByDescending(d => d.Date)
				.Select(a => new AttedanceServiceModel()
				{
					ClinicRemarks = a.ClinicRemarks,
					Diagnosis = a.Diagnosis,
					Id = a.Id,
					Date = a.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Patient = new Models.Patients.PatientServiceModel()
					{
						FirstName = a.Patient.User.FirstName,
						LastName = a.Patient.User.LastName,
						Email = a.Patient.User.Email,
						PhoneNumber = a.Patient.User.PhoneNumber
					}

				})).FirstAsync();
		}
	}
}
