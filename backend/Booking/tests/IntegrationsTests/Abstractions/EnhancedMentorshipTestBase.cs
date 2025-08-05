using System.Net.Http.Json;
using System.Text.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Users.Features.Utils;

namespace IntegrationsTests.Abstractions;

/// <summary>
/// Enhanced mentorship test base using the new multi-client system
/// Provides easy creation of mentor and mentee clients with proper cookie management
/// </summary>
public abstract class EnhancedMentorshipTestBase : EnhancedIntegrationTestBase
{
    protected EnhancedMentorshipTestBase(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    #region Quick Actor Creation Methods

    /// <summary>
    /// Creates an authenticated mentor with a unique userId
    /// Usage: var mentor = await CreateMentor("mentor1", 50.0m, "Expert in .NET");
    /// Then: await mentor.act.PostAsync(...);
    /// </summary>
    protected async Task<MentorActor> CreateMentor(
        string userId, 
        decimal hourlyRate = 75.0m, 
        string? email = null)
    {
        var (arrange, act) = GetClientsForUser(userId);
        
        // Register and login
        var loginData = await CreateUserAndLogin(email , null , arrange);
        
        // Become mentor
        var becomeMentorPayload = new
        {
            Bio = bio
        };

        var response = await arrange.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, becomeMentorPayload);
        // response.EnsureSuccessStatusCode();

        return new MentorActor(userId, arrange, act, loginData, hourlyRate, bio);
    }

    /// <summary>
    /// Creates an authenticated mentee with a unique userId
    /// </summary>
    protected async Task<MenteeActor> CreateMentee(string userId, string? email = null)
    {
        var (arrange, act) = GetClientsForUser(userId);
        var loginData = await CreateUserAndLogin(userId, email);
        
        return new MenteeActor(userId, arrange, act, loginData);
    }

    /// <summary>
    /// Creates multiple mentors at once
    /// Usage: var mentors = await CreateMentors(("senior", 100m), ("junior", 50m));
    /// </summary>
    protected async Task<MentorActor[]> CreateMentors(params (string userId, decimal hourlyRate, string bio)[] mentorConfigs)
    {
        var tasks = mentorConfigs.Select(config => 
            CreateMentor(config.userId, config.hourlyRate, config.bio));
        return await Task.WhenAll(tasks);
    }

    /// <summary>
    /// Creates multiple mentees at once
    /// </summary>
    protected async Task<MenteeActor[]> CreateMentees(params string[] userIds)
    {
        var tasks = userIds.Select(CreateMentee);
        return await Task.WhenAll(tasks);
    }

    #endregion

    #region Scenario Builder Methods

    /// <summary>
    /// Creates a complete mentorship scenario with mentor and mentee
    /// </summary>
    protected async Task<MentorshipScenario> CreateMentorshipScenario(
        string mentorId = "mentor",
        string menteeId = "mentee",
        decimal mentorRate = 75.0m,
        string mentorBio = "Expert software engineer")
    {
        var mentorTask = CreateMentor(mentorId, mentorRate, mentorBio);
        var menteeTask = CreateMentee(menteeId);

        await Task.WhenAll(mentorTask, menteeTask);

        var mentor = await mentorTask;
        var mentee = await menteeTask;

        return new MentorshipScenario(mentor, mentee);
    }

    /// <summary>
    /// Creates a scenario with multiple mentors and one mentee
    /// </summary>
    protected async Task<MultiMentorScenario> CreateMultiMentorScenario(
        string menteeId = "mentee",
        params (string mentorId, decimal rate, string bio)[] mentors)
    {
        var mentorTasks = mentors.Select(m => CreateMentor(m.mentorId, m.rate, m.bio));
        var menteeTask = CreateMentee(menteeId);

        await Task.WhenAll(mentorTasks.Concat(new[] { menteeTask.ContinueWith(t => (MentorActor?)null) }));

        var createdMentors = await Task.WhenAll(mentorTasks);
        var mentee = await menteeTask;

        return new MultiMentorScenario(createdMentors, mentee);
    }

    #endregion

    #region Business Logic Helper Methods

    /// <summary>
    /// Sets up weekly availability for a mentor
    /// </summary>
    protected async Task<List<int>> SetupMentorAvailability(MentorActor mentor, 
        IEnumerable<(DayOfWeek day, TimeOnly start, TimeOnly end)>? slots = null)
    {
        slots ??= GetDefaultWeeklyAvailability();
        var availabilityIds = new List<int>();

        foreach (var (day, start, end) in slots)
        {
            var availabilityPayload = new
            {
                DayOfWeek = day,
                StartTime = start.ToString("HH:mm"),
                EndTime = end.ToString("HH:mm")
            };

            var response = await mentor.Arrange.PostAsJsonAsync(MentorshipsEndpoints.SetAvailability, availabilityPayload);
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            availabilityIds.Add(result.GetProperty("AvailabilityId").GetInt32());
        }

        return availabilityIds;
    }

    /// <summary>
    /// Books a session between mentee and mentor
    /// </summary>
    protected async Task<int> BookSession(
        MenteeActor mentee, 
        MentorActor mentor, 
        DateTime scheduledAt,
        int durationMinutes = 60,
        string note = "Test session")
    {
        var bookSessionPayload = new
        {
            MentorSlug = mentor.LoginData.UserSlug.ToString(),
            ScheduledAt = scheduledAt,
            DurationMinutes = durationMinutes,
            Note = note
        };

        var response = await mentee.Act.PostAsJsonAsync(MentorshipsEndpoints.BookSession, bookSessionPayload);
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        return result.GetProperty("SessionId").GetInt32();
    }

    /// <summary>
    /// Confirms a session and adds Google Meet link
    /// </summary>
    protected async Task ConfirmSession(MentorActor mentor, int sessionId, string googleMeetUrl = "https://meet.google.com/abc-defg-hij")
    {
        var confirmPayload = new { GoogleMeetUrl = googleMeetUrl };
        var response = await mentor.Act.PutAsJsonAsync(
            MentorshipsEndpoints.ConfirmSession.Replace("{sessionId}", sessionId.ToString()),
            confirmPayload);
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Adds a review for a session
    /// </summary>
    protected async Task<int> AddReview(MenteeActor mentee, int sessionId, int rating = 5, string comment = "Excellent session!")
    {
        var reviewPayload = new
        {
            SessionId = sessionId,
            Rating = rating,
            Comment = comment
        };

        var response = await mentee.Act.PostAsJsonAsync(MentorshipsEndpoints.AddReview, reviewPayload);
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        return result.GetProperty("ReviewId").GetInt32();
    }

    #endregion

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

#region Actor Classes

/// <summary>
/// Represents a mentor actor in tests with associated HTTP clients and data
/// </summary>
public record MentorActor(
    string UserId,
    HttpClient Arrange,
    HttpClient Act,
    LoginResponse LoginData,
    decimal HourlyRate,
    string Bio)
{
    public string Slug => LoginData.UserSlug.ToString();
}

/// <summary>
/// Represents a mentee actor in tests
/// </summary>
public record MenteeActor(
    string UserId,
    HttpClient Arrange,
    HttpClient Act,
    LoginResponse LoginData)
{
    public string Slug => LoginData.UserSlug.ToString();
}

/// <summary>
/// Represents a complete mentorship testing scenario
/// </summary>
public record MentorshipScenario(MentorActor Mentor, MenteeActor Mentee)
{
    public async Task<List<int>> SetupMentorAvailability(EnhancedMentorshipTestBase testBase)
    {
        return await testBase.SetupMentorAvailability(Mentor);
    }

    public async Task<int> BookSession(EnhancedMentorshipTestBase testBase, DateTime scheduledAt, 
        int duration = 60, string note = "Test session")
    {
        return await testBase.BookSession(Mentee, Mentor, scheduledAt, duration, note);
    }
}

/// <summary>
/// Represents a scenario with multiple mentors and one mentee
/// </summary>
public record MultiMentorScenario(MentorActor[] Mentors, MenteeActor Mentee);

#endregion