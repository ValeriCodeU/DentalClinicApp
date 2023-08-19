using DentalClinicApp.Core.Models.Users;
using DentalClinicApp.Core.Models.Users.Enums;
using DentalClinicApp.Infrastructure.Data.Identity;

namespace DentalClinicApp.Core.Contracts
{
    public interface IUserService
    {
        //Task<IEnumerable<UserListViewModel>> GetUsersAsync();

        Task<UserQueryServiceModel> GetUsersAsync(
            string? roleName = null,
            string? searchTerm = null,
            UserSorting sorting = UserSorting.Newest,
            int currentPage = 1,
            int usersPerPage = 1
            );

        Task<UserEditViewModel> GetUserForEditAsync(Guid id);

        Task<bool> EditUserAsync(UserEditViewModel model);

        Task<ApplicationUser> GetUserByIdAsync(Guid id);

        Task<bool> DeleteUserAsync(Guid id);

        Task<UserDeleteViewModel> GetUserForDeleteAsync(Guid id);
    }
}
