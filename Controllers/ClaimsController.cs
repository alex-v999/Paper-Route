using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Paper_Route.Models;
using Paper_Route.User;

namespace Paper_Route.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ClaimsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: /Claims
        public async Task<IActionResult> Index(string mode = "user")
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToAction("Login", "Account");

            List<UserClaim> claims;

            if (User.IsInRole("Worker") && mode == "worker")
            {
                claims = await _context.UserClaims
                    .OrderByDescending(c => c.DateCreated)
                    .ToListAsync();
                ViewBag.Mode = "worker";
            }
            else
            {
                claims = await _context.UserClaims
                    .Where(c => c.UserId == currentUser.Id)
                    .OrderByDescending(c => c.DateCreated)
                    .ToListAsync();
                ViewBag.Mode = "user";
            }

            return View(claims);
        }


        // GET: /Claims/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Claims/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserClaim model, List<IFormFile> attachments)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            // Assign UserId server-side
            model.UserId = currentUser.Id;
            model.DateCreated = DateTime.UtcNow;

            // Re-validate after setting UserId
           //if (!TryValidateModel(model))
                //return View(model);

            // Handle attachments
            if (attachments != null && attachments.Any())
            {
                var filePaths = new List<string>();
                foreach (var file in attachments)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        var path = Path.Combine("wwwroot/uploads/claims", fileName);
                        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                        using var stream = new FileStream(path, FileMode.Create);
                        await file.CopyToAsync(stream);
                        filePaths.Add("/uploads/claims/" + fileName);
                    }
                }
                model.Attachments = string.Join(';', filePaths);
            }

            _context.UserClaims.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Your claim has been successfully submitted.";
            return RedirectToAction("Index");
        }


    }
}
