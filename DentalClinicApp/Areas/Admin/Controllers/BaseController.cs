using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DentalClinicApp.Areas.Admin.Constants.AdminConstant;

namespace DentalClinicApp.Areas.Admin.Controllers
{
    [Area(AreaName)]
    [Authorize(Roles = AdminRoleName)]

    public class BaseController : Controller
    {
      
    }
}
