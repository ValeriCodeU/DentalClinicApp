using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Appointments;
using HouseRentingSystem.Extensions;
using Microsoft.AspNetCore.Mvc;
using static DentalClinicApp.Core.Constants.ModelConstant;

namespace DentalClinicApp.Controllers
{
    public class AppointmentController : BaseController
    {
        private readonly IPatientService patientService;
        private readonly IAppointmentService appointmentService;

        public AppointmentController(
            IPatientService _patientService,
            IAppointmentService _appointmentService)
        {
            patientService = _patientService;
            appointmentService = _appointmentService;
        }
        public async Task<IActionResult> Make()
        {
            var model = new AppointmentFormModel();

            var userId = this.User.Id();

            if (!await patientService.IsExistsByIdAsync(userId))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            return View(model);
        }


        [HttpPost]

        public async Task<IActionResult> Make(AppointmentFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = this.User.Id();

            var patientId = await patientService.GetPatientIdAsync(userId);
            var personalDentist = await patientService.GetPersonalDentistIdAsync(patientId);

            await appointmentService.CreateAsync(model, patientId, personalDentist);

            return View(model);
        }

        public async Task<IActionResult> Details()
        {
            return View();
        }
    }
}
