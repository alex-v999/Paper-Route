using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Paper_Route.Models.Consultations;
using Paper_Route.User;

namespace Paper_Route.Controllers
{
    public class ConsultationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConsultationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Consultations
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var consultations = await _context.Consultations
                .Where(c => c.UserId == userId)
                .Include(c => c.Type)
                .OrderByDescending(c => c.ScheduledDateTime)
                .ToListAsync();

            ViewBag.ConsultationTypes = await _context.ConsultationTypes.OrderBy(t => t.Name).ToListAsync();

            return View(consultations);
        }

        // POST: /Consultations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConsultationViewModel vm)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("", "You must be logged in to create a consultation.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ConsultationTypes = await _context.ConsultationTypes.OrderBy(t => t.Name).ToListAsync();
                // Re-fetch user consultations to render index properly on validation error
                var consultations = await _context.Consultations
                    .Where(c => c.UserId == userId)
                    .Include(c => c.Type)
                    .OrderByDescending(c => c.ScheduledDateTime)
                    .ToListAsync();
                return View("Index", consultations);
            }

            // Combine date + time
            var scheduled = vm.ScheduledDate.Date + vm.ScheduledTime;

            // Validate working days/hours (example: Mon-Fri 09:00-17:00)
            if (scheduled.DayOfWeek == DayOfWeek.Saturday || scheduled.DayOfWeek == DayOfWeek.Sunday
                || scheduled.Hour < 9 || scheduled.Hour > 16)
            {
                ModelState.AddModelError(nameof(vm.ScheduledDate), "Selected date/time is outside allowed range (Mon-Fri 09:00-17:00).");
                ViewBag.ConsultationTypes = await _context.ConsultationTypes.OrderBy(t => t.Name).ToListAsync();
                var consultations = await _context.Consultations
                    .Where(c => c.UserId == userId)
                    .Include(c => c.Type)
                    .OrderByDescending(c => c.ScheduledDateTime)
                    .ToListAsync();
                return View("Index", consultations);
            }

            // Ensure type exists
            var type = await _context.ConsultationTypes.FindAsync(vm.ConsultationTypeID);
            if (type == null)
            {
                ModelState.AddModelError(nameof(vm.ConsultationTypeID), "Selected consultation type does not exist.");
                ViewBag.ConsultationTypes = await _context.ConsultationTypes.OrderBy(t => t.Name).ToListAsync();
                var consultations = await _context.Consultations
                    .Where(c => c.UserId == userId)
                    .Include(c => c.Type)
                    .OrderByDescending(c => c.ScheduledDateTime)
                    .ToListAsync();
                return View("Index", consultations);
            }

            var consultation = new Consultation
            {
                ConsultationTypeID = vm.ConsultationTypeID,
                ScheduledDateTime = scheduled,
                Description = vm.Description,
                UserId = userId,
                WorkerProfileID = null
            };

            try
            {
                _context.Consultations.Add(consultation);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Consultation created.";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Save failed: " + ex.GetBaseException().Message);
                ViewBag.ConsultationTypes = await _context.ConsultationTypes.OrderBy(t => t.Name).ToListAsync();
                var consultations = await _context.Consultations
                    .Where(c => c.UserId == userId)
                    .Include(c => c.Type)
                    .OrderByDescending(c => c.ScheduledDateTime)
                    .ToListAsync();
                return View("Index", consultations);
            }

            return RedirectToAction(nameof(Index));
        }

        // Optionally: GET: /Consultations/Types to return types as JSON (for SPA or AJAX)
        public async Task<IActionResult> Types()
        {
            var types = await _context.ConsultationTypes.OrderBy(t => t.Name).ToListAsync();
            return Json(types);
        }
    }
}
