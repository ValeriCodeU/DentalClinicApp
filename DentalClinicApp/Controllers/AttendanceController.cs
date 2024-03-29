﻿using DentalClinicApp.Core.Constants;
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = this.User.Id();

            if (!await dentistService.IsExistsByIdAsync(userId))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var dentistId = await dentistService.GetDentistIdAsync(userId);

            var attendaceId = await attendanceService.CreateAsync(model, dentistId);

            return RedirectToAction(nameof(Details), new { id = attendaceId });
        }

        [Authorize(Roles = DentistRoleName + "," + PatientRoleName)]

        public async Task<IActionResult> Details(int id)
        {
            if (!await attendanceService.AttendanceExistsAsync(id))
            {
                TempData[MessageConstant.ErrorMessage] = "Attendance does not exist!";

                return RedirectToAction(nameof(MyAttendances));
            }

            var model = await attendanceService.AttendanceDetailsByIdAsync(id);

            return View(model);
        }

        //[Authorize(Roles = DentistRoleName)]        

        //public async Task<IActionResult> MyAttendances()
        //{

        //    var userId = User.Id();

        //    IEnumerable<AttedanceServiceModel> model;

        //    model = await attendanceService.GetDentistAttendancesAsync(userId);

        //    //if (this.User.IsInRole("Dentist"))
        //    //{
        //    //    model = await attendanceService.GetDentistAttendancesAsync(userId);
        //    //}         


        //    return View(model);
        //}

        //with query and paging

        [Authorize(Roles = DentistRoleName + "," + PatientRoleName)]

        public async Task<IActionResult> MyAttendances([FromQuery]MyAttendancesQueryModel query)
        {            
            //var dentistId = await dentistService.GetDentistIdAsync(User.Id());
            var userId = this.User.Id();
            int clientId = 0;
            bool isPatient = false;

            if (this.User.IsInRole(PatientRoleName))
            {
                if (!await patientService.IsExistsByIdAsync(userId))
                {
                    return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
                }

                clientId = await patientService.GetPatientIdAsync(userId);
                isPatient = true;
            }
            else if (this.User.IsInRole(DentistRoleName))
            {
                clientId = await dentistService.GetDentistIdAsync(userId);
            }

            var attendancesPerPage = MyAttendancesQueryModel.AttendancesPerPage;

            var result = await attendanceService.GetAttendancesAsync(
                clientId,
                isPatient,
                query.Sorting,
                query.SearchTerm,
                query.CurrentPage,
                attendancesPerPage
                );

            query.Attendances = result.Attendances;
            query.TotalAttendancesCount = result.TotalAttendancesCount;

            return View(query);
        }

        [Authorize(Roles = DentistRoleName)]

        public async Task<IActionResult> Edit(int id)
        {
            
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
            model.Patients = await patientService.GetPatientsAsync(this.User.Id());            

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = DentistRoleName)]

        public async Task<IActionResult> Edit(AttendanceFormModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!await attendanceService.AttendanceExistsAsync(id))
            {
                TempData[MessageConstant.ErrorMessage] = "Attendance does not exist!";

                return RedirectToAction("Index", "Home");
            }

            await attendanceService.EditAttendanceAsync(model, id);

            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Roles = DentistRoleName)]

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
        [Authorize(Roles = DentistRoleName)]

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

        [Authorize(Roles = PatientRoleName)]

        public async Task<IActionResult> Card()
        {
            var userId = this.User.Id();

            if (!await patientService.IsExistsByIdAsync(userId))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            int patientId = await patientService.GetPatientIdAsync(userId);

            var model = await attendanceService.AllAttendancesByPatientIdAsync(patientId);


            return View(model);
        }

    }
}
