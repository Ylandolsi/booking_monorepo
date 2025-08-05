using System.Reflection;
using Booking.Api.DependencyInjection;
using Booking.Api.Extensions;
using Booking.Common;
using Booking.Modules.Mentorships;
using Booking.Modules.Users;
using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Presistence.Seed.Users;
using Booking.Modules.Users.RecurringJobs;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();

Assembly[] moduleApplicationAssemblies =
[
    Booking.Modules.Mentorships.AssemblyReference.Assembly ,
    Booking.Modules.Users.AssemblyReference.Assembly 
];


builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(moduleApplicationAssemblies);



builder.Services.AddEmailSender(builder.Configuration);


builder.Services.UseHangFire(builder.Configuration);


builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
builder.Configuration.AddModuleConfiguration(["users"]);

builder.Services.AddUsersModule(builder.Configuration);
builder.Services.AddMentorshipsModule(builder.Configuration);



WebApplication app = builder.Build();
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    //     need to handle seeding langugaes and expertise in a better way!
    // and once only

    app.UseSwaggerWithUi();
    app.MapOpenApi();
    app.MapScalarApiReference(opt => { opt.WithTitle("Booking API"); });
    app.ApplyMigrations();

    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    // Delete test users only
    var testUsers = await userManager.Users
        .ToListAsync();



    foreach (var user in testUsers)
    {
        await userManager.DeleteAsync(user);
    }

    var testUser = User.Create("test-user-slg",
        "Test",
        "User",
        "yesslandolsi@gmail.com",
        "");
    
    var TestProfileSeeder = new TestProfileSeeder(app.Services);
    await TestProfileSeeder.SeedComprehensiveUserProfilesAsync();


    var result = await userManager.CreateAsync(testUser, "Password123!");

    if (result.Succeeded)
    {
        var emailToken = await userManager.GenerateEmailConfirmationTokenAsync(testUser);
        await userManager.ConfirmEmailAsync(testUser, emailToken);

        Console.WriteLine("✅ Seeded verified user: yesslandolsi@gmail.com/ Password123!");
    }
    else
    {
        Console.WriteLine("❌ Failed to seed user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseStaticFiles(); // Serves files from wwwroot by default : http://localhost:5000/logo.png.
app.UseGlobalExceptionHandler();
app.UseCors("DefaultCors");
app.UseHangfireDashboard();

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();
app.UseCancellationMiddleware();

RecurringJobs.AddRecurringJobs();



app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

await app.RunAsync();

// REMARK: Required for functional and integration tests to work.
namespace Booking.Api
{
    public partial class Program;
}