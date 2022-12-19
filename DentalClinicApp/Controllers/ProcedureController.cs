using DentalClinicApp.Core.Constants;
using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.DentalProcedures;
using HouseRentingSystem.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DentalClinicApp.Core.Constants.RoleConstant;

namespace DentalClinicApp.Controllers
{
    public class ProcedureController : BaseController
    {
        private readonly IDentistService dentistService;
        private readonly IPatientService patientService;
        private readonly IProcedureService procedureService;

        public ProcedureController(
            IDentistService _dentistService, 
            IPatientService _patientService,
            IProcedureService _procedureService)
        {
            dentistService = _dentistService;
            patientService = _patientService;
            procedureService = _procedureService;
        }

        [Authorize(Roles = DentistRoleName)]

        public async Task<IActionResult> Create()
        {
            var userId = this.User.Id();

            if (!await dentistService.IsExistsByIdAsync(userId))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var model = new ProcedureFormModel()
            {
                Patients = await patientService.GetPatientsAsync(userId)
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = DentistRoleName)]

        public async Task<IActionResult> Create(ProcedureFormModel model)
        {
            var userId = this.User.Id();

            if (!await dentistService.IsExistsByIdAsync(userId))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var dentistId = await dentistService.GetDentistIdAsync(userId);

            await procedureService.CreateAsync(model, dentistId);

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = DentistRoleName)]

        public async Task<IActionResult> Edit(int id)
        {

            if (!await procedureService.ProcedureExistsAsync(id))
            {
                TempData[MessageConstant.ErrorMessage] = "Procedure does not exist!";

                return RedirectToAction("Index", "Home");
            }

            var model = new ProcedureFormModel();

            var procedure = await procedureService.ProcedureDetailsByIdAsync(id);
           

            model.Name = procedure.Name;
            model.Description = procedure.Description;
            model.StartDate = procedure.StartDate;
            model.EndDate = procedure.EndDate;
            model.Cost = procedure.Cost;
            model.PatientId = procedure.Patient.Id;
            model.Patients = await patientService.GetPatientsAsync(this.User.Id());

            ViewBag.Date = model.StartDate;

            return View(model);
        }


        [HttpPost]
        [Authorize(Roles = DentistRoleName)]

        public async Task<IActionResult> Edit(ProcedureFormModel model, int id)
        {            

            if (!await procedureService.ProcedureExistsAsync(id))
            {
                TempData[MessageConstant.ErrorMessage] = "Procedure does not exist!";

                return RedirectToAction("Index", "Home");           
            }            

            await procedureService.EditProcedureAsync(model, id);

            return RedirectToAction(nameof(MyProcedures));
        }

        [Authorize(Roles = DentistRoleName)]

        public async Task<IActionResult> MyProcedures()
        {
            var userId = this.User.Id();

            IEnumerable<ProcedureServiceModel> model;

            model = await procedureService.GetDentistProceduresAsync(userId);

            return View(model);
        }

        [Authorize(Roles = PatientRoleName)]

        public async Task<IActionResult> Card()
        {
            var userId = this.User.Id();

            if (!await patientService.IsExistsByIdAsync(userId))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            int patientId = await patientService.GetPatientIdAsync(userId);

            var model = await procedureService.AllProceduresByPatientIdAsync(patientId);

            return View(model);
        }

        [Authorize(Roles = DentistRoleName + "," + PatientRoleName)]

        public async Task<IActionResult> Details(int id)
        {
            if (!await procedureService.ProcedureExistsAsync(id))
            {
                TempData[MessageConstant.ErrorMessage] = "Procedure does not exist!";

                return RedirectToAction("Index", "Home");
            }

            var model = await procedureService.ProcedureDetailsByIdAsync(id);

            return View(model);
        }

        [Authorize(Roles = DentistRoleName)]

        public async Task<IActionResult> Delete(int id)
        {
            if (!await procedureService.ProcedureExistsAsync(id))
            {
                TempData[MessageConstant.ErrorMessage] = "Dental procedure does not exist!";

                return RedirectToAction("Index", "Home");
            }

            var model = new ProcedureDeleteViewModel();

            var procedure = await procedureService.ProcedureDetailsByIdAsync(id);

            model.Name = procedure.Name;
            model.StartDate = procedure.StartDate;
            model.EndDate = procedure.EndDate;

            return View(model);

        }

        [HttpPost]
        [Authorize(Roles = DentistRoleName)]

        public async Task<IActionResult> Delete(ProcedureDeleteViewModel model)
        {
            if (!await procedureService.ProcedureExistsAsync(model.Id))
            {
                TempData[MessageConstant.ErrorMessage] = "Dental procedure does not exist!";

                return RedirectToAction("Index", "Home");
            }

            var result = await procedureService.DeleteProcedureAsync(model.Id);

            if (result)
            {
                TempData[MessageConstant.SuccessMessage] = "You successfully deleted this dental procedure!";
            }

            return RedirectToAction(nameof(MyProcedures));           
        }
    }
}
