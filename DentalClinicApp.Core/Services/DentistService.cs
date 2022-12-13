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
    }
}
