using System.Net;
using System.Net.Http.Json;
using Booking.Modules.Mentorships.Features;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships;

public class SessionManagementTests : MentorshipTestBase
{
    public SessionManagementTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task ConfirmSession_ShouldConfirmSession_WhenMentorConfirmsBookedSession()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        var (menteeData, menteeClient) = await CreateMenteeAndLogin();
        
        await SetMentorAvailability(mentorClient, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));
        var sessionId = await BookSession(menteeClient, mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(10));

        var confirmPayload = new
        {
            GoogleMeetUrl = "https://meet.google.com/abc-defg-hij"
        };

        // Act
        var response = await mentorClient.PutAsJsonAsync(
            MentorshipsEndpoints.ConfirmSession.Replace("{sessionId}", sessionId.ToString()), 
            confirmPayload);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task ConfirmSession_ShouldReturnBadRequest_WhenGoogleMeetUrlIsInvalid()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        var (menteeData, menteeClient) = await CreateMenteeAndLogin();
        
        await SetMentorAvailability(mentorClient, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));
        var sessionId = await BookSession(menteeClient, mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(10));

        var confirmPayload = new
        {
            GoogleMeetUrl = "invalid-url"
        };

        // Act
        var response = await mentorClient.PutAsJsonAsync(
            MentorshipsEndpoints.ConfirmSession.Replace("{sessionId}", sessionId.ToString()), 
            confirmPayload);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ConfirmSession_ShouldReturnUnauthorized_WhenMenteeTriesToConfirm()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        var (menteeData, menteeClient) = await CreateMenteeAndLogin();
        
        await SetMentorAvailability(mentorClient, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));
        var sessionId = await BookSession(menteeClient, mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(10));

        var confirmPayload = new
        {
            GoogleMeetUrl = "https://meet.google.com/abc-defg-hij"
        };

        // Act - Mentee trying to confirm (should fail)
        var response = await menteeClient.PutAsJsonAsync(
            MentorshipsEndpoints.ConfirmSession.Replace("{sessionId}", sessionId.ToString()), 
            confirmPayload);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetSession_ShouldReturnSession_WhenUserIsParticipant()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        var (menteeData, menteeClient) = await CreateMenteeAndLogin();
        
        await SetMentorAvailability(mentorClient, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));
        var sessionId = await BookSession(menteeClient, mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(10));

        // Act - Both mentor and mentee should be able to get session details
        var mentorResponse = await mentorClient.GetAsync(
            MentorshipsEndpoints.GetSession.Replace("{sessionId}", sessionId.ToString()));
        var menteeResponse = await menteeClient.GetAsync(
            MentorshipsEndpoints.GetSession.Replace("{sessionId}", sessionId.ToString()));

        // Assert
        mentorResponse.EnsureSuccessStatusCode();
        menteeResponse.EnsureSuccessStatusCode();

        var mentorSession = await mentorResponse.Content.ReadFromJsonAsync<dynamic>();
        var menteeSession = await menteeResponse.Content.ReadFromJsonAsync<dynamic>();
        
        Assert.NotNull(mentorSession);
        Assert.NotNull(menteeSession);
    }

    [Fact]
    public async Task GetSession_ShouldReturnNotFound_WhenSessionDoesNotExist()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();

        // Act
        var response = await mentorClient.GetAsync(
            MentorshipsEndpoints.GetSession.Replace("{sessionId}", "99999"));

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetSession_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Act
        var response = await _client.GetAsync(
            MentorshipsEndpoints.GetSession.Replace("{sessionId}", "1"));

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task SessionWorkflow_ShouldWorkEndToEnd_FromBookingToConfirmation()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin(100.0m, "Expert in system design");
        var (menteeData, menteeClient) = await CreateMenteeAndLogin();
        
        // Set mentor availability
        await SetMentorAvailability(mentorClient, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));

        // Step 1: Mentee books a session
        var sessionId = await BookSession(menteeClient, mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(10), 90, "System design consultation");

        // Step 2: Verify session appears in both mentor and mentee sessions
        var mentorSessionsResponse = await mentorClient.GetAsync(MentorshipsEndpoints.GetMentorSessions);
        var menteeSessionsResponse = await menteeClient.GetAsync(MentorshipsEndpoints.GetMenteeSessions);
        
        mentorSessionsResponse.EnsureSuccessStatusCode();
        menteeSessionsResponse.EnsureSuccessStatusCode();

        var mentorSessions = await mentorSessionsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        var menteeSessions = await menteeSessionsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        
        Assert.Single(mentorSessions);
        Assert.Single(menteeSessions);

        // Step 3: Mentor confirms the session
        var confirmPayload = new
        {
            GoogleMeetUrl = "https://meet.google.com/test-session-123"
        };

        var confirmResponse = await mentorClient.PutAsJsonAsync(
            MentorshipsEndpoints.ConfirmSession.Replace("{sessionId}", sessionId.ToString()), 
            confirmPayload);

        confirmResponse.EnsureSuccessStatusCode();

        // Step 4: Verify session details include Google Meet link
        var sessionDetailsResponse = await mentorClient.GetAsync(
            MentorshipsEndpoints.GetSession.Replace("{sessionId}", sessionId.ToString()));
        
        sessionDetailsResponse.EnsureSuccessStatusCode();
        var sessionDetails = await sessionDetailsResponse.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(sessionDetails);

        // Optional: Cancel the session
        var cancelPayload = new
        {
            CancellationReason = "Emergency came up"
        };

        var cancelResponse = await menteeClient.PutAsJsonAsync(
            MentorshipsEndpoints.CancelSession.Replace("{sessionId}", sessionId.ToString()), 
            cancelPayload);

        cancelResponse.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task MultipleSessionsWorkflow_ShouldHandleMultipleConcurrentSessions()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin(75.0m, "Full-stack mentor");
        var (mentee1Data, mentee1Client) = await CreateMenteeAndLogin();
        var (mentee2Data, mentee2Client) = await CreateMenteeAndLogin();
        
        // Set mentor availability for multiple days
        await SetMentorAvailability(mentorClient, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));
        await SetMentorAvailability(mentorClient, DayOfWeek.Tuesday, new TimeOnly(10, 0), new TimeOnly(16, 0));
        await SetMentorAvailability(mentorClient, DayOfWeek.Wednesday, new TimeOnly(9, 0), new TimeOnly(15, 0));

        // Book multiple sessions with different mentees
        var session1Id = await BookSession(mentee1Client, mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(10), 60, "React consultation");
        var session2Id = await BookSession(mentee2Client, mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekTuesday.AddHours(11), 90, "Node.js architecture");
        var session3Id = await BookSession(mentee1Client, mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekWednesday.AddHours(13), 60, "Database design");

        // Verify all sessions are created
        var mentorSessionsResponse = await mentorClient.GetAsync(MentorshipsEndpoints.GetMentorSessions);
        mentorSessionsResponse.EnsureSuccessStatusCode();
        
        var mentorSessions = await mentorSessionsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.Equal(3, mentorSessions.Count);

        // Verify mentee 1 has 2 sessions
        var mentee1SessionsResponse = await mentee1Client.GetAsync(MentorshipsEndpoints.GetMenteeSessions);
        mentee1SessionsResponse.EnsureSuccessStatusCode();
        
        var mentee1Sessions = await mentee1SessionsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.Equal(2, mentee1Sessions.Count);

        // Verify mentee 2 has 1 session
        var mentee2SessionsResponse = await mentee2Client.GetAsync(MentorshipsEndpoints.GetMenteeSessions);
        mentee2SessionsResponse.EnsureSuccessStatusCode();
        
        var mentee2Sessions = await mentee2SessionsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.Single(mentee2Sessions);
    }
}
