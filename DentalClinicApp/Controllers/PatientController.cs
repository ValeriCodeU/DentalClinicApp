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
        private readonly IProblemService problemService;

        public PatientController(
            IPatientService _patientService,
            UserManager<ApplicationUser> _userManager,
            IUserService _userService,
            IProblemService _problemService)
        {
            patientService = _patientService;
            userManager = _userManager;
            userService = _userService;
            problemService = _problemService;
        }

        public async Task<IActionResult> Become()
        {
            var userId = this.User.Id();
            
            if (await patientService.IsExistsByIdAsync(userId))
            {
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

            await userManager.AddToRoleAsync(user, "Patient");



            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> MyPatients()
        {
            var userId = this.User.Id();

            var patients = await patientService.GetMyPatients(userId);            

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
