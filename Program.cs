using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Paper_Route.Models;
using Paper_Route.Models.Consultations;
using Paper_Route.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var db = services.GetRequiredService<ApplicationDbContext>();

        // Ensure DB schema is up-to-date first
        db.Database.Migrate();

        // Seed roles
        string[] roles = { "User", "Worker", "Admin" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Seed consultation types
        var consultationTypes = new[]
        {
            new ConsultationType { Name = "General Consultation", Description = "A general consultation session." },
            new ConsultationType { Name = "Specialized Consultation", Description = "A specialized consultation session." },
            new ConsultationType { Name = "Document Inspections", Description = "A not so specialized consultation." }
        };

        foreach (var ct in consultationTypes)
        {
            if (!db.ConsultationTypes.Any(t => t.Name == ct.Name))
            {
                db.ConsultationTypes.Add(ct);
            }
        }

        // ---- Seed AvailableSlots for a rolling window ----
        // Adjust windowDays and timezone as needed for dev
        int windowDays = 30;
        TimeZoneInfo tz = TimeZoneInfo.Local; // or TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest")
        TimeSpan workStart = TimeSpan.FromHours(8);   // 08:00 local
        TimeSpan workEnd = TimeSpan.FromHours(15);    // 15:00 local (end exclusive)
        int slotMinutes = 15;

        DateTime todayLocal = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, tz).Date;
        var endLocal = todayLocal.AddDays(windowDays);

        for (var day = todayLocal; day < endLocal; day = day.AddDays(1))
        {
            // only Monday..Friday
            if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday) continue;

            for (var t = workStart; t < workEnd; t = t.Add(TimeSpan.FromMinutes(slotMinutes)))
            {
                var localStart = day + t; // local DateTime (unspecified kind)
                // Convert local to UTC safely
                var startUtc = TimeZoneInfo.ConvertTimeToUtc(localStart, tz);

                // skip past slots
                if (startUtc <= DateTime.UtcNow) continue;

                // uniqueness: avoid duplicates. Use StartUtc and null WorkerProfileId for global slots
                bool exists = await db.Set<AvailableSlot>()
                    .AnyAsync(s => s.StartUtc == startUtc && s.WorkerProfileId == null);

                if (!exists)
                {
                    db.Set<AvailableSlot>().Add(new AvailableSlot
                    {
                        StartUtc = startUtc,
                        DurationMinutes = slotMinutes,
                        State = SlotState.Available,
                        WorkerProfileId = null,
                        Note = null
                    });
                }
            }
        }

        await db.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
        throw;
    }
}

app.Run();
