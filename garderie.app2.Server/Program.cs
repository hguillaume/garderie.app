// nuget: Microsoft.EntityFrameworkCore.SqlServer
// nuget: Microsoft.EntityFrameworkCore.Tools
// dotnet tool install -g Microsoft.dotnet-openapi

using garderie.app2.Server.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
//    .AddIdentityCookies();

//builder.Services
//  .AddAuthentication(o =>
//  {
//      //o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//      //o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//  })
//  //.AddJwtBearer()
//  .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
//  {
//      o.ExpireTimeSpan = TimeSpan.FromMinutes(30); // optional
//  });

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    });

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    //.AddSignInManager<SignInManager<IdentityUser>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization();

// Configure Cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "LoginCookie";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(365 * 100); // Cookie expiration
    options.LoginPath = "/Account/Login"; // Redirect path for unauthorized users
    options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect path for access denied
    options.SlidingExpiration = true; // Renew cookie expiration on activity
});

// Add services to the container.
//builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Session configuration
//builder.Services.AddSession();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60 * 24 * 365 * 100); // Set session timeout to 100 years
    //options.Cookie.HttpOnly = true; // Make the session cookie HTTP only
    //options.Cookie.IsEssential = true; // Make the session cookie essential for the application
});
builder.Services.AddDistributedMemoryCache(); // Required for session storage

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.Cookie.SameSite = SameSiteMode.None;
//    //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
//});

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.Cookie.Name = "LoginCookie";
//        options.LoginPath = "/user/login";
//        //options.LogoutPath = "/Account/Logout";
//        options.Cookie.HttpOnly = false;
//        options.Cookie.SameSite = SameSiteMode.None;
//        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//        options.ExpireTimeSpan = TimeSpan.FromMinutes(60 * 24 * 365 * 100); // Set session timeout to 100 years
//    });

//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(policy =>
//    {
//        policy.WithOrigins("https://localhost:3450") // Update to your frontend origin
//               //policy.AllowAnyOrigin()
//              .AllowAnyHeader()
//              .AllowAnyMethod()
//              .AllowCredentials();
//    });
//});

// Add services
//builder.Services.AddAuthentication("CookieAuth")
//    .AddCookie("CookieAuth", options =>
//    {
//        options.Cookie.Name = "MyAppCookie";
//        options.ExpireTimeSpan = TimeSpan.FromMinutes(60 * 24 * 365 * 100); // Set session timeout to 100 years
//        //options.LoginPath = "/user/login";
//        //options.AccessDeniedPath = "/Account/AccessDenied";
//    });

// Identity configuration
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddIdentityCore<IdentityUser>(options =>
//{
//    options.Password.RequireDigit = false;
//    options.Password.RequiredLength = 6;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequireUppercase = false;

//    // Lockout settings.
//    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60 * 24 * 365);
//    options.Lockout.MaxFailedAccessAttempts = 5;
//    options.Lockout.AllowedForNewUsers = true;

//    // User settings.
//    options.User.AllowedUserNameCharacters =
//    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
//    options.User.RequireUniqueEmail = false;
//});

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    // Cookie settings
//    options.Cookie.HttpOnly = true;
//    options.ExpireTimeSpan = TimeSpan.FromMinutes(60 * 24 * 365);

//    options.LoginPath = "/Identity/Account/Login";
//    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
//    options.SlidingExpiration = true;
//});

var app = builder.Build();

app.MapIdentityApi<IdentityUser>();

app.UseSession();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

//app.UseCors();

app.Run();
