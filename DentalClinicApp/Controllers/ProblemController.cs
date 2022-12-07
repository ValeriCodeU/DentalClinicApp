using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models;
using DentalClinicApp.Core.Models.DentalProblems;
using DentalClinicApp.Infrastructure.Data.Identity;
using HouseRentingSystem.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Create()
        {
            var model = new ProblemFormModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProblemFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = this.User.Id();
            int patientId = await patientService.GetPatientIdAsync(userId);

            await problemService.CreateAsync(patientId, model);



            return RedirectToAction(nameof(Mine));
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!await problemService.ProblemExistsAsync(id))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var model = await problemService.ProblemDetailsByIdAsync(id);
            

            return View(model);
        }

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

        public async Task<IActionResult> Mine()
        {
            var userId = this.User.Id();
            int patientId = await patientService.GetPatientIdAsync(userId);

            //IEnumerable<ProblemDetailsViewModel> myProblems;

            var myProblems = await problemService.AllProblemsByPatientIdAsync(patientId);

            
                
            return View(myProblems);
        }

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
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await problemService.DeleteAsync(model.Id);

            return RedirectToAction(nameof(Mine));

        }
    }
}
