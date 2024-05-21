using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MyGuitarShopData;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.File(Path.Combine("Logs", "error.log"), rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error);
});


builder.Services.AddControllersWithViews();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
     
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });


builder.Services.AddDbContext<MyGuitarShopContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyGuitarShopConnectionString")));


builder.Services.AddScoped<ProductDb>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseSerilogRequestLogging();
}


app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // This line must be before UseAuthorization
app.UseAuthorization();

// Define the default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

app.Run();