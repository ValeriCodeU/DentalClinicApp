using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Dentists;
using DentalClinicApp.Core.Models.Patients;
using DentalClinicApp.Infrastructure.Data.Identity;
using HouseRentingSystem.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static DentalClinicApp.Core.Constants.ModelConstant;

namespace DentalClinicApp.Controllers
{
    public class PatientController : BaseController
    {
        private readonly IPatientService patientService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;

        public PatientController(
            IPatientService _patientService,
            UserManager<ApplicationUser> _userManager,
            IUserService _userService)
        {
            patientService = _patientService;
            userManager = _userManager;
            userService = _userService;
        }

        public async Task<IActionResult> Become()
        {
            var userId = this.User.Id();
            
            if (await patientService.IsExistsByIdAsync(userId))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }


            var model = new BecomePatientFormModel();
            model.Dentists = await patientService.GetDentistAsync();
            

            return View(model);
        }


        [HttpPost]

        public async Task<IActionResult> Become(BecomePatientFormModel model)
        {
            var dentistId = model.Id;

            var userId = this.User.Id();

            if (await patientService.IsExistsByIdAsync(userId))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }


            await patientService.CreatePatientAsync(userId, dentistId);

            var user = await userService.GetUserByIdAsync(userId);

            await userManager.AddToRoleAsync(user, "Patient");



            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> MyPatients()
        {
            var userId = this.User.Id();

            var patients = await patientService.GetMyPatients(userId);            

            return View(patients);
        }
    }
}
