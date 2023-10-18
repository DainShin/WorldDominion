using Microsoft.EntityFrameworkCore;
using WorldDominion.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add MySQL
var connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException
("Connection string not found");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySQL(connectionString));

// Enabling Sessions
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 쇼핑카트는 30분동안 저장됨
    options.Cookie.HttpOnly = true; // cookie only can come from the browser
    options.Cookie.IsEssential = true; 
});

var app = builder.Build();

// Turn on our seesions
app.UseSession(); 

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
    name: "About",
    pattern: "about",
    defaults: new{ controller="Home", action="About"}
);

app.MapControllerRoute(
    name: "Privacy",
    pattern: "privacy",
    defaults: new{ controller="Home", action="Privacy"}
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
