using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Appointments;
using HouseRentingSystem.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DentalClinicApp.Core.Constants.RoleConstant;

namespace DentalClinicApp.Controllers
{
    public class AppointmentController : BaseController
    {
        private readonly IPatientService patientService;
        private readonly IAppointmentService appointmentService;
        private readonly IDentistService dentistService;    

        public AppointmentController(
            IPatientService _patientService,
            IAppointmentService _appointmentService,
            IDentistService _dentistService)
        {
            patientService = _patientService;
            appointmentService = _appointmentService;
            dentistService = _dentistService;
        }

        [Authorize(Roles = PatientRoleName)]

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
        [Authorize(Roles = PatientRoleName)]

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

        [Authorize(Roles = DentistRoleName)]

        public async Task<IActionResult> Accept(int id)
        {
            var model = await appointmentService.GetAppointmentByIdAsync(id);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = DentistRoleName)]

        public async Task<IActionResult> Accept(AppointmentServiceModel model)
        {
            if (await appointmentService.AcceptAppointmentByIdAsync(model.Id))
            {
                TempData["message"] = "You have successfully accepted an appointment!";
            }

            return RedirectToAction(nameof(MyAppointments));
        }

        [HttpGet]
        [Authorize(Roles = DentistRoleName)]

        public async Task<IActionResult> Postpone(int id)
        {
            var model = await appointmentService.GetAppointmentByIdAsync(id);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = DentistRoleName)]

        public async Task<IActionResult> Postpone(AppointmentServiceModel model)
        {
            await appointmentService.PostponeAppointmentByIdAsync(model.Id);

            if (await appointmentService.PostponeAppointmentByIdAsync(model.Id))
            {
                TempData["message"] = "You have successfully postponed an appointment!";
            }

            return RedirectToAction(nameof(MyAppointments));
        }

        [Authorize(Roles = DentistRoleName + "," + PatientRoleName)]

        public async Task<IActionResult> MyAppointments([FromQuery]MyAppointmentsQueryModel query)
        {
            var userId = this.User.Id();
            int clientId = 0;
            bool isParient = false;

            var model = new MyAppointmentsQueryModel();

            if (this.User.IsInRole(PatientRoleName))
            {
                clientId = await patientService.GetPatientIdAsync(userId);                
                isParient = true;
            }
            else if (this.User.IsInRole(DentistRoleName))
            {
                clientId = await dentistService.GetDentistIdAsync(userId);                
            }

            var appointmentsPerPage = MyAppointmentsQueryModel.AppointmentsPerPage;

            var result = await appointmentService.GetAppointmentsAsync(
                clientId,
                isParient,
                query.Sorting,
                query.Status,
                query.SearchTerm,
                query.CurrentPage,
                appointmentsPerPage
                );

            query.Appointments = result.Appointments;
            query.TotalAppointmentsCount = result.TotalAppointmentsCount;

            return View(query);           
        }
    }
}
