using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Server.Data;
using ProjectManager.Server.Middleware;
using ProjectManager.Server.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices(builder);
var app = builder.Build();
StartApp(app);
return;

void ConfigureServices(WebApplicationBuilder webApplicationBuilder) {
    var connectionString = webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection") ??
                           throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    //https://learn.microsoft.com/en-gb/ef/core/dbcontext-configuration/#dbcontext-in-dependency-injection-for-aspnet-core
    webApplicationBuilder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

    webApplicationBuilder.Services.AddDatabaseDeveloperPageExceptionFilter();

    webApplicationBuilder.Services
        .AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<ApplicationDbContext>();

    webApplicationBuilder.Services.AddIdentityServer()
        .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

    webApplicationBuilder.Services.AddAuthentication()
        .AddIdentityServerJwt();

    webApplicationBuilder.Services.AddControllersWithViews();
    webApplicationBuilder.Services.AddControllers(); //?
    
    webApplicationBuilder.Services.AddRazorPages();
    webApplicationBuilder.Services.AddHttpClient(); //TODO lehet baj?
}

void StartApp(WebApplication webApplication) {
// Configure the HTTP request pipeline.
    if (webApplication.Environment.IsDevelopment()) {
        webApplication.UseMigrationsEndPoint();
        webApplication.UseWebAssemblyDebugging();
    }
    else {
        //webApplication.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        webApplication.UseHsts();
    }

    webApplication.UseMiddleware<ErrorHandlerMiddleware>();
    webApplication.UseHttpsRedirection();

    webApplication.UseBlazorFrameworkFiles();
    webApplication.UseStaticFiles();

    webApplication.UseRouting();

    webApplication.UseIdentityServer();
    webApplication.UseAuthorization();


    webApplication.MapRazorPages();
    webApplication.MapControllers();
    webApplication.MapFallbackToFile("index.html");

	webApplication.Run();
}
