using System.Net;
using System.Net.Http.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Users.Features;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships.Reviews;

public class ReviewTests : AuthenticationTestBase
{
    public ReviewTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task AddReview_ShouldCreateReview_WhenValidDataProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var reviewData = new
        {
            MentorSlug = "test-mentor",
            Rating = 5,
            Comment = "Excellent mentor! Very knowledgeable and patient.",
            SessionId = 1 // This would be a real session ID in practice
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.AddReview, reviewData);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetMentorReviews_ShouldReturnReviews_WhenMentorExists()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var mentorSlug = "test-mentor";

        // Act
        var response = await userAct.GetAsync($"/mentorships/reviews/{mentorSlug}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateReview_ShouldUpdateReview_WhenValidDataProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var reviewId = 1; // This would be a real review ID in practice
        var updateData = new
        {
            Rating = 4,
            Comment = "Updated review - still very good but could be more structured"
        };

        // Act
        var response = await userAct.PutAsJsonAsync($"/mentorships/reviews/{reviewId}", updateData);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeleteReview_ShouldDeleteReview_WhenReviewExists()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var reviewId = 1; // This would be a real review ID in practice

        // Act
        var response = await userAct.DeleteAsync($"/mentorships/reviews/{reviewId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
} 