using DentalClinicApp.Core.Contracts;
using HouseRentingSystem.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DentalClinicApp.Areas.Admin.Controllers
{
    public class DentistController : BaseController
    {
        private readonly IDentistService dentistService;

        public DentistController(IDentistService _dentistService)
        {
            dentistService = _dentistService;
        }

        //public async Task<IActionResult> GetStatistic()
        //{
        //    return View();
        //}

        //public async Task<IActionResult> Accept()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Details()
        {
            var userId = this.User.Id();

            var model = await dentistService.GetAllManagedDentistsAsync(userId);

            return View(model);
        }


    }
}
