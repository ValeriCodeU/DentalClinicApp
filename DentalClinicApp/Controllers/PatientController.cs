using DentalClinicApp.Core.Constants;
using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Patients;
using DentalClinicApp.Infrastructure.Data.Identity;
using HouseRentingSystem.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static DentalClinicApp.Core.Constants.RoleConstant;

namespace DentalClinicApp.Controllers
{
    public class PatientController : BaseController
    {
        private readonly IPatientService patientService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUserService userService;
        private readonly IProblemService problemService;
        private readonly IProcedureService procedureService;
        private readonly IDentistService dentistService;

        public PatientController(
            IPatientService _patientService,
            UserManager<ApplicationUser> _userManager,
            IUserService _userService,
            IProblemService _problemService,
            SignInManager<ApplicationUser> _signInManager,
            IProcedureService _procedureService,
            IDentistService _dentistService)
        {
            patientService = _patientService;
            userManager = _userManager;
            userService = _userService;
            problemService = _problemService;
            signInManager = _signInManager;
            procedureService = _procedureService;
            dentistService = _dentistService;
        }

        [Authorize(Roles = UserRoleName)]

        public async Task<IActionResult> Become()
        {
            var userId = this.User.Id();

            if (await patientService.IsExistsByIdAsync(userId))
            {
                TempData[MessageConstant.ErrorMessage] = "You are already a patient";
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }


            var model = new BecomePatientFormModel();
            model.Dentists = await patientService.GetDentistsAsync();

            return View(model);
        }


        [HttpPost]
        [Authorize(Roles = UserRoleName)]


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

        [Authorize(Roles = DentistRoleName)]

        public async Task<IActionResult> MyPatients([FromQuery]MyPatientsQueryModel query)
        {
            var dentistId = await dentistService.GetDentistIdAsync(this.User.Id());

            var patientsPerPage = MyPatientsQueryModel.PatientsPerPage;

            var result = await patientService.GetMyPatientsAsync(
                dentistId,
                query.Sorting,
                query.SearchTerm,
                query.CurrentPage,
                patientsPerPage
                );

            query.Patients = result.Patients;
            query.TotalPatientsCount = result.TotalPatientsCount;

            return View(query);
        }

        [Authorize(Roles = DentistRoleName)]

        public async Task<IActionResult> PatientProblemDetails(int id)
        {
            var patientUserId = await patientService.GetUserIdByPatientId(id);

            if (!await patientService.IsExistsByIdAsync(patientUserId))
            {
                TempData[MessageConstant.ErrorMessage] = "This patient does not exist!";
                return RedirectToAction(nameof(MyPatients));
            }

            var model = await patientService.PatientDetailsByIdAsync(id);

            return View(model);
        }

        [Authorize(Roles = DentistRoleName)]

        public async Task<IActionResult> PatientAttendanceDetails(int id)
        {
            var patientUserId = await patientService.GetUserIdByPatientId(id);

            if (!await patientService.IsExistsByIdAsync(patientUserId))
            {
                TempData[MessageConstant.ErrorMessage] = "This patient does not exist!";
                return RedirectToAction(nameof(MyPatients));
            }

            var model = await patientService.PatientAttendanceDetailsByIdAsync(id);

            return View(model);
        }

        [Authorize(Roles = DentistRoleName)]

        public async Task<IActionResult> PatientProcedureDetails(int id)
        {
            var patientUserId = await patientService.GetUserIdByPatientId(id);

            if (!await patientService.IsExistsByIdAsync(patientUserId))
            {
                TempData[MessageConstant.ErrorMessage] = "This patient does not exist!";
                return RedirectToAction(nameof(MyPatients));
            }

            var model = await patientService.PatientProcedureDetailsByIdAsync(id);

            return View(model);
        }
    }
}
