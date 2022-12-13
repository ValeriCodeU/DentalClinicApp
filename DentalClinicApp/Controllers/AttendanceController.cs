using Microsoft.AspNetCore.Mvc;

namespace DentalClinicApp.Controllers
{
    public class AttendanceController : BaseController
    {
        

        public async Task<IActionResult> Create()
        {
            return View();
        }
    }
}
