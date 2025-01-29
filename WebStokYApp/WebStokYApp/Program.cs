using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WebStokYApp.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "CustomerCookie"; // Default olarak CustomerCookie se�iliyor
})
.AddCookie("CustomerCookie", options =>
{
    options.LoginPath = "/Customer/LoginCustomer"; // Yetkisiz m��teri y�nlendirme
    options.AccessDeniedPath = "/Customer/AccessDenied"; // Eri�im reddedildi�inde y�nlendirme
})
.AddCookie("AdminCookie", options =>
{
    options.LoginPath = "/Customer/LoginAdmin"; // Yetkisiz admin y�nlendirme
    options.AccessDeniedPath = "/Admin/AccessDenied"; // Eri�im reddedildi�inde y�nlendirme
});

// Register Authorization services
builder.Services.AddAuthorization();

var app = builder.Build();

// Seed initial data into the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    SeedData.Initialize(services, context); // Veri taban�n� ba�latma
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Configure static file serving for Login pages
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Login_v1")),
    RequestPath = "/Login_v1"
});

app.UseRouting();

// Enable Authentication and Authorization middleware
app.UseAuthentication(); // Kimlik do�rulama middleware
app.UseAuthorization();  // Yetkilendirme middleware

// Map controller routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=AdminLogin}/{id?}");

app.Run();

