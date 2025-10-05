using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Paper_Route.User;

public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        // Load profile info
        var profile = new UserProfile(); 
        return View(profile);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UserProfile model)
    {
        if (!ModelState.IsValid) return View("Index", model);

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();


        return RedirectToAction("Index");
    }
}
