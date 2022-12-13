using DentalClinicApp.Core.Models.Dentists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Contracts
{
    public interface IDentistService
    {
        Task<DentistDetailsViewModel> GetAllManagedDentistsAsync(Guid userId);
    }
}
