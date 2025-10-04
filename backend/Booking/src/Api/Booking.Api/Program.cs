using System.Reflection;
using System.Text.Json.Serialization;
using Booking.Api;
using Booking.Api.Extensions;
using Booking.Api.Services;
using Booking.Common;
using Booking.Common.RealTime;
using Booking.Modules.Catalog;
using Booking.Modules.Catalog.Persistence;
using Booking.Modules.Users;
using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Features.Authentication;
using Booking.Modules.Users.Persistence;
 
using Booking.Modules.Users.RecurringJobs;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using AssemblyReference = Booking.Modules.Catalog.AssemblyReference;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); // Binds to 0.0.0.0:5000 (IPv4) and [::]:5000 (IPv6)
});
builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.ConfigureHttpJsonOptions(options =>
{
    // ignore circular references in JSON serialization 
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddControllers();
// builder.Services.AddAntiforgery();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();


Assembly[] moduleApplicationAssemblies =
[
    AssemblyReference.Assembly,
    //Booking.Modules.Mentorships.AssemblyReference.Assembly,
    Booking.Modules.Users.AssemblyReference.Assembly,
    Booking.Modules.Catalog.AssemblyReference.Assembly,
];


builder.Services.AddInfrastructure(builder.Configuration, builder);
builder.Services.AddApplication(moduleApplicationAssemblies);




//builder.Services.TryAddSingleton(typeof(IUserIdProvider), typeof(SignalRCustomUserIdProvider));
builder.Services.AddSignalR();


builder.Services.AddScoped<NotificationService>();


builder.Services.UseHangFire(builder.Configuration);


builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
// for each module add its own app.settings.json ..
// builder.Configuration.AddModuleConfiguration(["users"]);

builder.Services.AddCatalogModule(builder.Configuration);
builder.Services.AddUsersModule(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();

builder.Services.AddSingleton<TestProfileSeeder>();
/*
builder.Services.AddHostedService<SeedHostedService>();
*/



var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    //     need to handle seeding langugaes and expertise in a better way!
    // and once only

    app.MapOpenApi();
    app.MapScalarApiReference(opt => { opt.WithTitle("Booking API"); });
    app.UseSwaggerWithUi();
    
    
    
    // using var scope = app.Services.CreateScope();
    // var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    // var usersDb = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
    // var catalogDb = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
    // var roleService = scope.ServiceProvider.GetRequiredService<RoleService>();


    // // Drop databases
    // await usersDb.Database.EnsureDeletedAsync();
    // await catalogDb.Database.EnsureDeletedAsync();
    // app.ApplyMigrations();


    // // Delete test users only
    // var testUsers = await userManager.Users
    //     .ToListAsync();


    // foreach (var user in testUsers)
    // {
    //     await userManager.DeleteAsync(user);
    // }

    // await roleService.CreateRoleAsync("Admin"); // TODO : check if admin is created when we move to prod
    // await roleService.CreateRoleAsync("User");

    // var TestProfileSeeder = new TestProfileSeeder(app.Services);
    // await TestProfileSeeder.SeedComprehensiveUserProfilesAsync();
}

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseStaticFiles(); // Serves files from wwwroot by default : http://localhost:5000/logo.png.
app.UseGlobalExceptionHandler();
app.UseCors("DefaultCors");

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();
app.UseCancellationMiddleware();


app.UseAuthentication();

app.UseAuthorization();


// app.UseAntiforgery();

app.UseHangfireDashboard();

RecurringJobs.AddRecurringJobs();
app.MapControllers();
app.MapEndpoints();

app.MapHub<NotificationHub>("/hubs/notifications");

await app.RunAsync();


// REMARK: Required for functional and integration tests to work.
namespace Booking.Api
{
    public class Program;
}