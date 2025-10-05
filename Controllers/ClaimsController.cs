using Microsoft.AspNetCore.Mvc;

namespace Paper_Route.Controllers
{
    public class ClaimsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
