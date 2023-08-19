using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Appointments;
using DentalClinicApp.Core.Models.Appointments.Enums;
using DentalClinicApp.Core.Models.Attendances;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ShoppingListApp.Core.Models.Products.Enums;
using System.Globalization;
using System.Linq;

namespace DentalClinicApp.Core.Services
{

    /// <summary>
    /// Manipulates appointment data
    /// </summary>
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository repo;

        public AppointmentService(IRepository _repo)
        {
            repo = _repo;
        }

        /// <summary>
        ///  Аccept appointment
        /// </summary>
        /// <param name="id">Appointment Id</param>
        /// <returns>>Boolean data type if appointment is accepted</returns>       
        public async Task<bool> AcceptAppointmentByIdAsync(int id)
        {
            var appointment = await repo.GetByIdAsync<Appointment>(id);

            appointment.Status = true;
            await repo.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Create a new appointment
        /// </summary>
        /// <param name="model">Appointment form model</param>
        /// <param name="patientId">Patient Id</param>
        /// <param name="dentistId">Dentist Id</param>
        /// <returns></returns>
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

        /// <summary>
        ///  Get appointment by Id
        /// </summary>
        /// <param name="id">Appointment Id</param>
        /// <returns>Appointment data</returns>       
        public async Task<AppointmentServiceModel> GetAppointmentByIdAsync(int id)
        {
            return await repo.AllReadonly<Appointment>()
                .Where(a => a.IsActive)
                .Where(a => a.Id == id)
                .Select(a => new AppointmentServiceModel()
                {
                    Id = a.Id,                    
                    StartDate = a.StartDateTime,
                    Details = a.Details,
                    Patient = new Models.Patients.PatientServiceModel()
                    {
                        FirstName = a.Patient.User.FirstName,
                        LastName = a.Patient.User.LastName,
                    }

                }).FirstAsync();
        }

        /// <summary>
        /// Get appointments for dentist or patient
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List of appointments</returns>
        public async Task<AppointmentQueryServiceModel> GetAppointmentsAsync(
            int clientId,
            bool isPatient,
            AppointmentSorting sorting = AppointmentSorting.Newest,
            string? searchTerm = null,
            int currentPage = 1,
            int appointmentsPerPage = 1
            )
        {
            var result = new AppointmentQueryServiceModel();

            IQueryable<Appointment> appointments = repo.AllReadonly<Appointment>();

            if (isPatient)
            {
                appointments = repo.AllReadonly<Appointment>()
                .Where(a => a.PatientId == clientId && a.IsActive);
            }
            else
            {
                appointments = repo.AllReadonly<Appointment>()
                .Where(a => a.DentistId == clientId && a.IsActive);
            }

            if (!String.IsNullOrEmpty(searchTerm))
            {
                searchTerm = $"%{searchTerm.ToLower()}%";

                appointments = appointments
                    .Where(a => EF.Functions.Like(a.Dentist.User.LastName.ToLower(), searchTerm) ||
                    EF.Functions.Like(a.Patient.User.FirstName.ToLower(), searchTerm) ||
                    EF.Functions.Like(a.Details.ToLower(), searchTerm));
            }

            appointments = sorting switch
            {
                AppointmentSorting.Newest => appointments.OrderBy(a => a.StartDateTime),
                AppointmentSorting.Latest => appointments.OrderByDescending(a => a.StartDateTime),
                _ => appointments.OrderByDescending(a => a.StartDateTime)
            };

            var appointmentList = await appointments.Skip((currentPage - 1) * appointmentsPerPage).Take(appointmentsPerPage)
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
               }).ToListAsync();

            result.TotalAppointmentsCount = await appointments.CountAsync();
            result.Appointments = appointmentList;


            return result;            
        }
        
        /// <summary>
        /// Postpone appointment
        /// </summary>
        /// <param name="id">Appointment Id</param>
        /// <returns>>Boolean data type if appointment is postponed</returns>
        public async Task<bool> PostponeAppointmentByIdAsync(int id)
        {
            var appointment = await repo.GetByIdAsync<Appointment>(id);

            appointment.Status = false;
            appointment.IsActive = false;
            await repo.SaveChangesAsync();

            return true;
        }
    }
}
