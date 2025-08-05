using System.Net.Http.Json;
using System.Text.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Users.Features.Utils;

namespace IntegrationsTests.Abstractions;

/// <summary>
/// Enhanced mentorship test base using the new multi-client system
/// Provides easy creation of mentor and mentee clients with proper cookie management
/// </summary>
public abstract class EnhancedMentorshipTestBase : AuthenticationTestBase
{
    protected EnhancedMentorshipTestBase(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }
    
    /// <summary>
    /// Creates an authenticated mentor with a unique userId
    /// Usage: var mentor = await CreateMentor("mentor1", 50.0m, "Expert in .NET");
    /// Then: await mentor.act.PostAsync(...);
    /// </summary>
    protected async Task<(HttpClient arrange, HttpClient act)> CreateMentor(
        string userId, 
        decimal hourlyRate = 75.0m, 
        string? email = null)
    {
        var (arrange, act) = GetClientsForUser(userId);
        
        // Register and login
        var loginData = await CreateUserAndLogin(userId, email, arrange);
        
        // Become mentor
        var becomeMentorPayload = new
        {
            HourlyRate = hourlyRate
        };

        var response = await arrange.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, becomeMentorPayload);
        response.EnsureSuccessStatusCode();

        return (arrange, act);
    }

    /// <summary>
    /// Creates an authenticated mentee with a unique userId
    /// </summary>
    protected async Task<(HttpClient arrange, HttpClient act)> CreateMentee(string userId, string? email = null)
    {
        var (arrange, act) = GetClientsForUser(userId);
        var loginData = await CreateUserAndLogin(userId, email, arrange);

        return (arrange, act); 
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
