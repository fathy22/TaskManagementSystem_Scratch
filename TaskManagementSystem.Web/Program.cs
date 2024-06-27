using Application.Attachments;
using Application.CustomLogs;
using Application.DashboardReports;
using Application.Roles;
using Application.TaskComments;
using Application.TaskSheets;
using Application.Teams;
using Application.UnitOfWorks;
using Application.Users;
using DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Authorization.Roles;
using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.Core.Sessions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TaskManagementSystemDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<TaskManagementSystemDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddScoped<ICustomSession, CustomSession>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ITeamAppService, TeamAppService>();
builder.Services.AddScoped<IAttachmentAppService, AttachmentAppService>();
builder.Services.AddScoped<ITaskSheetAppService, TaskSheetAppService>();
builder.Services.AddScoped<ITaskCommentAppService, TaskCommentAppService>();
builder.Services.AddScoped<IUserAppService, UserAppService>();
builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();
builder.Services.AddScoped<ICustomLogAppService, CustomLogAppService>();
builder.Services.AddScoped<IDashboardReportAppService, DashboardReportAppService>();
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".TaskManagementSystem.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust timeout as needed
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Make the session cookie essential
});

// Add IHttpContextAccessor for accessing sessions
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AccessDenied", policy =>
        policy.RequireAssertion(context =>
            context.User.Identity.IsAuthenticated &&
            !context.User.IsInRole(StaticRoleNames.Host.RegularUsers) &&
            !context.User.IsInRole(StaticRoleNames.Host.TeamLeads)
        )
    );
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Home/AccessDenied"; // Specify your access denied path
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();
app.UseSession();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");
app.MapRazorPages();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await TaskManagementSystem.Application.Roles.RoleManager.SeedRolesAndUsers(services);
}
app.Run();
