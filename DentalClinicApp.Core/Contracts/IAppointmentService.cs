using DentalClinicApp.Core.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Contracts
{
    public interface IAppointmentService
    {
        Task CreateAsync(AppointmentFormModel model, int patientId, int dentistId);

        Task<AppointmentDetailsViewModel> GetDentistAppointments(Guid userId);

        Task<AppointmentServiceModel> GetAppointmentByIdAsync(int id);

        Task<bool> AcceptAppointmentByIdAsync(int id);

        Task<bool> PostponeAppointmentByIdAsync(int id);
    }
}
