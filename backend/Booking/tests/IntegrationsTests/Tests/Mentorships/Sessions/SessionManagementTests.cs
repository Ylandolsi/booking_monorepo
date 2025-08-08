/*using System.Net;
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

    [Fact]
    public async Task BookSession_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
    {
        // Arrange
        var client = Factory.CreateClient();
        
        var sessionData = new
        {
            MentorSlug = "test-mentor",
            ScheduledAt = DateTime.Now.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss"),
            DurationMinutes = 60,
            Notes = "Looking forward to learning about Clean Architecture"
        };

        // Act
        var response = await client.PostAsJsonAsync(MentorshipsEndpoints.BookSession, sessionData);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldReturnBadRequest_WhenMentorSlugIsEmpty()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var sessionData = new
        {
            MentorSlug = "",
            ScheduledAt = DateTime.Now.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss"),
            DurationMinutes = 60,
            Notes = "Looking forward to learning about Clean Architecture"
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.BookSession, sessionData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldReturnBadRequest_WhenScheduledTimeIsInPast()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var sessionData = new
        {
            MentorSlug = "test-mentor",
            ScheduledAt = DateTime.Now.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss"),
            DurationMinutes = 60,
            Notes = "Looking forward to learning about Clean Architecture"
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.BookSession, sessionData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldReturnBadRequest_WhenDurationIsInvalid()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var sessionData = new
        {
            MentorSlug = "test-mentor",
            ScheduledAt = DateTime.Now.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss"),
            DurationMinutes = 0, // Invalid duration
            Notes = "Looking forward to learning about Clean Architecture"
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.BookSession, sessionData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldReturnBadRequest_WhenDurationIsNegative()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var sessionData = new
        {
            MentorSlug = "test-mentor",
            ScheduledAt = DateTime.Now.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss"),
            DurationMinutes = -30,
            Notes = "Looking forward to learning about Clean Architecture"
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.BookSession, sessionData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldReturnNotFound_WhenMentorDoesNotExist()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var sessionData = new
        {
            MentorSlug = "non-existent-mentor-999",
            ScheduledAt = DateTime.Now.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss"),
            DurationMinutes = 60,
            Notes = "Looking forward to learning about Clean Architecture"
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.BookSession, sessionData);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldReturnConflict_WhenTimeSlotIsUnavailable()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var scheduledTime = DateTime.Now.AddDays(1);
        var sessionData = new
        {
            MentorSlug = "test-mentor",
            ScheduledAt = scheduledTime.ToString("yyyy-MM-ddTHH:mm:ss"),
            DurationMinutes = 60,
            Notes = "First session"
        };

        // Book first session
        await userAct.PostAsJsonAsync(MentorshipsEndpoints.BookSession, sessionData);

        // Try to book overlapping session
        var overlappingSessionData = new
        {
            MentorSlug = "test-mentor",
            ScheduledAt = scheduledTime.AddMinutes(30).ToString("yyyy-MM-ddTHH:mm:ss"),
            DurationMinutes = 60,
            Notes = "Overlapping session"
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.BookSession, overlappingSessionData);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task CancelSession_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
    {
        // Arrange
        var client = Factory.CreateClient();
        var sessionId = 1;

        // Act
        var response = await client.PostAsJsonAsync($"/mentorships/sessions/{sessionId}/cancel", new { });

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CancelSession_ShouldReturnNotFound_WhenSessionDoesNotExist()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var nonExistentSessionId = 999999;

        // Act
        var response = await userAct.PostAsJsonAsync($"/mentorships/sessions/{nonExistentSessionId}/cancel", new { });

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CancelSession_ShouldReturnBadRequest_WhenSessionAlreadyCancelled()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var sessionId = 1;

        // Cancel session first time
        await userAct.PostAsJsonAsync($"/mentorships/sessions/{sessionId}/cancel", new { });

        // Act - Try to cancel again
        var response = await userAct.PostAsJsonAsync($"/mentorships/sessions/{sessionId}/cancel", new { });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CancelSession_ShouldReturnBadRequest_WhenSessionHasAlreadyOccurred()
    {
        // Arrange - This would need a session that's in the past
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var pastSessionId = 1; // Assuming this session is in the past

        // Act
        var response = await userAct.PostAsJsonAsync($"/mentorships/sessions/{pastSessionId}/cancel", new { });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetSession_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
    {
        // Arrange
        var client = Factory.CreateClient();
        var sessionId = 1;

        // Act
        var response = await client.GetAsync($"/mentorships/sessions/{sessionId}");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetSession_ShouldReturnNotFound_WhenSessionDoesNotExist()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var nonExistentSessionId = 999999;

        // Act
        var response = await userAct.GetAsync($"/mentorships/sessions/{nonExistentSessionId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetSession_ShouldReturnForbidden_WhenUserNotAuthorizedToViewSession()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var unauthorizedSessionId = 1; // Session that belongs to another user

        // Act
        var response = await userAct.GetAsync($"/mentorships/sessions/{unauthorizedSessionId}");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetMentorSessions_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
    {
        // Arrange
        var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync(MentorshipsEndpoints.GetMentorSessions);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetMentorSessions_ShouldReturnForbidden_WhenUserIsNotMentor()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);

        // Act - User is not a mentor, should be forbidden
        var response = await userAct.GetAsync(MentorshipsEndpoints.GetMentorSessions);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetMenteeSessions_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
    {
        // Arrange
        var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync(MentorshipsEndpoints.GetMenteeSessions);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Theory]
    [InlineData(30)]
    [InlineData(60)]
    [InlineData(90)]
    [InlineData(120)]
    public async Task BookSession_ShouldAcceptValidDurations_WhenMultiplesOfThirty(int duration)
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser($"test-duration-{duration}");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var sessionData = new
        {
            MentorSlug = "test-mentor",
            ScheduledAt = DateTime.Now.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss"),
            DurationMinutes = duration,
            Notes = $"Session for {duration} minutes"
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.BookSession, sessionData);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldCreateSessionWithoutNotes_WhenNotesNotProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var sessionData = new
        {
            MentorSlug = "test-mentor",
            ScheduledAt = DateTime.Now.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss"),
            DurationMinutes = 60
            // Notes not provided
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.BookSession, sessionData);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldRespectMentorBufferTime_WhenSchedulingConsecutiveSessions()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var firstSessionTime = DateTime.Now.AddDays(1);
        var firstSessionData = new
        {
            MentorSlug = "test-mentor",
            ScheduledAt = firstSessionTime.ToString("yyyy-MM-ddTHH:mm:ss"),
            DurationMinutes = 60,
            Notes = "First session"
        };

        // Book first session
        await userAct.PostAsJsonAsync(MentorshipsEndpoints.BookSession, firstSessionData);

        // Try to book session immediately after (within buffer time)
        var secondSessionData = new
        {
            MentorSlug = "test-mentor",
            ScheduledAt = firstSessionTime.AddMinutes(60).ToString("yyyy-MM-ddTHH:mm:ss"), // No buffer time
            DurationMinutes = 60,
            Notes = "Second session - should fail"
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.BookSession, secondSessionData);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
} */