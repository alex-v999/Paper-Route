using System;
using System.Diagnostics;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConsultationViewModel vm)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("", "You must be logged in to create a consultation.");
                return await ReturnIndexWithErrors(userId);
            }

            Debug.WriteLine("=== [Create] START ===");
            Debug.WriteLine($"[DEBUG] UserId: {userId}");
            Debug.WriteLine($"[DEBUG] SelectedSlotId: {vm.SelectedSlotId}");
            Debug.WriteLine($"[DEBUG] ConsultationTypeID: {vm.ConsultationTypeID}");
            Debug.WriteLine($"[DEBUG] Description: {vm.Description}");

            if (!vm.SelectedSlotId.HasValue)
            {
                ModelState.AddModelError("", "You must select a slot.");
                return await ReturnIndexWithErrors(userId);
            }

            try
            {
                using var tx = await _context.Database.BeginTransactionAsync();

                // Fetch the slot
                var slot = await _context.AvailableSlots
                    .FirstOrDefaultAsync(s => s.Id == vm.SelectedSlotId.Value);

                if (slot == null)
                {
                    ModelState.AddModelError("", "Selected slot not found.");
                    return await ReturnIndexWithErrors(userId);
                }

                if (slot.State != SlotState.Available)
                {
                    ModelState.AddModelError("", "Slot is no longer available. Please choose another.");
                    return await ReturnIndexWithErrors(userId);
                }

                if (slot.StartUtc <= DateTime.UtcNow)
                {
                    ModelState.AddModelError("", "Cannot book a past slot.");
                    return await ReturnIndexWithErrors(userId);
                }

                var type = await _context.ConsultationTypes.FindAsync(vm.ConsultationTypeID);
                if (type == null)
                {
                    ModelState.AddModelError(nameof(vm.ConsultationTypeID), "Invalid consultation type.");
                    return await ReturnIndexWithErrors(userId);
                }

                // Create consultation
                var consultation = new Consultation
                {
                    ConsultationTypeID = vm.ConsultationTypeID,
                    ScheduledDateTime = slot.StartUtc,
                    Description = vm.Description,
                    UserId = userId,
                    WorkerProfileID = slot.WorkerProfileId
                };

                _context.Consultations.Add(consultation);
                await _context.SaveChangesAsync();

                // Mark slot as booked
                slot.State = SlotState.Booked;
                slot.ConsultationId = consultation.Id;
                _context.AvailableSlots.Update(slot);
                await _context.SaveChangesAsync();

                await tx.CommitAsync();

                Debug.WriteLine($"[INFO] Consultation {consultation.Id} created for slot {slot.Id}.");
                TempData["Success"] = "Consultation created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ERROR] Unexpected exception caught:");
                Debug.WriteLine(ex);
                ModelState.AddModelError("", "Unexpected error: " + ex.GetBaseException().Message);
                return await ReturnIndexWithErrors(userId);
            }
        }

        private async Task<IActionResult> ReturnIndexWithErrors(string userId)
        {
            ViewBag.ConsultationTypes = await _context.ConsultationTypes
                .OrderBy(t => t.Name)
                .ToListAsync();

            var consultations = await _context.Consultations
                .Where(c => c.UserId == userId)
                .Include(c => c.Type)
                .OrderByDescending(c => c.ScheduledDateTime)
                .ToListAsync();

            return View("Index", consultations);
        }


        // Optionally: GET: /Consultations/Types to return types as JSON (for SPA or AJAX)
        public async Task<IActionResult> Types()
        {
            var types = await _context.ConsultationTypes.OrderBy(t => t.Name).ToListAsync();
            return Json(types);
        }

        [HttpGet]
        public async Task<IActionResult> Available(string date)
        {
            if (!DateTime.TryParse(date, out var localDate)) return BadRequest("invalid date");
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest");
            var startUtc = TimeZoneInfo.ConvertTimeToUtc(localDate.Date, tz);
            var endUtc = TimeZoneInfo.ConvertTimeToUtc(localDate.Date.AddDays(1), tz);

            var slots = await _context.AvailableSlots
                .Where(s => s.StartUtc >= startUtc && s.StartUtc < endUtc)
                .OrderBy(s => s.StartUtc)
                .Select(s => new {
                    id = s.Id,
                    startUtc = s.StartUtc.ToString("o"),
                    startLocal = TimeZoneInfo.ConvertTimeFromUtc(s.StartUtc, tz).ToString("HH:mm"),
                    durationMinutes = s.DurationMinutes,
                    state = s.State.ToString(),
                    note = s.Note
                }).ToListAsync();

            return Json(new { slots, message = slots.Count == 0 ? "No available slots for this day." : (string?)null });
        }

    }
}
