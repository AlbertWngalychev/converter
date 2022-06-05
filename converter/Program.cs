using converter;
using converter.Converter;
using converter.Converter.Core;
using converter.Data;
using converter.Middlewares;
using converter.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
//Add history page GET /history/page/1
var builder = WebApplication.CreateBuilder(args);

AddServices(builder);

var app = builder.Build();

ConfigureServices(app);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

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
    services.AddScoped<IResultRepository, ResultRepository>();
    services.AddScoped<IStatusRepository, StatusRepository>();

    services.AddMemoryCache();
    services.AddSingleton<IModelRepositoryCache, ModelRepositoryCache>();
    
    services.AddSingleton<ConverterFileManagerBase<converter.Models.Convert>>(x =>
    {
        return new ConverterManager(Path.Combine(builder.Environment.WebRootPath, "wwwroot/Files"));
    });

    services.AddSingleton<IConverterFileManagerBaseObserver<converter.Models.Convert>>( x=>
    {
        var manager = x.GetService<ConverterFileManagerBase<converter.Models.Convert>>();
        var statusRepo = x.GetService<IStatusRepository>();

        return new AddStatusObserver(statusRepo, manager);
    });

}
void ConfigureServices(WebApplication app)
{
    app.UseMiddleware<ErrorHandlingMiddleware>();

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