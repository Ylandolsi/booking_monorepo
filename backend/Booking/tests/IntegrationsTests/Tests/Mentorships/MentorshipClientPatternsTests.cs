using System.Net;
using System.Net.Http.Json;
using Booking.Modules.Mentorships.Features;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships;

/// <summary>
/// Tests demonstrating different HTTP client patterns for mentor and mentee roles
/// Shows how to use specialized authenticated clients for different user types
/// </summary>
public class MentorshipClientPatternsTests : MentorshipTestBase
{
    public MentorshipClientPatternsTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task AuthenticatedMentorClient_ShouldHaveMentorPermissions()
    {
        // Arrange - Create authenticated mentor client
        var mentorClient = await CreateAuthenticatedMentorClient(
            hourlyRate: 80.0m,
            bio: "Senior software engineer with 8+ years of experience");

        // Act & Assert - Mentor should be able to perform mentor-specific actions

        // 1. Set availability (mentor-only action)
        var availabilityPayload = new
        {
            DayOfWeek = DayOfWeek.Monday,
            StartTime = "09:00",
            EndTime = "17:00"
        };

        var availabilityResponse = await mentorClient.PostAsJsonAsync(
            MentorshipsEndpoints.SetAvailability, 
            availabilityPayload);
        availabilityResponse.EnsureSuccessStatusCode();

        // 2. Update mentor profile (mentor-only action)
        var updateProfilePayload = new
        {
            HourlyRate = 90.0m,
            Bio = "Updated profile with enhanced experience details"
        };

        var updateResponse = await mentorClient.PutAsJsonAsync(
            MentorshipsEndpoints.UpdateMentorProfile, 
            updateProfilePayload);
        updateResponse.EnsureSuccessStatusCode();

        // 3. Get mentor sessions (mentor-only action)
        var sessionsResponse = await mentorClient.GetAsync(MentorshipsEndpoints.GetMentorSessions);
        sessionsResponse.EnsureSuccessStatusCode();

        var sessions = await sessionsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(sessions);
        Assert.Empty(sessions); // No sessions yet
    }

    [Fact]
    public async Task AuthenticatedMenteeClient_ShouldHaveMenteePermissions()
    {
        // Arrange - Create authenticated mentee client and a mentor for booking
        var menteeClient = await CreateAuthenticatedMenteeClient();
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        
        // Set up mentor availability
        await SetupMentorWeeklyAvailability(mentorClient);

        // Act & Assert - Mentee should be able to perform mentee-specific actions

        // 1. Book a session (mentee action)
        var bookSessionPayload = new
        {
            MentorSlug = mentorData.UserSlug.ToString(),
            ScheduledAt = MentorshipTestData.Sessions.NextWeekMonday.AddHours(10),
            DurationMinutes = 60,
            Note = "Excited to learn about software architecture!"
        };

        var bookResponse = await menteeClient.PostAsJsonAsync(
            MentorshipsEndpoints.BookSession, 
            bookSessionPayload);
        bookResponse.EnsureSuccessStatusCode();

        var bookResult = await bookResponse.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(bookResult);

        // 2. Get mentee sessions (mentee-only action)
        var sessionsResponse = await menteeClient.GetAsync(MentorshipsEndpoints.GetMenteeSessions);
        sessionsResponse.EnsureSuccessStatusCode();

        var sessions = await sessionsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(sessions);
        Assert.Single(sessions); // One session booked

        // 3. Request mentorship relationship (mentee action)
        var relationshipPayload = new
        {
            MentorSlug = mentorData.UserSlug.ToString(),
            Message = "I would love to become your long-term mentee"
        };

        var relationshipResponse = await menteeClient.PostAsJsonAsync(
            MentorshipsEndpoints.RequestMentorship, 
            relationshipPayload);
        relationshipResponse.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task MentorMenteePairClients_ShouldEnableCompleteWorkflow()
    {
        // Arrange - Create both types of authenticated clients
        var (mentorClient, menteeClient, mentorData, menteeData) = await CreateMentorMenteePair(
            mentorHourlyRate: 100.0m,
            mentorBio: "Expert mentor ready to guide junior developers");

        // Act & Assert - Demonstrate complete workflow using both clients

        // 1. Mentor sets availability
        var availabilityIds = await SetupMentorWeeklyAvailability(mentorClient);
        Assert.Equal(5, availabilityIds.Count);

        // 2. Mentee books sessions
        var sessionIds = await BookMultipleSessionsForTesting(
            menteeClient, 
            mentorData.UserSlug.ToString(), 
            2);
        Assert.Equal(2, sessionIds.Count);

        // 3. Mentor confirms sessions
        foreach (var sessionId in sessionIds)
        {
            await ConfirmSessionAsMentor(mentorClient, sessionId);
        }

        // 4. Mentee adds reviews
        for (int i = 0; i < sessionIds.Count; i++)
        {
            var (rating, comment) = MentorshipTestData.Reviews.SampleReviews[i];
            await AddSessionReview(menteeClient, sessionIds[i], rating, comment);
        }

        // 5. Verify final state from both perspectives
        
        // Mentor perspective
        var mentorSessionsResponse = await mentorClient.GetAsync(MentorshipsEndpoints.GetMentorSessions);
        mentorSessionsResponse.EnsureSuccessStatusCode();
        var mentorSessions = await mentorSessionsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(mentorSessions);
        Assert.Equal(2, mentorSessions.Count);

        // Mentee perspective
        var menteeSessionsResponse = await menteeClient.GetAsync(MentorshipsEndpoints.GetMenteeSessions);
        menteeSessionsResponse.EnsureSuccessStatusCode();
        var menteeSessions = await menteeSessionsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(menteeSessions);
        Assert.Equal(2, menteeSessions.Count);

        // Public perspective (reviews)
        var reviewsResponse = await _client.GetAsync(
            MentorshipsEndpoints.GetMentorReviews.Replace("{mentorSlug}", mentorData.UserSlug.ToString()));
        reviewsResponse.EnsureSuccessStatusCode();
        var reviews = await reviewsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(reviews);
        Assert.Equal(2, reviews.Count);
    }

    [Fact]
    public async Task UnauthenticatedClient_ShouldOnlyAccessPublicEndpoints()
    {
        // Arrange - Use the base _client (unauthenticated)
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        await SetupMentorWeeklyAvailability(mentorClient);

        // Act & Assert - Public endpoints should work

        // 1. Get mentor profile (public)
        var profileResponse = await _client.GetAsync(
            MentorshipsEndpoints.GetMentorProfile.Replace("{mentorSlug}", mentorData.UserSlug.ToString()));
        profileResponse.EnsureSuccessStatusCode();

        // 2. Get mentor availability (public)
        var availabilityResponse = await _client.GetAsync(
            MentorshipsEndpoints.GetMentorAvailability.Replace("{mentorSlug}", mentorData.UserSlug.ToString()));
        availabilityResponse.EnsureSuccessStatusCode();

        // 3. Get mentor reviews (public)
        var reviewsResponse = await _client.GetAsync(
            MentorshipsEndpoints.GetMentorReviews.Replace("{mentorSlug}", mentorData.UserSlug.ToString()));
        reviewsResponse.EnsureSuccessStatusCode();

        // Act & Assert - Private endpoints should return Unauthorized

        // 1. Try to book session (should fail)
        var bookSessionPayload = new
        {
            MentorSlug = mentorData.UserSlug.ToString(),
            ScheduledAt = MentorshipTestData.Sessions.NextWeekMonday.AddHours(10),
            DurationMinutes = 60,
            Note = "Unauthorized booking attempt"
        };

        var bookResponse = await _client.PostAsJsonAsync(MentorshipsEndpoints.BookSession, bookSessionPayload);
        Assert.Equal(HttpStatusCode.Unauthorized, bookResponse.StatusCode);

        // 2. Try to set availability (should fail)
        var availabilityPayload = new
        {
            DayOfWeek = DayOfWeek.Monday,
            StartTime = "09:00",
            EndTime = "17:00"
        };

        var setAvailabilityResponse = await _client.PostAsJsonAsync(
            MentorshipsEndpoints.SetAvailability, 
            availabilityPayload);
        Assert.Equal(HttpStatusCode.Unauthorized, setAvailabilityResponse.StatusCode);

        // 3. Try to get mentor sessions (should fail)
        var mentorSessionsResponse = await _client.GetAsync(MentorshipsEndpoints.GetMentorSessions);
        Assert.Equal(HttpStatusCode.Unauthorized, mentorSessionsResponse.StatusCode);

        // 4. Try to get mentee sessions (should fail)
        var menteeSessionsResponse = await _client.GetAsync(MentorshipsEndpoints.GetMenteeSessions);
        Assert.Equal(HttpStatusCode.Unauthorized, menteeSessionsResponse.StatusCode);
    }

    [Fact]
    public async Task CrossRoleActions_ShouldReturnForbiddenOrBadRequest()
    {
        // Arrange
        var menteeClient = await CreateAuthenticatedMenteeClient();
        var (mentorData, mentorClient) = await CreateMentorAndLogin();

        // Act & Assert - Mentee trying mentor actions should fail

        // 1. Mentee trying to set availability (mentor-only action)
        var availabilityPayload = new
        {
            DayOfWeek = DayOfWeek.Monday,
            StartTime = "09:00",
            EndTime = "17:00"
        };

        var availabilityResponse = await menteeClient.PostAsJsonAsync(
            MentorshipsEndpoints.SetAvailability, 
            availabilityPayload);
        Assert.True(availabilityResponse.StatusCode == HttpStatusCode.Forbidden || 
                   availabilityResponse.StatusCode == HttpStatusCode.BadRequest);

        // 2. Mentee trying to update mentor profile (mentor-only action)
        var updateProfilePayload = new
        {
            HourlyRate = 50.0m,
            Bio = "Trying to update as mentee"
        };

        var updateResponse = await menteeClient.PutAsJsonAsync(
            MentorshipsEndpoints.UpdateMentorProfile, 
            updateProfilePayload);
        Assert.True(updateResponse.StatusCode == HttpStatusCode.Forbidden || 
                   updateResponse.StatusCode == HttpStatusCode.BadRequest);

        // 3. Mentee trying to get mentor sessions (mentor-only action)
        var mentorSessionsResponse = await menteeClient.GetAsync(MentorshipsEndpoints.GetMentorSessions);
        Assert.True(mentorSessionsResponse.StatusCode == HttpStatusCode.Forbidden || 
                   mentorSessionsResponse.StatusCode == HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task MultipleClientsScenario_ShouldSupportConcurrentOperations()
    {
        // Arrange - Create multiple mentors and mentees
        var mentor1Client = await CreateAuthenticatedMentorClient(
            hourlyRate: 50.0m, 
            bio: "Junior mentor");
        var mentor2Client = await CreateAuthenticatedMentorClient(
            hourlyRate: 100.0m, 
            bio: "Senior mentor");
        
        var mentee1Client = await CreateAuthenticatedMenteeClient();
        var mentee2Client = await CreateAuthenticatedMenteeClient();

        // Get mentor data for booking sessions
        var (mentor1Data, _) = await CreateMentorAndLogin(50.0m, "Junior mentor");
        var (mentor2Data, _) = await CreateMentorAndLogin(100.0m, "Senior mentor");

        // Act - Concurrent operations

        // Both mentors set availability
        var mentor1AvailabilityTask = SetupMentorWeeklyAvailability(mentor1Client);
        var mentor2AvailabilityTask = SetupMentorWeeklyAvailability(mentor2Client);

        await Task.WhenAll(mentor1AvailabilityTask, mentor2AvailabilityTask);

        // Both mentees book sessions with different mentors
        var mentee1SessionTask = BookSession(
            mentee1Client, 
            mentor1Data.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(10));

        var mentee2SessionTask = BookSession(
            mentee2Client, 
            mentor2Data.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekTuesday.AddHours(14));

        var sessionIds = await Task.WhenAll(mentee1SessionTask, mentee2SessionTask);

        // Assert - All operations should succeed
        Assert.All(sessionIds, sessionId => Assert.True(sessionId > 0));

        // Verify each mentor has their respective sessions
        var mentor1SessionsResponse = await mentor1Client.GetAsync(MentorshipsEndpoints.GetMentorSessions);
        mentor1SessionsResponse.EnsureSuccessStatusCode();
        var mentor1Sessions = await mentor1SessionsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(mentor1Sessions);
        Assert.Single(mentor1Sessions);

        var mentor2SessionsResponse = await mentor2Client.GetAsync(MentorshipsEndpoints.GetMentorSessions);
        mentor2SessionsResponse.EnsureSuccessStatusCode();
        var mentor2Sessions = await mentor2SessionsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(mentor2Sessions);
        Assert.Single(mentor2Sessions);
    }

    [Fact]
    public async Task ClientAuthentication_ShouldMaintainAuthenticationAcrossRequests()
    {
        // Arrange
        var mentorClient = await CreateAuthenticatedMentorClient();

        // Act & Assert - Multiple requests with same client should maintain authentication

        // Request 1: Set availability
        var availability1 = new
        {
            DayOfWeek = DayOfWeek.Monday,
            StartTime = "09:00",
            EndTime = "12:00"
        };
        var response1 = await mentorClient.PostAsJsonAsync(MentorshipsEndpoints.SetAvailability, availability1);
        response1.EnsureSuccessStatusCode();

        // Request 2: Set another availability
        var availability2 = new
        {
            DayOfWeek = DayOfWeek.Tuesday,
            StartTime = "14:00",
            EndTime = "17:00"
        };
        var response2 = await mentorClient.PostAsJsonAsync(MentorshipsEndpoints.SetAvailability, availability2);
        response2.EnsureSuccessStatusCode();

        // Request 3: Update profile
        var updateProfile = new
        {
            HourlyRate = 85.0m,
            Bio = "Updated bio to reflect current expertise"
        };
        var response3 = await mentorClient.PutAsJsonAsync(MentorshipsEndpoints.UpdateMentorProfile, updateProfile);
        response3.EnsureSuccessStatusCode();

        // Request 4: Get sessions
        var response4 = await mentorClient.GetAsync(MentorshipsEndpoints.GetMentorSessions);
        response4.EnsureSuccessStatusCode();

        // All requests should succeed with the same authenticated client
        Assert.True(response1.IsSuccessStatusCode);
        Assert.True(response2.IsSuccessStatusCode);
        Assert.True(response3.IsSuccessStatusCode);
        Assert.True(response4.IsSuccessStatusCode);
    }
}
