using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WorldDominion.Models;
using WorldDominion.Services;

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
    options.Cookie.IsEssential = true; // 
});

// Adding identity service and roles
builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Registering the DbInitializer seeder
builder.Services.AddTransient<DbInitializer>();  

// Register Google Auth
builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});

builder.Services.AddScoped<CartService>(); // AddScoped는 서비스 컨테이너에 서비스를 등록하느 메서드 중 하나

var app = builder.Build(); // 서비스는 항상 이 코드 전에

// Turn on our seesions
app.UseSession(); // controller로 가기 전까지 미들웨어와 interact. 

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

// Setup authentication & authorization
app.UseAuthentication();
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

app.MapRazorPages();

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using var scope = scopeFactory.CreateScope();
// var Initiallizer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
await DbInitializer.Initiallize(
    scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>(),
    scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>()
);

app.Run();
