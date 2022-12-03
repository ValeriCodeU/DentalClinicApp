using DentalClinicApp.Core.Models.Users;
using DentalClinicApp.Infrastructure.Data.Identity;

namespace DentalClinicApp.Core.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserListViewModel>> GetUsersAsync();

        Task<UserEditViewModel> GetUserForEditAsync(Guid id);

        Task<bool> EditUserAsync(UserEditViewModel model);

        Task<ApplicationUser> GetUserByIdAsync(Guid id);       
    }
}
