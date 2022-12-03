using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalClinicApp.Controllers
{
    [Authorize]

    public class BaseController : Controller
    {
       
    }
}
