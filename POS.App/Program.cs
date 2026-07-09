using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using POS.Database.Context;
using POS.Database.Interfaces;
using POS.Database.Repositories;
using POS.Domain.Helpers;
using POS.Domain.Interfaces;
using POS.Domain.Services;
using POS.Shared.Common;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection"));
});
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<IGenerateInvoiceHelper, GenerateInvoiceHelper>();
builder.Services.AddScoped<ISalesService, SalesService>();
builder.Services.AddScoped<IVoidLog, VoidLogRepository>();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";

        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

// Configure Data Protection for production (important for shared hosting)
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "data-protection-keys")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Apply any pending migrations
    await dbContext.Database.MigrateAsync();

    int AdminId = SystemUser.DefaultAdminId; 
    if (!dbContext.Users.Any(u => u.Id == AdminId))
    {
        var defaultAdminHash = BCrypt.Net.BCrypt.HashPassword("Admin123");

        await dbContext.Database.ExecuteSqlInterpolatedAsync($@"
            SET IDENTITY_INSERT dbo.Users ON;
            INSERT INTO dbo.Users (Id, UserName, FullName, PasswordHash, Role, CreatedAt)
            VALUES ({AdminId}, {SystemUser.DefaultAdminUserName}, {SystemUser.DefaultAdminFullName}, {defaultAdminHash}, {SystemUser.AdminRole}, SYSUTCDATETIME());
            SET IDENTITY_INSERT dbo.Users OFF;");
    }
}



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
