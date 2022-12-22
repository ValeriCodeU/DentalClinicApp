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

        Task<bool> IsExistsByIdAsync(Guid userId);

        Task<int> GetDentistIdAsync(Guid userId);

        Task<DentistStatisticsViewModel> GetStatisticsAsync(int id);

        Task<bool> AddUserAsDentistAsync(Guid userId, int managerId);

        Task<int> GetManagerOfDentistAsync(Guid userId);
    }
}
