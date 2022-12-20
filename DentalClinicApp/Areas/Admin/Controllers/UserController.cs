using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Users;
using DentalClinicApp.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalClinicApp.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUserService userService;

        public UserController(
            RoleManager<IdentityRole<Guid>> _roleManager,
            UserManager<ApplicationUser> _userManager,
            IUserService _userService,
            SignInManager<ApplicationUser> _signInManager)
        {
            roleManager = _roleManager;
            userManager = _userManager;
            userService = _userService;
            signInManager = _signInManager;
        }

        public async Task<IActionResult> ManageUsers()
        {
            var users = await userService.GetUsersAsync();

            return View(users);
        }

        public async Task<IActionResult> SetRole(Guid id)
        {
            var user = await userService.GetUserByIdAsync(id);

            var model = new UserRolesViewModel()
            {
                Id = user.Id,
                Name = $"{user.FirstName} {user.LastName}"
            };


            ViewBag.RoleItems = roleManager.Roles
                .ToList()
                .Select(r => new SelectListItem()
                {
                    Text = r.Name,
                    Value = r.Name,
                    Selected = userManager.IsInRoleAsync(user, r.Name).Result
                })
                .ToList();

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> SetRole(UserRolesViewModel model)
        {
            var user = await userService.GetUserByIdAsync(model.Id);
            var userRoles = await userManager.GetRolesAsync(user);

            await userManager.RemoveFromRolesAsync(user, userRoles);

            if (model.RoleNames == null)
            {
                return RedirectToAction(nameof(ManageUsers));
            }

            if (model.RoleNames.Length > 0)
            {
                await userManager.AddToRolesAsync(user, model.RoleNames);
            }

            return RedirectToAction(nameof(ManageUsers));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await userService.GetUserForEditAsync(id);

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            if (await userService.EditUserAsync(model))
            {
                var user = await userService.GetUserByIdAsync(model.Id);

                await signInManager.SignOutAsync();
                await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("ManageUsers", "User", new { area = "Admin" });
            }

            return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
        }

        

        //public async Task<IActionResult> CreateRole()
        //{
        //    await roleManager.CreateAsync(new IdentityRole<Guid>()
        //    {
        //        Name = "Patient"
        //    });

        //    return Ok();
        //}

        //public async Task<IActionResult> EditRole()
        //{
        //    var role = await roleManager.FindByNameAsync("User");
        //    role.Name = "Patient";
        //    role.NormalizedName = "PATIENT";
        //    await roleManager.UpdateAsync(role);

        //    return Ok();
        //}
    }
}
