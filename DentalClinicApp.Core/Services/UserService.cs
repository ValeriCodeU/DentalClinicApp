using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Users;
using DentalClinicApp.Infrastructure.Data;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DentalClinicApp.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository repo;        
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(
            IRepository _repo,
            DentalClinicDbContext _dbContex,
            UserManager<ApplicationUser> _userManager)
        {
            repo = _repo;            
            userManager = _userManager;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await repo.GetByIdAsync<ApplicationUser>(id);

            user.IsActive = false;            
            await repo.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditUserAsync(UserEditViewModel model)
        {
            bool result = false;

            var user = await repo.GetByIdAsync<ApplicationUser>(model.Id);

            if (user != null && user.IsActive)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.Username;
                user.PhoneNumber = model.PhoneNumber;

                await repo.SaveChangesAsync();

                result = true;
            }

            return result;
        }

        public async Task<UserDeleteViewModel> GetUserForDeleteAsync(Guid id)
        {
            var user = await repo.GetByIdAsync<ApplicationUser>(id);

            return new UserDeleteViewModel()
            {
                Id = user.Id,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<ApplicationUser> GetUserByIdAsync(Guid id)
        {
            return await repo.GetByIdAsync<ApplicationUser>(id);
        }

        public async Task<UserEditViewModel> GetUserForEditAsync(Guid id)
        {
            var user = await repo.GetByIdAsync<ApplicationUser>(id);

            return new UserEditViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Username = user.UserName,               
            };
        }
       

        public async Task<IEnumerable<UserListViewModel>> GetUsersAsync()
        {
            var result = new List<UserListViewModel>();

            var users = await repo.AllReadonly<ApplicationUser>().Where(u => u.IsActive).ToListAsync();

            foreach (var u in users)
            {
                result.Add(new UserListViewModel()
                {
                    Id=u.Id,
                    Name = $"{u.FirstName} {u.LastName}",
                    Email = u.Email,
                    RoleNames = await userManager.GetRolesAsync(u)

                });
            }

            return result;           
        }
    }
}
