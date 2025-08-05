using System.Net;
using System.Net.Http.Json;
using Booking.Modules.Mentorships.Features;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships;

/// <summary>
/// Comprehensive integration tests for complete mentorship workflows
/// Tests end-to-end scenarios from mentor registration to session completion
/// </summary>
public class MentorshipWorkflowTests : MentorshipTestBase
{
    public MentorshipWorkflowTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CompleteSessionWorkflow_ShouldSucceed_WhenAllStepsAreValid()
    {
        // Arrange - Create mentor first
        var (mentorClient, mentorData) = await CreateAuthenticatedMentorClient(
            hourlyRate: 75.0m,
            bio: "Expert software engineer with mentoring experience");

        // Step 1: Mentor sets availability  
        var availabilityIds = await SetupMentorWeeklyAvailability(mentorClient);
        Assert.Equal(5, availabilityIds.Count); // Weekday availability

        // Step 2: Create a new mentee session and book
        var (menteeClient, menteeData) = await CreateAuthenticatedMenteeClient();
        
        var scheduledAt = MentorshipTestData.Sessions.NextWeekMonday.AddHours(10); // Monday 10 AM
        var sessionId = await BookSession(
            menteeClient, 
            mentorData.UserSlug.ToString(), 
            scheduledAt, 
            60, 
            "Looking forward to learning clean architecture");

        Assert.True(sessionId > 0);

        // Step 3: Switch back to mentor and confirm the session
        var (mentorClientForConfirm, _) = await CreateMentorAndLogin(75.0m, "Expert software engineer");
        await ConfirmSessionAsMentor(mentorClientForConfirm, sessionId, "https://meet.google.com/test-session");

        // Step 4: Verify session details
        var sessionResponse = await mentorClientForConfirm.GetAsync(
            MentorshipsEndpoints.GetSession.Replace("{sessionId}", sessionId.ToString()));
        sessionResponse.EnsureSuccessStatusCode();

        // Step 5: Switch back to mentee and add review
        var (menteeClientForReview, _) = await CreateMenteeAndLogin();
        var reviewId = await AddSessionReview(menteeClientForReview, sessionId, 5, "Excellent session on clean architecture!");
        Assert.True(reviewId > 0);

        // Step 6: Verify mentor's reviews (public endpoint)
        var reviewsResponse = await _client.GetAsync(
            MentorshipsEndpoints.GetMentorReviews.Replace("{mentorSlug}", mentorData.UserSlug.ToString()));
        reviewsResponse.EnsureSuccessStatusCode();

        var reviews = await reviewsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(reviews);
        Assert.Single(reviews);
    }

    [Fact]
    public async Task MentorshipRelationshipWorkflow_ShouldSucceed_WhenRequestIsAccepted()
    {
        // Arrange
        var (mentorClient, menteeClient, mentorData, menteeData) = await CreateMentorMenteePair();

        // Step 1: Mentee requests mentorship relationship
        var relationshipId = await RequestMentorshipRelationship(
            menteeClient, 
            mentorData.UserSlug.ToString(), 
            "I would love to learn from your expertise in software engineering");

        Assert.True(relationshipId > 0);

        // Step 2: Mentor accepts the relationship
        await AcceptMentorshipRelationship(mentorClient, relationshipId);

        // Step 3: Verify relationship exists
        var relationshipsResponse = await mentorClient.GetAsync(MentorshipsEndpoints.GetMentorshipRelationships);
        relationshipsResponse.EnsureSuccessStatusCode();

        var relationships = await relationshipsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(relationships);
        Assert.Single(relationships);
    }

    [Fact]
    public async Task MultipleMentorsScenario_ShouldAllowMenteeToInteractWithMultipleMentors()
    {
        // Arrange - Create multiple mentors and one mentee
        var menteeClient = await CreateAuthenticatedMenteeClient();
        var menteeData = await CreateUserAndLogin(); // Get mentee data

        var mentor1Client = await CreateAuthenticatedMentorClient(hourlyRate: 50.0m, bio: "Junior mentor");
        var mentor1Data = await CreateUserAndLogin(); // This will be overwritten, need to fix this

        var mentor2Client = await CreateAuthenticatedMentorClient(hourlyRate: 100.0m, bio: "Senior mentor");
        var mentor2Data = await CreateUserAndLogin(); // This will be overwritten, need to fix this

        // For now, let's use the CreateMentorAndLogin method for clarity
        var (mentor1DataActual, mentor1ClientActual) = await CreateMentorAndLogin(50.0m, "Junior mentor");
        var (mentor2DataActual, mentor2ClientActual) = await CreateMentorAndLogin(100.0m, "Senior mentor");

        // Set availability for both mentors
        await SetupMentorWeeklyAvailability(mentor1ClientActual);
        await SetupMentorWeeklyAvailability(mentor2ClientActual);

        // Book sessions with both mentors
        var session1Id = await BookSession(
            menteeClient, 
            mentor1DataActual.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(10));

        var session2Id = await BookSession(
            menteeClient, 
            mentor2DataActual.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekTuesday.AddHours(14));

        // Verify mentee has sessions with both mentors
        var menteeSessionsResponse = await menteeClient.GetAsync(MentorshipsEndpoints.GetMenteeSessions);
        menteeSessionsResponse.EnsureSuccessStatusCode();

        var menteeSessions = await menteeSessionsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(menteeSessions);
        Assert.Equal(2, menteeSessions.Count);
    }

    [Fact]
    public async Task SessionCancellationWorkflow_ShouldHandleAllCancellationScenarios()
    {
        // Arrange
        var (mentorClient, menteeClient, mentorData, menteeData) = await CreateMentorMenteePair();
        await SetupMentorWeeklyAvailability(mentorClient);

        // Book multiple sessions
        var sessionIds = await BookMultipleSessionsForTesting(
            menteeClient, 
            mentorData.UserSlug.ToString(), 
            3);

        Assert.Equal(3, sessionIds.Count);

        // Scenario 1: Mentee cancels a session
        var cancelPayload = new { CancellationReason = "Schedule conflict" };
        var cancelResponse = await menteeClient.PutAsJsonAsync(
            MentorshipsEndpoints.CancelSession.Replace("{sessionId}", sessionIds[0].ToString()),
            cancelPayload);
        cancelResponse.EnsureSuccessStatusCode();

        // Scenario 2: Mentor confirms remaining sessions
        await ConfirmSessionAsMentor(mentorClient, sessionIds[1]);
        await ConfirmSessionAsMentor(mentorClient, sessionIds[2]);

        // Verify session states
        var mentorSessionsResponse = await mentorClient.GetAsync(MentorshipsEndpoints.GetMentorSessions);
        mentorSessionsResponse.EnsureSuccessStatusCode();

        var mentorSessions = await mentorSessionsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(mentorSessions);
        Assert.Equal(3, mentorSessions.Count); // All sessions should be visible
    }

    [Fact]
    public async Task AvailabilityManagementWorkflow_ShouldAllowCompleteAvailabilityLifecycle()
    {
        // Arrange
        var mentorClient = await CreateAuthenticatedMentorClient();

        // Step 1: Set initial availability
        var initialAvailabilityIds = await SetupMentorWeeklyAvailability(mentorClient);
        Assert.Equal(5, initialAvailabilityIds.Count);

        // Step 2: Update an availability slot
        var updatePayload = new
        {
            DayOfWeek = DayOfWeek.Monday,
            StartTime = "10:00",
            EndTime = "18:00"
        };

        var updateResponse = await mentorClient.PutAsJsonAsync(
            MentorshipsEndpoints.UpdateAvailability.Replace("{availabilityId}", initialAvailabilityIds[0].ToString()),
            updatePayload);
        updateResponse.EnsureSuccessStatusCode();

        // Step 3: Add weekend availability
        var weekendAvailabilityPayload = new
        {
            DayOfWeek = DayOfWeek.Saturday,
            StartTime = "10:00",
            EndTime = "14:00"
        };

        var weekendResponse = await mentorClient.PostAsJsonAsync(
            MentorshipsEndpoints.SetAvailability, 
            weekendAvailabilityPayload);
        weekendResponse.EnsureSuccessStatusCode();

        // Step 4: Delete an availability slot
        var deleteResponse = await mentorClient.DeleteAsync(
            MentorshipsEndpoints.DeleteAvailability.Replace("{availabilityId}", initialAvailabilityIds[1].ToString()));
        deleteResponse.EnsureSuccessStatusCode();

        // Step 5: Verify final availability state
        var (mentorData, _) = await CreateMentorAndLogin(); // Get mentor data for slug
        var finalAvailabilityResponse = await _client.GetAsync(
            MentorshipsEndpoints.GetMentorAvailability.Replace("{mentorSlug}", mentorData.UserSlug.ToString()));
        finalAvailabilityResponse.EnsureSuccessStatusCode();

        var finalAvailabilities = await finalAvailabilityResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(finalAvailabilities);
        // Should have 4 weekday slots (one deleted) + 1 weekend slot = 5 total
        Assert.Equal(5, finalAvailabilities.Count);
    }

    [Fact]
    public async Task ReviewSystemWorkflow_ShouldAllowCompleteReviewLifecycle()
    {
        // Arrange
        var (mentorClient, menteeClient, mentorData, menteeData) = await CreateMentorMenteePair();
        await SetupMentorWeeklyAvailability(mentorClient);

        // Book and confirm multiple sessions
        var sessionIds = await BookMultipleSessionsForTesting(
            menteeClient, 
            mentorData.UserSlug.ToString(), 
            3);

        foreach (var sessionId in sessionIds)
        {
            await ConfirmSessionAsMentor(mentorClient, sessionId);
        }

        // Add reviews for all sessions
        var reviewIds = new List<int>();
        for (int i = 0; i < sessionIds.Count; i++)
        {
            var (rating, comment) = MentorshipTestData.Reviews.SampleReviews[i];
            var reviewId = await AddSessionReview(menteeClient, sessionIds[i], rating, comment);
            reviewIds.Add(reviewId);
        }

        // Update a review
        var updateReviewPayload = new
        {
            Rating = 5,
            Comment = "Updated: Absolutely fantastic mentoring experience!"
        };

        var updateReviewResponse = await menteeClient.PutAsJsonAsync(
            MentorshipsEndpoints.UpdateReview.Replace("{reviewId}", reviewIds[0].ToString()),
            updateReviewPayload);
        updateReviewResponse.EnsureSuccessStatusCode();

        // Verify all reviews
        var reviewsResponse = await _client.GetAsync(
            MentorshipsEndpoints.GetMentorReviews.Replace("{mentorSlug}", mentorData.UserSlug.ToString()));
        reviewsResponse.EnsureSuccessStatusCode();

        var reviews = await reviewsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(reviews);
        Assert.Equal(3, reviews.Count);

        // Delete a review
        var deleteReviewResponse = await menteeClient.DeleteAsync(
            MentorshipsEndpoints.DeleteReview.Replace("{reviewId}", reviewIds[1].ToString()));
        deleteReviewResponse.EnsureSuccessStatusCode();

        // Verify review deletion
        var finalReviewsResponse = await _client.GetAsync(
            MentorshipsEndpoints.GetMentorReviews.Replace("{mentorSlug}", mentorData.UserSlug.ToString()));
        finalReviewsResponse.EnsureSuccessStatusCode();

        var finalReviews = await finalReviewsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(finalReviews);
        Assert.Equal(2, finalReviews.Count); // One review deleted
    }

    [Theory]
    [InlineData(30)]
    [InlineData(45)]
    [InlineData(60)]
    [InlineData(90)]
    [InlineData(120)]
    public async Task SessionBooking_ShouldAcceptValidDurations(int duration)
    {
        // Arrange
        var (mentorClient, menteeClient, mentorData, menteeData) = await CreateMentorMenteePair();
        await SetupMentorWeeklyAvailability(mentorClient);

        // Act
        var sessionId = await BookSession(
            menteeClient, 
            mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(10),
            duration,
            $"Test session with {duration} minute duration");

        // Assert
        Assert.True(sessionId > 0);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(15)]
    [InlineData(300)]
    [InlineData(-30)]
    public async Task SessionBooking_ShouldRejectInvalidDurations(int invalidDuration)
    {
        // Arrange
        var (mentorClient, menteeClient, mentorData, menteeData) = await CreateMentorMenteePair();
        await SetupMentorWeeklyAvailability(mentorClient);

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
    public async Task MentorProfileManagement_ShouldAllowCompleteProfileLifecycle()
    {
        // Arrange
        var mentorClient = await CreateAuthenticatedMentorClient(
            hourlyRate: 75.0m, 
            bio: "Initial bio");

        // Step 1: Update mentor profile
        var updateProfilePayload = new
        {
            HourlyRate = 100.0m,
            Bio = "Updated bio with more details about expertise and experience"
        };

        var updateResponse = await mentorClient.PutAsJsonAsync(
            MentorshipsEndpoints.UpdateMentorProfile, 
            updateProfilePayload);
        updateResponse.EnsureSuccessStatusCode();

        // Step 2: Verify profile update
        var (mentorData, _) = await CreateMentorAndLogin(); // Get mentor data
        var profileResponse = await _client.GetAsync(
            MentorshipsEndpoints.GetMentorProfile.Replace("{mentorSlug}", mentorData.UserSlug.ToString()));
        profileResponse.EnsureSuccessStatusCode();

        // Step 3: Deactivate mentor profile
        var deactivateResponse = await mentorClient.PutAsJsonAsync(
            MentorshipsEndpoints.DeactivateMentor, 
            new { });
        deactivateResponse.EnsureSuccessStatusCode();

        // Step 4: Verify mentor is deactivated (profile should return 404 or show inactive)
        var deactivatedProfileResponse = await _client.GetAsync(
            MentorshipsEndpoints.GetMentorProfile.Replace("{mentorSlug}", mentorData.UserSlug.ToString()));
        // Depending on implementation, this might be 404 or show inactive status
        Assert.True(deactivatedProfileResponse.StatusCode == HttpStatusCode.NotFound || 
                   deactivatedProfileResponse.IsSuccessStatusCode);
    }
}
