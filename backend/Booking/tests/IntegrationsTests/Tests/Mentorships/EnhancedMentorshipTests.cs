using System.Net;
using System.Net.Http.Json;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships;

/// <summary>
/// Example tests demonstrating the enhanced multi-client testing approach
/// </summary>
public class EnhancedMentorshipTests : EnhancedMentorshipTestBase
{
    public EnhancedMentorshipTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CompleteWorkflow_ShouldWork_WithMultipleActors()
    {
        // Arrange - Create multiple actors easily
        var scenario = await CreateMentorshipScenario(
            mentorId: "senior_mentor",
            menteeId: "junior_dev", 
            mentorRate: 100.0m,
            mentorBio: "Senior software architect with 15 years experience");

        // Setup mentor availability
        await scenario.SetupMentorAvailability(this);

        // Act - Book a session
        var sessionTime = GetNextWeekday(DayOfWeek.Monday, new TimeOnly(10, 0));
        var sessionId = await scenario.BookSession(this, sessionTime, 60, "Architecture review session");

        // Mentor confirms the session
        await ConfirmSession(scenario.Mentor, sessionId);

        // After session, mentee leaves a review
        var reviewId = await AddReview(scenario.Mentee, sessionId, 5, "Excellent mentoring session!");

        // Assert - Verify the workflow completed successfully
        Assert.True(sessionId > 0);
        Assert.True(reviewId > 0);
    }

    [Fact]
    public async Task MultipleMentors_ShouldAllowMenteeToBookWithDifferentMentors()
    {
        // Arrange - Create multiple mentors and one mentee
        var multiScenario = await CreateMultiMentorScenario(
            menteeId: "student",
            ("backend_expert", 80.0m, "Backend specialist"),
            ("frontend_guru", 75.0m, "Frontend expert"),
            ("devops_master", 90.0m, "DevOps engineer"));

        // Setup availability for all mentors
        var availabilityTasks = multiScenario.Mentors.Select(mentor => 
            SetupMentorAvailability(mentor));
        await Task.WhenAll(availabilityTasks);

        // Act - Mentee books sessions with different mentors
        var sessionTasks = multiScenario.Mentors.Select((mentor, index) =>
        {
            var sessionTime = GetNextWeekday(DayOfWeek.Monday, new TimeOnly(9 + index, 0));
            return BookSession(multiScenario.Mentee, mentor, sessionTime, 60, $"Session with {mentor.UserId}");
        });

        var sessionIds = await Task.WhenAll(sessionTasks);

        // Assert
        Assert.Equal(3, sessionIds.Length);
        Assert.All(sessionIds, id => Assert.True(id > 0));
    }

    [Fact]
    public async Task ConcurrentBooking_ShouldHandleMultipleMenteesBookingSameMentor()
    {
        // Arrange - Create one mentor and multiple mentees
        var mentor = await CreateMentor("popular_mentor", 100.0m, "Very popular mentor");
        var mentees = await CreateMentees("mentee1", "mentee2", "mentee3");

        await SetupMentorAvailability(mentor);

        // Act - Multiple mentees try to book at the same time
        var bookingTasks = mentees.Select((mentee, index) =>
        {
            var sessionTime = GetNextWeekday(DayOfWeek.Monday, new TimeOnly(10 + index, 0));
            return BookSession(mentee, mentor, sessionTime, 60, $"Session by {mentee.UserId}");
        });

        var sessionIds = await Task.WhenAll(bookingTasks);

        // Assert - All bookings should succeed (different time slots)
        Assert.Equal(3, sessionIds.Length);
        Assert.All(sessionIds, id => Assert.True(id > 0));
    }

    [Fact]
    public async Task AuthenticationIsolation_ShouldWorkIndependentlyAcrossUsers()
    {
        // Arrange - Create users with the new system
        var users = CreateUsers("mentor1", "mentee1", "unauthenticated");

        // Only authenticate some users
        await CreateUserAndLogin("mentor1");
        await CreateUserAndLogin("mentee1");
        // unauthenticated user is not logged in

        // Act & Assert - Test different authentication states

        // 1. Authenticated mentor should be able to become mentor
        var becomeMentorResponse = await users["mentor1"].act.PostAsJsonAsync(
            "/mentorships/become-mentor", 
            new { HourlyRate = 50.0m, Bio = "Test mentor" });
        becomeMentorResponse.EnsureSuccessStatusCode();

        // 2. Authenticated mentee should be able to access mentee endpoints  
        var menteeSessionsResponse = await users["mentee1"].act.GetAsync("/mentorships/sessions/mentee");
        menteeSessionsResponse.EnsureSuccessStatusCode();

        // 3. Unauthenticated user should get unauthorized
        var unauthorizedResponse = await users["unauthenticated"].act.PostAsJsonAsync(
            "/mentorships/become-mentor",
            new { HourlyRate = 50.0m, Bio = "Unauthorized attempt" });
        Assert.Equal(HttpStatusCode.Unauthorized, unauthorizedResponse.StatusCode);

        // 4. Verify cookie isolation - each user should have independent authentication state
        Assert.True(IsUserAuthenticated("mentor1"));
        Assert.True(IsUserAuthenticated("mentee1"));
        Assert.False(IsUserAuthenticated("unauthenticated"));
    }

    [Fact]
    public async Task CookieManagement_ShouldAllowLogoutAndRelogin()
    {
        // Arrange
        var mentor = await CreateMentor("test_mentor", 50.0m);

        // Verify mentor is authenticated
        Assert.True(IsUserAuthenticated("test_mentor"));

        // Act - Clear cookies (simulating logout)
        ClearCookies("test_mentor");

        // Assert - Mentor should no longer be authenticated
        Assert.False(IsUserAuthenticated("test_mentor"));

        // Trying to access protected endpoint should fail
        var unauthorizedResponse = await mentor.Act.GetAsync("/mentorships/sessions/mentor");
        Assert.Equal(HttpStatusCode.Unauthorized, unauthorizedResponse.StatusCode);

        // Re-login should work
        var newLoginData = await CreateUserAndLogin("test_mentor", mentor.LoginData.Email);
        Assert.True(IsUserAuthenticated("test_mentor"));

        // Should be able to access protected endpoints again
        var authorizedResponse = await mentor.Act.GetAsync("/mentorships/sessions/mentor");
        authorizedResponse.EnsureSuccessStatusCode();
    }
}