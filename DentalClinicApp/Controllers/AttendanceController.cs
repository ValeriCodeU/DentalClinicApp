using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Attendances;
using HouseRentingSystem.Extensions;
using Microsoft.AspNetCore.Mvc;

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
    }
}
