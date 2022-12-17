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

        public ProcedureController(
            IDentistService _dentistService, 
            IPatientService _patientService)
        {
            dentistService = _dentistService;
            patientService = _patientService;
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
    }
}
