using System.Net;
using System.Net.Http.Json;
using Booking.Modules.Mentorships.Features;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships;

/// <summary>
/// Basic mentorship functionality tests using cookie-based authentication
/// These tests verify the core functionality works with the current authentication system
/// </summary>
public class MentorshipBasicTests : MentorshipTestBase
{
    public MentorshipBasicTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task MentorRegistration_ShouldWork_WithCookieAuthentication()
    {
        // Arrange - Create and login a user
        var userData = await CreateUserAndLogin();

        var becomeMentorPayload = new
        {
            HourlyRate = 75.0m,
            Bio = "Expert software engineer with 10+ years of experience"
        };

        // Act - Become a mentor (using cookie authentication)
        var response = await _client.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, becomeMentorPayload);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task AvailabilityManagement_ShouldWork_WithCookieAuthentication()
    {
        // Arrange - Create mentor
        var userData = await CreateUserAndLogin();
        await _client.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, new
        {
            HourlyRate = 50.0m,
            Bio = "Test mentor"
        });

        // Act - Set availability
        var availabilityPayload = new
        {
            DayOfWeek = DayOfWeek.Monday,
            StartTime = "09:00",
            EndTime = "17:00"
        };

        var response = await _client.PostAsJsonAsync(MentorshipsEndpoints.SetAvailability, availabilityPayload);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task SessionBooking_ShouldWork_WithSequentialAuthentication()
    {
        // Arrange - Create mentor and set availability
        var mentorData = await CreateUserAndLogin();
        await _client.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, new
        {
            HourlyRate = 75.0m,
            Bio = "Expert mentor"
        });

        // Set availability
        await SetupMentorWeeklyAvailability();

        // Create mentee in a new session
        var menteeData = await CreateUserAndLogin();

        // Act - Book a session
        var bookingPayload = new
        {
            MentorSlug = mentorData.UserSlug.ToString(),
            ScheduledAt = MentorshipTestData.Sessions.NextWeekMonday.AddHours(10),
            DurationMinutes = 60,
            Note = "Looking forward to the session!"
        };

        var response = await _client.PostAsJsonAsync(MentorshipsEndpoints.BookSession, bookingPayload);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task PublicEndpoints_ShouldWork_WithoutAuthentication()
    {
        // Arrange - Create mentor with availability
        var mentorData = await CreateUserAndLogin();
        await _client.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, new
        {
            HourlyRate = 100.0m,
            Bio = "Public test mentor"
        });

        await SetupMentorWeeklyAvailability();

        // Act & Assert - Test public endpoints without authentication
        var unauthenticatedClient = Factory.CreateClient();

        // Get mentor profile (public)
        var profileResponse = await unauthenticatedClient.GetAsync(
            MentorshipsEndpoints.GetMentorProfile.Replace("{mentorSlug}", mentorData.UserSlug.ToString()));
        profileResponse.EnsureSuccessStatusCode();

        // Get mentor availability (public)
        var availabilityResponse = await unauthenticatedClient.GetAsync(
            MentorshipsEndpoints.GetMentorAvailability.Replace("{mentorSlug}", mentorData.UserSlug.ToString()));
        availabilityResponse.EnsureSuccessStatusCode();

        // Get mentor reviews (public)
        var reviewsResponse = await unauthenticatedClient.GetAsync(
            MentorshipsEndpoints.GetMentorReviews.Replace("{mentorSlug}", mentorData.UserSlug.ToString()));
        reviewsResponse.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task AuthenticationRequired_ShouldReturnUnauthorized_ForProtectedEndpoints()
    {
        // Arrange - Use unauthenticated client
        var unauthenticatedClient = Factory.CreateClient();

        // Act & Assert - Protected endpoints should return unauthorized

        // Try to become mentor
        var becomeMentorPayload = new
        {
            HourlyRate = 50.0m,
            Bio = "Unauthorized attempt"
        };
        var becomeMentorResponse = await unauthenticatedClient.PostAsJsonAsync(
            MentorshipsEndpoints.BecomeMentor, becomeMentorPayload);
        Assert.Equal(HttpStatusCode.Unauthorized, becomeMentorResponse.StatusCode);

        // Try to set availability
        var availabilityPayload = new
        {
            DayOfWeek = DayOfWeek.Monday,
            StartTime = "09:00",
            EndTime = "17:00"
        };
        var availabilityResponse = await unauthenticatedClient.PostAsJsonAsync(
            MentorshipsEndpoints.SetAvailability, availabilityPayload);
        Assert.Equal(HttpStatusCode.Unauthorized, availabilityResponse.StatusCode);

        // Try to book session
        var bookingPayload = new
        {
            MentorSlug = "test-mentor",
            ScheduledAt = DateTime.UtcNow.AddDays(7),
            DurationMinutes = 60,
            Note = "Unauthorized booking"
        };
        var bookingResponse = await unauthenticatedClient.PostAsJsonAsync(
            MentorshipsEndpoints.BookSession, bookingPayload);
        Assert.Equal(HttpStatusCode.Unauthorized, bookingResponse.StatusCode);
    }

    [Fact]
    public async Task SimpleWorkflow_ShouldDemonstrateBasicFunctionality()
    {
        // Step 1: User becomes mentor
        var mentorData = await CreateUserAndLogin();
        var becomeMentorResponse = await _client.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, new
        {
            HourlyRate = 80.0m,
            Bio = "Full-stack developer with 8 years experience"
        });
        becomeMentorResponse.EnsureSuccessStatusCode();

        // Step 2: Mentor sets availability
        var availabilityId = await SetMentorAvailability(dayOfWeek: DayOfWeek.Wednesday, 
            startTime: new TimeOnly(14, 0), endTime: new TimeOnly(18, 0));
        Assert.True(availabilityId > 0);

        // Step 3: Different user becomes mentee and books session
        var menteeData = await CreateUserAndLogin();
        var sessionId = await BookSession(
            mentorSlug: mentorData.UserSlug.ToString(),
            scheduledAt: MentorshipTestData.Sessions.NextWeekWednesday.AddHours(15), // Wednesday 3 PM
            durationMinutes: 90,
            note: "Need help with React patterns");
        Assert.True(sessionId > 0);

        // Step 4: Switch back to mentor and confirm session
        var mentorLoginAgain = await LoginUser(mentorData.Email, "TestPassword123!");
        await ConfirmSessionAsMentor(sessionId: sessionId, googleMeetUrl: "https://meet.google.com/simple-workflow");

        // Step 5: Switch back to mentee and add review
        await LoginUser(menteeData.Email, "TestPassword123!");
        var reviewId = await AddSessionReview(sessionId: sessionId, rating: 5, 
            comment: "Great session! Learned a lot about React patterns.");
        Assert.True(reviewId > 0);

        // Step 6: Verify everything worked using public endpoints
        var publicClient = Factory.CreateClient();
        
        var finalReviewsResponse = await publicClient.GetAsync(
            MentorshipsEndpoints.GetMentorReviews.Replace("{mentorSlug}", mentorData.UserSlug.ToString()));
        finalReviewsResponse.EnsureSuccessStatusCode();
        
        var reviews = await finalReviewsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(reviews);
        Assert.Single(reviews);
    }
}
