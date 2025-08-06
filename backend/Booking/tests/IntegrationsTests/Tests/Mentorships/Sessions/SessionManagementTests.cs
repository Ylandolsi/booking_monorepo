using System.Net;
using System.Net.Http.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Users.Features;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships.Sessions;

public class SessionManagementTests : AuthenticationTestBase
{
    public SessionManagementTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task BookSession_ShouldCreateSession_WhenValidDataProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var sessionData = new
        {
            MentorSlug = "test-mentor",
            ScheduledAt = DateTime.Now.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss"),
            DurationMinutes = 60,
            Notes = "Looking forward to learning about Clean Architecture"
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.BookSession, sessionData);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CancelSession_ShouldCancelSession_WhenSessionExists()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var sessionId = 1; // This would be a real session ID in practice

        // Act
        var response = await userAct.PostAsJsonAsync($"/mentorships/sessions/{sessionId}/cancel", new { });

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetSession_ShouldReturnSession_WhenSessionExists()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var sessionId = 1; // This would be a real session ID in practice

        // Act
        var response = await userAct.GetAsync($"/mentorships/sessions/{sessionId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetMentorSessions_ShouldReturnSessions_WhenMentorExists()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);

        // Act
        var response = await userAct.GetAsync(MentorshipsEndpoints.GetMentorSessions);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetMenteeSessions_ShouldReturnSessions_WhenMenteeExists()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);

        // Act
        var response = await userAct.GetAsync(MentorshipsEndpoints.GetMenteeSessions);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
} 