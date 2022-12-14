using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Attendances;
using HouseRentingSystem.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DentalClinicApp.Core.Constants.RoleConstant;

namespace DentalClinicApp.Controllers
{
    public class AttendanceController : BaseController
    {

        private readonly IDentistService dentistService;
        private readonly IAttendanceService attendanceService;
        private readonly IPatientService patientService;

        public AttendanceController(
            IDentistService _dentistService,
            IAttendanceService _attendanceService,
            IPatientService _patientService)
        {
            dentistService = _dentistService;
            attendanceService = _attendanceService;
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

            var model = new AttendanceFormModel()
            {
                Patients = await patientService.GetPatientsAsync(userId)
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = DentistRoleName)]

        public async Task<IActionResult> Create(AttendanceFormModel model)
        {
            var userId = this.User.Id();

            if (!await dentistService.IsExistsByIdAsync(userId))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var dentistId = await dentistService.GetDentistIdAsync(userId);

            await attendanceService.CreateAsync(model, dentistId);

            return RedirectToAction(nameof(Details));
        }

        public async Task<IActionResult> Details()
        {
            return View();
        }

        public async Task<IActionResult> MyAttendances()
        {

            var userId = User.Id();

            IEnumerable<AttedanceServiceModel> model;

            model = await attendanceService.GetDentistAttendancesAsync(userId);

            //if (this.User.IsInRole("Dentist"))
            //{
            //    model = await attendanceService.GetDentistAttendancesAsync(userId);
            //}         


            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = new AttendanceFormModel();

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(AttendanceFormModel model)
        {
            return RedirectToAction(nameof(MyAttendances));
        }

    }
}
