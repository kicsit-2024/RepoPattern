using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepoPattern.Data;
using RepoPattern.Models;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext") ?? throw new InvalidOperationException("Connection string 'AppDbContext' not found.")));
//builder.Services.AddScoped<UnitOfWork>();

//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache(); // Use in-memory cache to store session data
//builder.Services.AddDistributedSqlServerCache(options =>
//{
//    options.ConnectionString = builder.Configuration.GetConnectionString("AppDbContext"); // Your database connection string
//    options.SchemaName = "dbo";
//    options.TableName = "Sessions"; // The table where session data will be stored
//});


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Make cookie accessible only by HTTP requests
    options.Cookie.IsEssential = true; // Make the session cookie essential
});

builder.Services.AddAuthentication("CustomCookie")
    .AddCookie("CustomCookie", options =>
    {
        options.Cookie.Name = "MyCustomCookie"; // Set your custom cookie name
        options.LoginPath = "/Accounts/Login"; // Redirect to login page if not authenticated
        //options.AccessDeniedPath = "/Accounts/AccessDenied";
        options.LogoutPath = "/Accounts/Logout"; // Redirect to logout page
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set expiration time
    });


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseSession();
//app.Use(async (context, next) =>
//{
//    // Access session data in middleware
//    var userId = context.Session.GetString("LoggedInUserId");

//    if (!string.IsNullOrEmpty(userId))
//    {
//        var db = context.RequestServices.GetRequiredService<AppDbContext>();
//        var user = db.Users.Where(m => m.Id.ToString() == userId).FirstOrDefault();
//        builder.
//    }

//    // Continue processing the request
//    await next.Invoke();
//});

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
app.Run();

