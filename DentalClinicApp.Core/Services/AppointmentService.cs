using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Appointments;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository repo;

        public AppointmentService(IRepository _repo)
        {
            repo = _repo;
        }

        public async Task CreateAsync(AppointmentFormModel model, int patientId, int dentistId)
        {

            var appointment = new Appointment()
            {
                DentistId = dentistId,
                PatientId = patientId,
                Details = model.Details,
                StartDateTime = model.StartDate,
                Status = model.Status
            };

            await repo.AddAsync<Appointment>(appointment);
            await repo.SaveChangesAsync();
        }

        public async Task<AppointmentDetailsViewModel> GetDentistAppointments(Guid userId)
        {
            return await repo.AllReadonly<Dentist>()
                .Where(d => d.User.IsActive)
                .Where(d => d.User.Id == userId)
                .Select(d => new AppointmentDetailsViewModel()
                {
                    Appointments = d.Appointments
                    .Where(a => a.IsActive)
                    .Select(a => new AppointmentServiceModel()
                    {
                        StartDate = a.StartDateTime,
                        Status = a.Status,
                        Details = a.Details,
                        Id = a.Id,
                        Patient = new Models.Patients.PatientServiceModel()
                        {
                            FirstName = a.Patient.User.FirstName,
                            LastName = a.Patient.User.LastName,
                            Email = a.Patient.User.Email,
                            PhoneNumber = a.Patient.User.PhoneNumber
                        }
                    }).ToList()

                }).FirstAsync();
        }
    }
}
