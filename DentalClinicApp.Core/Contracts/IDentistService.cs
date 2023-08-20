using DentalClinicApp.Core.Models.Dentists;
using DentalClinicApp.Core.Models.Dentists.Enums;

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
        /// <param name="userId">User globally unique identifier</param>
        /// <returns>List of dentists</returns>
        Task<DentistQueryServiceModel> GetAllManagedDentistsAsync(
            int managerId,            
            string? searchTerm = null,
            DentistSorting sorting = DentistSorting.Newest,
            int currentPage = 1,
            int dentistsPerPage = 1
            );

        /// <summary>
        /// Check if the dentist exists
        /// </summary>
        /// <param name="userId">User globally unique identifier</param>
        /// <returns>Boolean data type if dentist exists</returns>
        Task<bool> IsExistsByIdAsync(Guid userId);

        /// <summary>
        /// Get a dentist identifier
        /// </summary>
        /// <param name="userId">User globally unique identifier</param>
        /// <returns>Integer identifier for a dentist</returns>
        Task<int> GetDentistIdAsync(Guid userId);

        /// <summary>
        /// Get statistic for a dentist
        /// </summary>
        /// <param name="id">Dentist identifier</param>
        /// <returns>View model for dentist statistic</returns>
        Task<DentistStatisticsViewModel> GetStatisticsAsync(int id);

        /// <summary>
        /// Create a user as a dentist        
        /// </summary>
        /// <param name="userId">User globally uniqque identifier</param>
        /// <param name="managerId">Manager identifier</param>
        /// <returns>Boolean data type to check for a successful operation</returns>
        Task<bool> AddUserAsDentistAsync(Guid userId, int managerId);

        /// <summary>
        /// Get a manager identifier
        /// </summary>
        /// <param name="userId">User globally unique identifier</param>
        /// <returns>Integer identifier for a manager</returns>
        Task<int> GetManagerOfDentistAsync(Guid userId);
    }
}
