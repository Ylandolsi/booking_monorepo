using System.Net;
using System.Net.Http.Json;
using Booking.Modules.Mentorships.Features;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships;

public class ReviewTests : MentorshipTestBase
{
    public ReviewTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task AddReview_ShouldCreateReview_WhenMenteeReviewsCompletedSession()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        var (menteeData, menteeClient) = await CreateMenteeAndLogin();
        
        await SetMentorAvailability(mentorClient, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));
        var sessionId = await BookSession(menteeClient, mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(10));

        // Simulate session completion (this would normally happen after the session time)
        // For testing purposes, we'll assume the session is completed

        var reviewPayload = new
        {
            SessionId = sessionId,
            Rating = 5,
            Comment = "Excellent mentor! Very knowledgeable and helpful. Learned a lot about clean architecture principles."
        };

        // Act
        var response = await menteeClient.PostAsJsonAsync(MentorshipsEndpoints.AddReview, reviewPayload);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(-1)]
    public async Task AddReview_ShouldReturnBadRequest_WhenRatingIsInvalid(int invalidRating)
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        var (menteeData, menteeClient) = await CreateMenteeAndLogin();
        
        await SetMentorAvailability(mentorClient, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));
        var sessionId = await BookSession(menteeClient, mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(10));

        var reviewPayload = new
        {
            SessionId = sessionId,
            Rating = invalidRating,
            Comment = "Test review"
        };

        // Act
        var response = await menteeClient.PostAsJsonAsync(MentorshipsEndpoints.AddReview, reviewPayload);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddReview_ShouldReturnBadRequest_WhenSessionNotCompleted()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        var (menteeData, menteeClient) = await CreateMenteeAndLogin();
        
        await SetMentorAvailability(mentorClient, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));
        var sessionId = await BookSession(menteeClient, mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(10));

        // Session is just booked, not completed
        var reviewPayload = new
        {
            SessionId = sessionId,
            Rating = 5,
            Comment = "Early review"
        };

        // Act
        var response = await menteeClient.PostAsJsonAsync(MentorshipsEndpoints.AddReview, reviewPayload);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddReview_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var reviewPayload = new
        {
            SessionId = 1,
            Rating = 5,
            Comment = "Test review"
        };

        // Act
        var response = await _client.PostAsJsonAsync(MentorshipsEndpoints.AddReview, reviewPayload);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AddReview_ShouldReturnBadRequest_WhenCommentIsTooLong()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        var (menteeData, menteeClient) = await CreateMenteeAndLogin();
        
        await SetMentorAvailability(mentorClient, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));
        var sessionId = await BookSession(menteeClient, mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(10));

        var reviewPayload = new
        {
            SessionId = sessionId,
            Rating = 5,
            Comment = new string('a', 1001) // Assuming max length is 1000 characters
        };

        // Act
        var response = await menteeClient.PostAsJsonAsync(MentorshipsEndpoints.AddReview, reviewPayload);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateReview_ShouldUpdateExistingReview_WhenMenteeOwnsReview()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        var (menteeData, menteeClient) = await CreateMenteeAndLogin();
        
        await SetMentorAvailability(mentorClient, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));
        var sessionId = await BookSession(menteeClient, mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(10));

        // First, add a review
        var addReviewPayload = new
        {
            SessionId = sessionId,
            Rating = 4,
            Comment = "Good session"
        };

        var addResponse = await menteeClient.PostAsJsonAsync(MentorshipsEndpoints.AddReview, addReviewPayload);
        addResponse.EnsureSuccessStatusCode();
        
        var reviewResult = await addResponse.Content.ReadFromJsonAsync<dynamic>();
        var reviewId = reviewResult?.GetProperty("ReviewId").GetInt32();

        // Update the review
        var updateReviewPayload = new
        {
            Rating = 5,
            Comment = "Actually, it was an excellent session! Updated my review after reflecting more."
        };

        // Act
        var response = await menteeClient.PutAsJsonAsync(
            MentorshipsEndpoints.UpdateReview.Replace("{reviewId}", reviewId.ToString()), 
            updateReviewPayload);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteReview_ShouldDeleteReview_WhenMenteeOwnsReview()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        var (menteeData, menteeClient) = await CreateMenteeAndLogin();
        
        await SetMentorAvailability(mentorClient, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));
        var sessionId = await BookSession(menteeClient, mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(10));

        // First, add a review
        var addReviewPayload = new
        {
            SessionId = sessionId,
            Rating = 3,
            Comment = "Session was okay"
        };

        var addResponse = await menteeClient.PostAsJsonAsync(MentorshipsEndpoints.AddReview, addReviewPayload);
        addResponse.EnsureSuccessStatusCode();
        
        var reviewResult = await addResponse.Content.ReadFromJsonAsync<dynamic>();
        var reviewId = reviewResult?.GetProperty("ReviewId").GetInt32();

        // Act
        var response = await menteeClient.DeleteAsync(
            MentorshipsEndpoints.DeleteReview.Replace("{reviewId}", reviewId.ToString()));

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task GetMentorReviews_ShouldReturnReviews_WhenMentorHasReviews()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        var (mentee1Data, mentee1Client) = await CreateMenteeAndLogin();
        var (mentee2Data, mentee2Client) = await CreateMenteeAndLogin();
        
        await SetMentorAvailability(mentorClient, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));
        await SetMentorAvailability(mentorClient, DayOfWeek.Tuesday, new TimeOnly(10, 0), new TimeOnly(16, 0));

        // Book sessions for both mentees
        var session1Id = await BookSession(mentee1Client, mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(10));
        var session2Id = await BookSession(mentee2Client, mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekTuesday.AddHours(14));

        // Add reviews from both mentees
        await mentee1Client.PostAsJsonAsync(MentorshipsEndpoints.AddReview, new
        {
            SessionId = session1Id,
            Rating = 5,
            Comment = "Outstanding mentor! Highly recommended."
        });

        await mentee2Client.PostAsJsonAsync(MentorshipsEndpoints.AddReview, new
        {
            SessionId = session2Id,
            Rating = 4,
            Comment = "Very knowledgeable and patient."
        });

        // Act
        var response = await _client.GetAsync(
            MentorshipsEndpoints.GetMentorReviews.Replace("{mentorSlug}", mentorData.UserSlug.ToString()));

        // Assert
        response.EnsureSuccessStatusCode();
        var reviews = await response.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(reviews);
        Assert.Equal(2, reviews.Count);
    }

    [Fact]
    public async Task GetMentorReviews_ShouldReturnEmptyList_WhenMentorHasNoReviews()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();

        // Act
        var response = await _client.GetAsync(
            MentorshipsEndpoints.GetMentorReviews.Replace("{mentorSlug}", mentorData.UserSlug.ToString()));

        // Assert
        response.EnsureSuccessStatusCode();
        var reviews = await response.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(reviews);
        Assert.Empty(reviews);
    }

    [Fact]
    public async Task GetMentorReviews_ShouldReturnNotFound_WhenMentorDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync(
            MentorshipsEndpoints.GetMentorReviews.Replace("{mentorSlug}", "non-existent-mentor"));

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ReviewWorkflow_ShouldWorkEndToEnd_FromSessionToReview()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin(120.0m, "Senior software architect");
        var (menteeData, menteeClient) = await CreateMenteeAndLogin();
        
        // Set up availability and book session
        await SetMentorAvailability(mentorClient, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));
        var sessionId = await BookSession(menteeClient, mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(10), 120, "Advanced architecture consultation");

        // Confirm session
        await mentorClient.PutAsJsonAsync(
            MentorshipsEndpoints.ConfirmSession.Replace("{sessionId}", sessionId.ToString()), 
            new { GoogleMeetUrl = "https://meet.google.com/architecture-session" });

        // Simulate session completion and add review
        var reviewPayload = new
        {
            SessionId = sessionId,
            Rating = 5,
            Comment = "Exceptional session! The mentor provided deep insights into microservices architecture and helped me understand complex design patterns. The session was well-structured and interactive."
        };

        var addReviewResponse = await menteeClient.PostAsJsonAsync(MentorshipsEndpoints.AddReview, reviewPayload);
        addReviewResponse.EnsureSuccessStatusCode();

        // Verify review appears in mentor's reviews
        var reviewsResponse = await _client.GetAsync(
            MentorshipsEndpoints.GetMentorReviews.Replace("{mentorSlug}", mentorData.UserSlug.ToString()));
        
        reviewsResponse.EnsureSuccessStatusCode();
        var reviews = await reviewsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        
        Assert.NotNull(reviews);
        Assert.Single(reviews);

        // Update the review
        var reviewResult = await addReviewResponse.Content.ReadFromJsonAsync<dynamic>();
        var reviewId = reviewResult?.GetProperty("ReviewId").GetInt32();

        var updatePayload = new
        {
            Rating = 5,
            Comment = "Exceptional session! The mentor provided deep insights into microservices architecture. Updated: Also got great book recommendations!"
        };

        var updateResponse = await menteeClient.PutAsJsonAsync(
            MentorshipsEndpoints.UpdateReview.Replace("{reviewId}", reviewId.ToString()), 
            updatePayload);

        updateResponse.EnsureSuccessStatusCode();
    }
}
