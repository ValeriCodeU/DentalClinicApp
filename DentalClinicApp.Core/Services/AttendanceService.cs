using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Attendances;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Services
{
	public class AttendanceService : IAttendanceService
	{
		private readonly IRepository repo;

		public AttendanceService(IRepository _repo)
		{
			repo = _repo;
		}

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
					Date = a.Date.ToString(),
					Patient = new Models.Patients.PatientServiceModel()
					{
						Id = a.Patient.Id,					
					}

				}).FirstAsync();
		}

		public async Task<bool> AttendanceExistsAsync(int id)
		{
            return await repo.AllReadonly<Attendance>().AnyAsync(a => a.Id == id && a.IsActive);
        }

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

		public async Task<bool> DeleteAttendanceAsync(int id)
		{
			var attendance = await repo.GetByIdAsync<Attendance>(id);

			attendance.IsActive = false;
			await repo.SaveChangesAsync();

			return true;
		}

		public async Task EditAttendanceAsync(AttendanceFormModel model, int attendanceId)
		{
			var attendance = await repo.GetByIdAsync<Attendance>(attendanceId);

			attendance.ClinicRemarks = model.ClinicRemarks;
			attendance.Diagnosis = model.Diagnosis;
			attendance.Date = DateTime.Now;
			attendance.PatientId = model.PatientId;

			await repo.SaveChangesAsync();
		}

		public async Task<IEnumerable<AttedanceServiceModel>> GetDentistAttendancesAsync(Guid userId)
		{
			return await repo.AllReadonly<Dentist>()
				.Where(d => d.UserId == userId)
				.Where(d => d.User.IsActive)
				.Select(d => d.Attendances
				.Where(d => d.IsActive)
				.Select(a => new AttedanceServiceModel()
				{
					ClinicRemarks=a.ClinicRemarks,
					Diagnosis=a.Diagnosis,
					Id = a.Id,
					Date = a.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
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
