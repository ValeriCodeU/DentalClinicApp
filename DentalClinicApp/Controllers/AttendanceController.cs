using DentalClinicApp.Core.Constants;
using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Attendances;
using DentalClinicApp.Infrastructure.Data.Entities;
using HouseRentingSystem.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DentalClinicApp.Core.Constants.ModelConstant;
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

            return RedirectToAction(nameof(MyAttendances));
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
            var userId = this.User.Id();
            var model = new AttendanceFormModel();

            if (!await attendanceService.AttendanceExistsAsync(id))
            {
                TempData[MessageConstant.ErrorMessage] = "Attendance does not exist!";

                return RedirectToAction("Index", "Home");
            }

            var attendance = await attendanceService.AttendanceDetailsByIdAsync(id);
            
            model.ClinicRemarks = attendance.ClinicRemarks;
            model.Diagnosis = attendance.Diagnosis;
            model.PatientId = attendance.Patient.Id;
            model.Patients = await patientService.GetPatientsAsync(userId);            

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(AttendanceFormModel model, int id)
        {
            if (!await attendanceService.AttendanceExistsAsync(id))
            {
                TempData[MessageConstant.ErrorMessage] = "Attendance does not exist!";

                return RedirectToAction("Index", "Home");
            }

            await attendanceService.EditAttendanceAsync(model, id);

            return RedirectToAction(nameof(MyAttendances));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!await attendanceService.AttendanceExistsAsync(id))
            {
                TempData[MessageConstant.ErrorMessage] = "Attendance does not exist!";

                return RedirectToAction("Index", "Home");
            }           

            var attendance = await attendanceService.AttendanceDetailsByIdAsync(id);

            var model = new AttendanceDeleteViewModel()
            {
                Id = attendance.Id,
                ClinicRemarks = attendance.ClinicRemarks,
                Diagnosis = attendance.Diagnosis
            };

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Delete(AttendanceDeleteViewModel model)
        {
            if (!await attendanceService.AttendanceExistsAsync(model.Id))
            {
                TempData[MessageConstant.ErrorMessage] = "Attendance does not exist!";

                return RedirectToAction("Index", "Home");
            }

            var result = await attendanceService.DeleteAttendanceAsync(model.Id);

            if (result)
            {
                TempData[MessageConstant.SuccessMessage] = "You successfully deleted this attendance!";
            }            

            return RedirectToAction(nameof(MyAttendances));
        }

    }
}
