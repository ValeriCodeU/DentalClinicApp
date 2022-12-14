﻿using DentalClinicApp.Core.Contracts;
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

		public async Task<IEnumerable<AttedanceServiceModel>> GetDentistAttendancesAsync(Guid userId)
		{
			return await repo.AllReadonly<Dentist>()
				.Where(d => d.UserId == userId)
				.Where(d => d.User.IsActive)
				.Select(d => d.Attendances
				.Select(a => new AttedanceServiceModel()
				{
					ClinicRemarks=a.ClinicRemarks,
					Diagnosis=a.Diagnosis,
					Id = a.Id,
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
