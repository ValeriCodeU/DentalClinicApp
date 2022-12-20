using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Dentists;
using DentalClinicApp.Core.Models.Patients;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DentalClinicApp.Core.Services
{
    public class DentistService : IDentistService
    {
        private readonly IRepository repo;

        public DentistService(IRepository _repo)
        {
            repo = _repo;
        }

        public async Task<DentistDetailsViewModel> GetAllManagedDentistsAsync(Guid userId)
        {

            return await repo.AllReadonly<Manager>()
               .Where(m => m.UserId == userId)
               .Select(m => new DentistDetailsViewModel()
               {
                   Dentists = m.AcceptedDentists
                   .Select(d => new DentistServiceModel()
                   {
                       Id = d.Id,
                       FirstName = d.User.FirstName,
                       LastName = d.User.LastName,
                       Email = d.User.Email,
                       PhoneNumber = d.User.PhoneNumber
                   })

               }).FirstAsync();


        }

        public async Task<int> GetDentistIdAsync(Guid userId)
        {
            var dentist = await repo.AllReadonly<Dentist>().FirstAsync(u => u.UserId == userId);

            return dentist.Id;
        }

        public async Task<DentistStatisticsViewModel> GetStatisticsAsync(int id)
        {
            var totalPatients = await repo.AllReadonly<Dentist>()
                .Where(d => d.Id == id && d.User.IsActive)
                .Select(p => p.Patients
                .Where(p => p.User.IsActive)
                .Count())
                .FirstAsync();


            var totalProcedures = await repo.AllReadonly<Dentist>()
               .Where(d => d.Id == id && d.User.IsActive)
               .Select(dp => dp.DentalProcedures
               .Where(dp => dp.IsActive)
               .Count())
               .FirstAsync();

            var totalAppointments = await repo.AllReadonly<Dentist>()
              .Where(d => d.Id == id && d.User.IsActive)
              .Select(da => da.Appointments
              .Where(da => da.IsActive)
              .Count())
              .FirstAsync();

            var totalAttendances = await repo.AllReadonly<Dentist>()
            .Where(d => d.Id == id && d.User.IsActive)
            .Select(dat => dat.Attendances
            .Where(dat => dat.IsActive)
            .Count())
            .FirstAsync();


            return await repo.AllReadonly<Dentist>()
                .Where(d => d.Id == id)
                .Select(d => new DentistStatisticsViewModel()
                {
                    FirstName = d.User.FirstName,
                    LastName = d.User.LastName,
                    Email = d.User.Email,
                    PhoneNumber = d.User.PhoneNumber,
                    TotalPatientsCount = totalPatients,
                    TotalProceduresCount = totalProcedures,
                    TotalAppointmentsCount = totalAppointments,
                    TotalAttendancesCount = totalAttendances

                }).FirstAsync();
            
        }

        public async Task<bool> IsExistsByIdAsync(Guid userId)
        {
            return await repo.AllReadonly<Dentist>().AnyAsync(d => d.UserId == userId && d.User.IsActive);
        }
    }
}
