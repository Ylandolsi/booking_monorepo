using System.Net;
using System.Net.Http.Json;
using Booking.Modules.Mentorships.Features;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships;

public class MenteeTests : MentorshipTestBase
{
    public MenteeTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task BookSession_ShouldCreateSession_WhenMentorIsAvailableAndMenteeIsAuthenticated()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        var (menteeData, menteeClient) = await CreateMenteeAndLogin();
        
        // Set mentor availability for next Monday
        await SetMentorAvailability(mentorClient, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));
        
        var scheduledAt = MentorshipTestData.Sessions.NextWeekMonday.AddHours(10); // Monday 10 AM

        var bookSessionPayload = new
        {
            MentorSlug = mentorData.UserSlug.ToString(),
            ScheduledAt = scheduledAt,
            DurationMinutes = 60,
            Note = "Looking forward to learning about clean architecture"
        };

        // Act
        var response = await menteeClient.PostAsJsonAsync(MentorshipsEndpoints.BookSession, bookSessionPayload);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task BookSession_ShouldReturnUnauthorized_WhenMenteeIsNotAuthenticated()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        
        var bookSessionPayload = new
        {
            MentorSlug = mentorData.UserSlug.ToString(),
            ScheduledAt = MentorshipTestData.Sessions.NextWeekMonday.AddHours(10),
            DurationMinutes = 60,
            Note = "Test session"
        };

        // Act
        var response = await _client.PostAsJsonAsync(MentorshipsEndpoints.BookSession, bookSessionPayload);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldReturnBadRequest_WhenMentorIsNotAvailable()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        var (menteeData, menteeClient) = await CreateMenteeAndLogin();
        
        // Don't set any availability for the mentor
        var scheduledAt = MentorshipTestData.Sessions.NextWeekMonday.AddHours(10);

        var bookSessionPayload = new
        {
            MentorSlug = mentorData.UserSlug.ToString(),
            ScheduledAt = scheduledAt,
            DurationMinutes = 60,
            Note = "Test session"
        };

        // Act
        var response = await menteeClient.PostAsJsonAsync(MentorshipsEndpoints.BookSession, bookSessionPayload);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldReturnBadRequest_WhenScheduledTimeIsInThePast()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        var (menteeData, menteeClient) = await CreateMenteeAndLogin();
        
        await SetMentorAvailability(mentorClient, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));

        var pastScheduledAt = DateTime.UtcNow.AddHours(-1); // 1 hour ago

        var bookSessionPayload = new
        {
            MentorSlug = mentorData.UserSlug.ToString(),
            ScheduledAt = pastScheduledAt,
            DurationMinutes = 60,
            Note = "Test session"
        };

        // Act
        var response = await menteeClient.PostAsJsonAsync(MentorshipsEndpoints.BookSession, bookSessionPayload);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(15)] // Too short
    [InlineData(5 * 60)] // Too long (5 hours)
    public async Task BookSession_ShouldReturnBadRequest_WhenDurationIsInvalid(int invalidDuration)
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        var (menteeData, menteeClient) = await CreateMenteeAndLogin();
        
        await SetMentorAvailability(mentorClient, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));

        var bookSessionPayload = new
        {
            MentorSlug = mentorData.UserSlug.ToString(),
            ScheduledAt = MentorshipTestData.Sessions.NextWeekMonday.AddHours(10),
            DurationMinutes = invalidDuration,
            Note = "Test session"
        };

        // Act
        var response = await menteeClient.PostAsJsonAsync(MentorshipsEndpoints.BookSession, bookSessionPayload);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetMenteeSessions_ShouldReturnEmptyList_WhenMenteeHasNoSessions()
    {
        // Arrange
        var (menteeData, menteeClient) = await CreateMenteeAndLogin();

        // Act
        var response = await menteeClient.GetAsync(MentorshipsEndpoints.GetMenteeSessions);

        // Assert
        response.EnsureSuccessStatusCode();
        var sessions = await response.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(sessions);
        Assert.Empty(sessions);
    }

    [Fact]
    public async Task GetMenteeSessions_ShouldReturnSessions_WhenMenteeHasBookedSessions()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        var (menteeData, menteeClient) = await CreateMenteeAndLogin();
        
        await SetMentorAvailability(mentorClient, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));
        await SetMentorAvailability(mentorClient, DayOfWeek.Tuesday, new TimeOnly(9, 0), new TimeOnly(17, 0));

        // Book multiple sessions
        await BookSession(menteeClient, mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(10), 60, "First session");
        await BookSession(menteeClient, mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekTuesday.AddHours(14), 90, "Second session");

        // Act
        var response = await menteeClient.GetAsync(MentorshipsEndpoints.GetMenteeSessions);

        // Assert
        response.EnsureSuccessStatusCode();
        var sessions = await response.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(sessions);
        Assert.Equal(2, sessions.Count);
    }

    [Fact]
    public async Task GetMenteeSessions_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Act
        var response = await _client.GetAsync(MentorshipsEndpoints.GetMenteeSessions);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CancelSession_ShouldCancelSession_WhenMenteeOwnsSession()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        var (menteeData, menteeClient) = await CreateMenteeAndLogin();
        
        await SetMentorAvailability(mentorClient, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));
        var sessionId = await BookSession(menteeClient, mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(10));

        var cancelPayload = new
        {
            CancellationReason = "Schedule conflict"
        };

        // Act
        var response = await menteeClient.PutAsJsonAsync(
            MentorshipsEndpoints.CancelSession.Replace("{sessionId}", sessionId.ToString()), 
            cancelPayload);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task CancelSession_ShouldReturnNotFound_WhenSessionDoesNotExist()
    {
        // Arrange
        var (menteeData, menteeClient) = await CreateMenteeAndLogin();
        
        var cancelPayload = new
        {
            CancellationReason = "Test"
        };

        // Act
        var response = await menteeClient.PutAsJsonAsync(
            MentorshipsEndpoints.CancelSession.Replace("{sessionId}", "99999"), 
            cancelPayload);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CancelSession_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var cancelPayload = new
        {
            CancellationReason = "Test"
        };

        // Act
        var response = await _client.PutAsJsonAsync(
            MentorshipsEndpoints.CancelSession.Replace("{sessionId}", "1"), 
            cancelPayload);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
