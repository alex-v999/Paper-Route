using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Paper_Route.User;

public class DocumentsController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public DocumentsController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        if (await _userManager.IsInRoleAsync(user, "Worker"))
        {
            ViewBag.IsWorker = true;
            // Load all pending documents or worker-specific documents
        }
        else
        {
            ViewBag.IsWorker = false;
            // Load only user's own documents
        }

        return View();
    }
}
