using converter.Data;
using converter.Models;
using converter.Converter;
using converter.Converter.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using converter;

var builder = WebApplication.CreateBuilder(args);

AddServices(builder);

var app = builder.Build();

ConfigureServices(app);

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void AddServices(WebApplicationBuilder builder)
{
    var services = builder.Services;

    services.AddControllersWithViews();

    services.AddEntityFrameworkSqlite().AddDbContext<convertContext>(options =>
    {
        options.UseSqlite(new SqliteConnection(builder.Configuration.GetConnectionString("filename")));
    });

    services.AddScoped<IConvertRepository, ConvertRepository>();
    services.AddScoped<IConvertRepository, ConvertRepository>();
    services.AddScoped<IConvertRepository, ConvertRepository>();

    services.AddModelRepositoryCache<ModelRepositoryCache>();

    services.AddConverterManager<converter.Models.Convert, ConverterManager>();

    services.AddSingleton<IConverterFileManagerBaseObserver<converter.Models.Convert>, AddStatusObserver>();


}

void ConfigureServices(WebApplication app)
{
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
}