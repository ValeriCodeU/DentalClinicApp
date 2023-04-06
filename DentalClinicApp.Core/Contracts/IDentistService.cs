using DentalClinicApp.Core.Models.Dentists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Contracts
{
    /// <summary>
    ///  Manipulates dentist data
    /// </summary>
    public interface IDentistService
    {
        /// <summary>
        /// Get all managed dentists
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of dentists</returns>
        Task<DentistDetailsViewModel> GetAllManagedDentistsAsync(Guid userId);

        /// <summary>
        /// Check if the dentist exists
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Boolean data type if dentist exists</returns>
        Task<bool> IsExistsByIdAsync(Guid userId);

        /// <summary>
        /// Get a dentist Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Integer Id for dentist</returns>
        Task<int> GetDentistIdAsync(Guid userId);

        /// <summary>
        /// Get statistic for a dentist
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View model for dentist statistic</returns>
        Task<DentistStatisticsViewModel> GetStatisticsAsync(int id);

        /// <summary>
        /// Create a user as a dentist        
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="managerId"></param>
        /// <returns>Boolean data type to check for a successful operation</returns>
        Task<bool> AddUserAsDentistAsync(Guid userId, int managerId);

        /// <summary>
        /// Get a manager Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Integer Id for a manager</returns>
        Task<int> GetManagerOfDentistAsync(Guid userId);
    }
}
