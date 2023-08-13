using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Users;
using DentalClinicApp.Core.Models.Users.Enums;
using DentalClinicApp.Infrastructure.Data;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoppingListApp.Core.Models.Products.Enums;

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

                await userManager.UpdateAsync(user);

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


        public async Task<UserQueryServiceModel> GetUsersAsync(
            string? roleName = null,
            string? searchTerm = null,
            UserSorting sorting = UserSorting.Newest,
            int currentPage = 1,
            int usersPerPage = 1
            )
        {            
            var result = new UserQueryServiceModel();

            var users = repo.AllReadonly<ApplicationUser>().Where(u => u.IsActive).ToList();

            //if (!String.IsNullOrEmpty(userName))
            //{
            //    users = users.Where(u => $"{u.FirstName} {u.LastName}" == userName);
            //}

            if (!String.IsNullOrEmpty(roleName))
            {
                //var usersInRole = await userManager.GetUsersInRoleAsync(roleName);
                //var usersInRole = users.Where(u => userManager.IsInRoleAsync(u, roleName).Result).AsQueryable();

                users = users.Where(u => userManager.IsInRoleAsync(u, roleName).Result).ToList();

                //users = (IQueryable<ApplicationUser>)usersInRole;
                //users = usersInRole;
            }

            if (!String.IsNullOrEmpty(searchTerm))
            {
                searchTerm = $"%{searchTerm.ToLower()}%";

                users = users
                    .Where(u => EF.Functions.Like(u.FirstName.ToLower(), searchTerm) ||
                    EF.Functions.Like(u.LastName.ToLower(), searchTerm)).ToList();
            }

            if (sorting == UserSorting.Name)
            {
                users = users.OrderBy(u => u.FirstName).ThenBy(u => u.LastName).ToList();
            }

            else if (sorting == UserSorting.Newest)
            {
                users = users.OrderByDescending(u => u.Id).ToList();
            }
            else
            {
                users = users.OrderByDescending(u => u.Id).ToList();
            }

            //users = sorting switch
            //{
            //    UserSorting.Name => users.OrderBy(u => u.FirstName),
            //    UserSorting.Newest => users.OrderByDescending(u => u.Id),
            //    _ => users.OrderByDescending(a => a.Id)
            //};

            var userList = users.Skip((currentPage - 1) * usersPerPage).Take(usersPerPage).ToList();


            foreach (var u in userList)
            {
                result.Users.Add(new UserListViewModel()
                {
                    Id = u.Id,
                    Name = $"{u.FirstName} {u.LastName}",
                    Email = u.Email,
                    RoleNames = await userManager.GetRolesAsync(u)
                });
            }

            result.TotalUsersCount = users.Count();

            return result;
        }
    }
}
