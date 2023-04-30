
using NToastNotify;
using CIProjectweb.Entities.DataModels;
using CIProjectweb.Repository.Repository;
using CIProjectweb.Repository.Repository.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Admin = CIProjectweb.Repository.Repository.Admin;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorPages().AddNToastNotifyNoty(new NotyOptions
{
    ProgressBar = true,
    Timeout = 5000
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CIDbContext>();
builder.Services.AddScoped<ILogin, Login>();
builder.Services.AddScoped<IAdmin, Admin>();
builder.Services.AddScoped<IUserInterface,UserInterface>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60 * 1);
    options.LoginPath = "/Home/Login";
    options.AccessDeniedPath = "/Home/Login";
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseNToastNotify();
//app.UseEndpoints(endpoints =>
//{
//    // Default route
//    endpoints.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Home}/{action=Login}/{id?}");

//    // Area route after default route
//    endpoints.MapControllerRoute(
//        name: "Admin",
//        pattern: "{area}/{controller=Admin}/{action=Index}/{id?}",
//        defaults: new { area = "Admin" }); // Replace "Admin" with the name of your area
//});
app.MapAreaControllerRoute(
            name: "MyAreaProducts",
            areaName: "Admin",
            pattern: "Admin/{controller=Admin}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();
