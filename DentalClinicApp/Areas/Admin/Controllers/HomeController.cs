﻿using Microsoft.AspNetCore.Mvc;

namespace DentalClinicApp.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
