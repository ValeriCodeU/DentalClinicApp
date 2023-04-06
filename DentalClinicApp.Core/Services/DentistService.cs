using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Dentists;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DentalClinicApp.Core.Services
{
    /// <summary>
    ///  Manipulates dentist data
    /// </summary>
    public class DentistService : IDentistService
    {
        private readonly IRepository repo;

        public DentistService(IRepository _repo)
        {
            repo = _repo;
        }

        /// <summary>
        /// Create a user as a dentist        
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="managerId"></param>
        /// <returns>Boolean data type to check for a successful operation</returns>
        public async Task<bool> AddUserAsDentistAsync(Guid userId, int managerId)
        {
            var dentist = new Dentist()
            {
                UserId = userId,
                ManagerId = managerId,
            };

            await repo.AddAsync(dentist);
            await repo.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Get all managed dentists
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of dentists</returns>
        public async Task<DentistDetailsViewModel> GetAllManagedDentistsAsync(Guid userId)
        {

            return await repo.AllReadonly<Manager>()
               .Where(m => m.UserId == userId)
               .Select(m => new DentistDetailsViewModel()
               {
                   Dentists = m.AcceptedDentists
                   .Where(d => d.User.IsActive)                   
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

        /// <summary>
        /// Get a dentist Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Integer Id for dentist</returns>
        public async Task<int> GetDentistIdAsync(Guid userId)
        {
            var dentist = await repo.AllReadonly<Dentist>().FirstAsync(u => u.UserId == userId);

            return dentist.Id;
        }

        /// <summary>
        /// Get a manager Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Integer Id for a manager</returns>
        public async Task<int> GetManagerOfDentistAsync(Guid userId)
        {
            var manager = await repo.AllReadonly<Manager>().FirstAsync(u => u.UserId == userId);

            return manager.Id;
        }

        /// <summary>
        /// Get statistic for a dentist
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View model for dentist statistic</returns>
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

        /// <summary>
        /// Check if the dentist exists
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Boolean data type if dentist exists</returns>
        public async Task<bool> IsExistsByIdAsync(Guid userId)
        {
            return await repo.AllReadonly<Dentist>().AnyAsync(d => d.UserId == userId && d.User.IsActive);
        }
    }
}
