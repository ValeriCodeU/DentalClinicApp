using DentalClinicApp.Core.Constants;
using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.DentalProblems;
using DentalClinicApp.Infrastructure.Data.Identity;
using HouseRentingSystem.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static DentalClinicApp.Core.Constants.RoleConstant;

namespace DentalClinicApp.Controllers
{
    public class ProblemController : BaseController
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProblemService problemService;
        private readonly IPatientService patientService;

        public ProblemController(
            UserManager<ApplicationUser> _userManager,
            IProblemService _problemService,
            IPatientService _patientService)
        {
            userManager = _userManager;
            problemService = _problemService;
            patientService = _patientService;
        }


        [HttpGet]
        [Authorize(Roles = PatientRoleName)]

        public IActionResult Create()
        {
            var model = new ProblemFormModel();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = PatientRoleName)]

        public async Task<IActionResult> Create(ProblemFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = this.User.Id();
            int patientId = await patientService.GetPatientIdAsync(userId);

            await problemService.CreateAsync(patientId, model);


            return RedirectToAction(nameof(MyProblems));
        }

        [Authorize(Roles = DentistRoleName)]
        [Authorize(Roles = PatientRoleName)]

        public async Task<IActionResult> Details(int id)
        {
            if (!await problemService.ProblemExistsAsync(id))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var model = await problemService.ProblemDetailsByIdAsync(id);
            

            return View(model);
        }

        [Authorize(Roles = PatientRoleName)]

        public async Task<IActionResult> Edit(int id)
        {
            var model = new ProblemFormModel();


            if (!await problemService.ProblemExistsAsync(id))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var problem = await problemService.ProblemDetailsByIdAsync(id);

            model.DiseaseName = problem.DiseaseName;
            model.DiseaseDescription = problem.DiseaseDescription;
            model.AlergyDescription = problem.AlergyDescription;
            model.DentalStatus = problem.DentalStatus;

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = PatientRoleName)]

        public async Task<IActionResult> Edit(ProblemFormModel model, int id)
        {
            if (!await problemService.ProblemExistsAsync(id))
            {
                ModelState.AddModelError("", "Dental problem does not exist!");

                return View(model);
            }

            if (!ModelState.IsValid)
            {               
                return View(model);
            }

            await problemService.EditProblemAsync(model, id);

            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Roles = PatientRoleName)]

        public async Task<IActionResult> MyProblems()
        {
            var userId = this.User.Id();

            if (!await patientService.IsExistsByIdAsync(userId))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            int patientId = await patientService.GetPatientIdAsync(userId);

            //if (!await problemService.ProblemExistsAsync(patientId))
            //{
            //    return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            //}

            //IEnumerable<ProblemDetailsViewModel> myProblems;

            var myProblems = await problemService.AllProblemsByPatientIdAsync(patientId);

            
                
            return View(myProblems);
        }

        [Authorize(Roles = PatientRoleName)]

        public async Task<IActionResult> Delete(int id)
        {
            var problem = await problemService.GetProblemByIdAsync(id);

            if (problem == null)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var userId = this.User.Id();

           

            return View(problem);
        }

        [HttpPost]

        public async Task<IActionResult> Delete(ProblemDetailsViewModel model)
        {

            if (!await problemService.ProblemExistsAsync(model.Id))
            {
                TempData[MessageConstant.ErrorMessage] = "Dental problem does not exist!";
                return RedirectToAction(nameof(MyProblems));                
            }

            await problemService.DeleteAsync(model.Id);

            TempData[MessageConstant.SuccessMessage] = "You successfully deleted this dental problem!";

            return RedirectToAction(nameof(MyProblems));

        }
    }
}
