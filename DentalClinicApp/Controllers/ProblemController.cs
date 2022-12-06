﻿using DentalClinicApp.Core.Contracts;
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



            return RedirectToAction("Index", "Home");
        }
    }
}
