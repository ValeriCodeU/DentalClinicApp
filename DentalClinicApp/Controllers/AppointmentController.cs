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

            return RedirectToAction(nameof(MyAppointments));

        }

        public async Task<IActionResult> Details()
        {
            var userId = this.User.Id();

            var model = await appointmentService.GetDentistAppointments(userId);


            return View(model);
        }

        public async Task<IActionResult> Accept(int id)
        {
            var model = await appointmentService.GetAppointmentByIdAsync(id);

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Accept(AppointmentServiceModel model)
        {
            if (await appointmentService.AcceptAppointmentByIdAsync(model.Id))
            {
                TempData["message"] = "You have successfully accepted an appointment!";
            }

            return RedirectToAction(nameof(Details));
        }

        [HttpGet]

        public async Task<IActionResult> Postpone(int id)
        {
            var model = await appointmentService.GetAppointmentByIdAsync(id);

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Postpone(AppointmentServiceModel model)
        {
            await appointmentService.PostponeAppointmentByIdAsync(model.Id);

            if (await appointmentService.PostponeAppointmentByIdAsync(model.Id))
            {
                TempData["message"] = "You have successfully postponed an appointment!";
            }

            return RedirectToAction(nameof(Details));
        }

        public async Task<IActionResult> MyAppointments()
        {
            var userId = this.User.Id();

            var model = await appointmentService.GetPatientAppointments(userId);

            return View(model);
        }
    }
}
