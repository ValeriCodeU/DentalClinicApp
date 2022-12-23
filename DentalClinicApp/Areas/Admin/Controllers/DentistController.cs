using DentalClinicApp.Core.Constants;
using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Dentists;
using DentalClinicApp.Infrastructure.Data.Identity;
using HouseRentingSystem.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static DentalClinicApp.Core.Constants.ModelConstant;

namespace DentalClinicApp.Areas.Admin.Controllers
{
    public class DentistController : BaseController
    {
        private readonly IDentistService dentistService;
        private readonly UserManager<ApplicationUser> userManager;

        public DentistController(
            IDentistService _dentistService,
             UserManager<ApplicationUser> _userManager)
        {
            dentistService = _dentistService;
            userManager = _userManager;
        }


        public async Task<IActionResult> MyDentists()
        {
            var userId = this.User.Id();

            var model = await dentistService.GetAllManagedDentistsAsync(userId);

            return View(model);
        }

        public async Task<IActionResult> Statistics(int id)
        {

            var model = await dentistService.GetStatisticsAsync(id);

            return View(model);
        }

        public IActionResult Create()
        {
            var model = new DentistFormModel();

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Create(DentistFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dentistUser = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber
            };
            var userId = this.User.Id();

            var managerId = await dentistService.GetManagerOfDentistAsync(userId);

            var result = await userManager.CreateAsync(dentistUser, model.Password);


            if (result.Succeeded)
            {
                if (await dentistService.AddUserAsDentistAsync(dentistUser.Id, managerId))
                {
                    await userManager.AddToRoleAsync(dentistUser, "Dentist");
                    TempData[MessageConstant.SuccessMessage] = "You have successfully added a new dentist!";

                    var dentistId = await dentistService.GetDentistIdAsync(dentistUser.Id);
                    return RedirectToAction(nameof(Statistics), new { id = dentistId });
                }
                TempData[MessageConstant.ErrorMessage] = "Something went wrong!";
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
    }
}
