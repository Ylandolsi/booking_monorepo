using System.Net.Http.Json;
using System.Text.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Features.Authentication;
using Booking.Modules.Users.Features.Utils;
using Booking.Modules.Users.Presistence;
using IntegrationsTests.Abstractions.Authentication;
using IntegrationsTests.Abstractions.Base;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationsTests.Abstractions;

/// <summary>
/// Enhanced mentorship test base using the new multi-client system
/// Provides easy creation of mentor and mentee clients with proper cookie management
/// </summary>
public abstract class MentorshipTestBase : AuthenticationTestBase
{
    protected MentorshipTestBase(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    /// <summary>
    /// Creates an authenticated mentor with a unique userId
    /// Usage: var mentor = await CreateMentor("mentor1", 50.0m, "Expert in .NET");
    /// Then: await mentor.act.PostAsync(...);
    /// </summary>
    public async Task<(HttpClient arrange, HttpClient act)> CreateMentor(
        string userId = "mentor1",
        decimal hourlyRate = 75.0m,
        int bufferTimeMinutes = 15,
        string? email = null)
    {
        var (arrange, act) = GetClientsForUser(userId);

        // Register and login
        var loginData = await CreateUserAndLogin(null, null, arrange);

        // Become mentor
        var becomeMentorPayload = new
        {
            HourlyRate = hourlyRate,
            BufferTimeMinutes = bufferTimeMinutes,
        };

        var response = await arrange.PostAsJsonAsync(MentorshipEndpoints.Mentors.Become, becomeMentorPayload);
        response.EnsureSuccessStatusCode();

        return (arrange, act);
    }

    /// <summary>
    /// Creates an authenticated mentee with a unique userId
    /// </summary>
    public async Task<(HttpClient arrange, HttpClient act)> CreateMentee(string userId, string? email = null)
    {
        var (arrange, act) = GetClientsForUser(userId);
        var loginData = await CreateUserAndLogin(null, null, arrange);

        return (arrange, act);
    }


    public async Task<User> GetFullUserInfoBySlug(string userSlug)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        var user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Slug == userSlug);
        return user;
    }

    public async Task<(HttpClient arrange, HttpClient act)> CreateAdmin(string userId, string? email = null)
    {
        var (adminArrange, adminAct) = GetClientsForUser(userId);
        var loginData = await CreateUserAndLogin(null, null, adminArrange);
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        var roleService = scope.ServiceProvider.GetRequiredService<RoleService>();
        var user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Slug == loginData.UserSlug);
        await roleService.CreateRoleAsync("Admin");
        await roleService.AssignRoleToUserAsync("Admin", user.Id.ToString());
        return (adminArrange, adminAct);
    }

    #region Helper Methods

    private static IEnumerable<(DayOfWeek day, TimeOnly start, TimeOnly end)> GetDefaultWeeklyAvailability()
    {
        return new[]
        {
            (DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0)),
            (DayOfWeek.Tuesday, new TimeOnly(9, 0), new TimeOnly(17, 0)),
            (DayOfWeek.Wednesday, new TimeOnly(9, 0), new TimeOnly(17, 0)),
            (DayOfWeek.Thursday, new TimeOnly(9, 0), new TimeOnly(17, 0)),
            (DayOfWeek.Friday, new TimeOnly(9, 0), new TimeOnly(17, 0))
        };
    }

    protected static DateTime GetNextWeekday(DayOfWeek dayOfWeek, TimeOnly time)
    {
        var today = DateTime.Now.Date;
        var daysUntilTarget = ((int)dayOfWeek - (int)today.DayOfWeek + 7) % 7;
        if (daysUntilTarget == 0) daysUntilTarget = 7; // Next week

        var targetDate = today.AddDays(daysUntilTarget);
        return targetDate.Add(time.ToTimeSpan());
    }

    #endregion
}