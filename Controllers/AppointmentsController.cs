using Microsoft.AspNetCore.Mvc;

namespace Paper_Route.Controllers
{
    public class AppointmentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
