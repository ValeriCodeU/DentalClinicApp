using DentalClinicApp.Core.Constants;
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
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUserService userService;
        private readonly IProblemService problemService;

        public PatientController(
            IPatientService _patientService,
            UserManager<ApplicationUser> _userManager,
            IUserService _userService,
            IProblemService _problemService,
            SignInManager<ApplicationUser> _signInManager)
        {
            patientService = _patientService;
            userManager = _userManager;
            userService = _userService;
            problemService = _problemService;
            signInManager = _signInManager;
        }

        public async Task<IActionResult> Become()
        {
            var userId = this.User.Id();
            
            if (await patientService.IsExistsByIdAsync(userId))
            {
                //TempData[MessageConstant.ErrorMessage] = "Вие вече сте пациент";
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
               
            }


            var model = new BecomePatientFormModel();
            model.Dentists = await patientService.GetDentistsAsync();
            

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

            await userManager.RemoveFromRoleAsync(user, "User");
            await userManager.AddToRoleAsync(user, "Patient");

            await signInManager.SignOutAsync();
            await signInManager.SignInAsync(user, isPersistent: false);


            TempData[MessageConstant.SuccessMessage] = "You have successfully become a patient!";

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> MyPatients()
        {
            var userId = this.User.Id();

            var result = await patientService.IsExistsByIdAsync(userId);

            if (result)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var patients = await patientService.GetMyPatientsAsync(userId);
          

            return View(patients);
        }

        public async Task<IActionResult> Details(int id)
        {
            //if (!await problemService.ProblemExistsAsync(id))
            //{
            //    return RedirectToAction(nameof(MyPatients));
            //}

            var model = await patientService.PatientDetailsByIdAsync(id);

            return View(model);
        }
    }
}
