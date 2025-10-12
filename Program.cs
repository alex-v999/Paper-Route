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
            new ConsultationType { Name = "General Consultation", Description = "A general consultation session.", DefaultDurationMinutes = 15},
            new ConsultationType { Name = "Specialized Consultation", Description = "A specialized consultation session.", DefaultDurationMinutes = 45 },
            new ConsultationType { Name = "Document Inspections", Description = "A not so specialized consultation." , DefaultDurationMinutes = 25}
        };

        foreach (var ct in consultationTypes)
        {
            if (!db.ConsultationTypes.Any(t => t.Name == ct.Name))
            {
                db.ConsultationTypes.Add(ct);
            }
        }

        // Choose explicit timezone
        var tz = TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest");

        int windowDays = 45;
        TimeSpan workStart = TimeSpan.FromHours(8);
        TimeSpan workEnd = TimeSpan.FromHours(15);
        int slotMinutes = 15;

        DateTime todayLocal = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, tz).Date;
        var endLocal = todayLocal.AddDays(windowDays);

        for (var day = todayLocal; day < endLocal; day = day.AddDays(1))
        {
            if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday) continue;

            for (var t = workStart; t < workEnd; t = t.Add(TimeSpan.FromMinutes(slotMinutes)))
            {
                // create local DateTime as Unspecified so conversion uses tz correctly
                var localStart = DateTime.SpecifyKind(day + t, DateTimeKind.Unspecified);

                // convert from the chosen timezone to UTC
                var startUtc = TimeZoneInfo.ConvertTimeToUtc(localStart, tz);

                // debug: log first few values if needed
                // Console.WriteLine($"localStart={localStart:yyyy-MM-dd HH:mm} startUtc={startUtc:O} nowUtc={DateTime.UtcNow:O}");

                // skip strictly past slots (allow small clock skew)
                if (startUtc <= DateTime.UtcNow.AddMinutes(-1)) continue;

                bool exists = await db.AvailableSlots
                    .AnyAsync(s => s.StartUtc == startUtc && s.WorkerProfileId == null);

                if (!exists)
                {
                    db.AvailableSlots.Add(new AvailableSlot
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

        // persist inserted slots
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
