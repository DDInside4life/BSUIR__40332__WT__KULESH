using KULESH.UI.Data;
using KULESH.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString =
    builder.Configuration.GetConnectionString("SqlLiteConnection")
    ?? throw new InvalidOperationException("Connection string " +
    "'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    // Do not require confirmed account for sign-in (allow immediate login after registration)
    options.SignIn.RequireConfirmedAccount = false;

    // Allow simple passwords for testing / educational purposes
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequiredUniqueChars = 0;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Authorization policy: requires claim "role" with value "admin"
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("role", "admin"));
});

// Register a No-op email sender to simulate email confirmation
builder.Services.AddSingleton<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, KULESH.UI.Services.NoOpEmailSender>();

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

// Use HTTP clients to communicate with the API project
builder.Services.AddHttpClient<ITeamService, ApiTeamService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7002");
});
builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7002");
});

// temporary item
builder.Services.AddDbContext<TempContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

// Seed initial data (create admin user with role claim)
KULESH.UI.Data.DbInit.SeedData(app).GetAwaiter().GetResult();

app.Run();

