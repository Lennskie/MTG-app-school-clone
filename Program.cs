using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mtg_app.Data;
using Seeders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".mtg-app.Session";
    options.Cookie.IsEssential = true;
});

builder.Services.AddDefaultIdentity<IdentityUser>(options => 
            // -- START NEW CODE BLOCK
        {
            options.SignIn.RequireConfirmedAccount = true;

            // Require settings
            options.Password.RequireDigit = true; 
            options.Password.RequiredLength = 8; 
            options.Password.RequireNonAlphanumeric = false; 
            options.Password.RequireUppercase = true; 
            options.Password.RequireLowercase = false; 
            options.Password.RequiredUniqueChars = 6;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30); 
            options.Lockout.MaxFailedAccessAttempts = 10; 
            options.Lockout.AllowedForNewUsers = true;
            // User settings
            options.User.RequireUniqueEmail = true;
                        
        }
        // -- END NEW CODE BLOCK
    )
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();



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
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

