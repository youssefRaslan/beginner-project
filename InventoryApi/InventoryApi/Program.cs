using Microsoft.EntityFrameworkCore;
using InventoryApi.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Add AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add Identity services
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppDbContext>();

// Add Controllers and Views
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // لازم دي تكون مضافة علشان الـ Identity UI

// Build the app
var app = builder.Build();

// Map the default route for Products controller
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // مهمة لو في ملفات CSS/JS

app.UseRouting();

app.UseAuthentication(); // لازم قبل Authorization
app.UseAuthorization();

app.MapRazorPages(); // علشان صفحات Identity

app.Run();
