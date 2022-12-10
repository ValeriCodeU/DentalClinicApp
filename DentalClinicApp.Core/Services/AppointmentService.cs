using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Appointments;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;
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
    }
}
