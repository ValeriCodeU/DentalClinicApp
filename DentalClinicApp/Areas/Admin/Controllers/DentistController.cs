using Microsoft.AspNetCore.Mvc;

namespace DentalClinicApp.Areas.Admin.Controllers
{
    public class DentistController : BaseController
    {

        public async Task<IActionResult> GetStatistic()
        {
            return View();
        }

        
    }
}
