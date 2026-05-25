using Microsoft.EntityFrameworkCore;
using POS.Database.Context;
using POS.Database.Interfaces;
using POS.Database.Repositories;
using POS.Domain.Interfaces;
using POS.Domain.Services;
using POS.Shared.Common;
using POS.Shared.Helpers;

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

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!dbContext.Users.Any(u => u.Id == SystemUser.DefaultCashierId))
    {
        dbContext.Database.ExecuteSqlInterpolated($@"
            SET IDENTITY_INSERT dbo.Users ON;
            INSERT INTO dbo.Users (Id, UserName, PasswordHash, Role, CreatedAt)
            VALUES ({SystemUser.DefaultCashierId}, {SystemUser.DefaultCashierUserName}, {string.Empty}, {SystemUser.DefaultCashierRole}, SYSUTCDATETIME());
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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
