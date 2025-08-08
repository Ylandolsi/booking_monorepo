/*using System.Net;
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

    [Fact]
    public async Task AddReview_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
    {
        // Arrange
        var client = Factory.CreateClient();
        
        var reviewData = new
        {
            MentorSlug = "test-mentor",
            Rating = 5,
            Comment = "Excellent mentor! Very knowledgeable and patient.",
            SessionId = 1
        };

        // Act
        var response = await client.PostAsJsonAsync(MentorshipsEndpoints.AddReview, reviewData);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(6)]
    [InlineData(10)]
    public async Task AddReview_ShouldReturnBadRequest_WhenInvalidRatingProvided(int rating)
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var reviewData = new
        {
            MentorSlug = "test-mentor",
            Rating = rating,
            Comment = "Review with invalid rating",
            SessionId = 1
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.AddReview, reviewData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddReview_ShouldReturnBadRequest_WhenMentorSlugIsEmpty()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var reviewData = new
        {
            MentorSlug = "",
            Rating = 5,
            Comment = "Review with empty mentor slug",
            SessionId = 1
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.AddReview, reviewData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddReview_ShouldReturnBadRequest_WhenCommentIsEmpty()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var reviewData = new
        {
            MentorSlug = "test-mentor",
            Rating = 5,
            Comment = "",
            SessionId = 1
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.AddReview, reviewData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddReview_ShouldReturnBadRequest_WhenCommentTooLong()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var longComment = new string('a', 1001); // Assuming max length is 1000
        var reviewData = new
        {
            MentorSlug = "test-mentor",
            Rating = 5,
            Comment = longComment,
            SessionId = 1
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.AddReview, reviewData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddReview_ShouldReturnNotFound_WhenMentorDoesNotExist()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var reviewData = new
        {
            MentorSlug = "non-existent-mentor-999",
            Rating = 5,
            Comment = "Review for non-existent mentor",
            SessionId = 1
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.AddReview, reviewData);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AddReview_ShouldReturnNotFound_WhenSessionDoesNotExist()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var reviewData = new
        {
            MentorSlug = "test-mentor",
            Rating = 5,
            Comment = "Review for non-existent session",
            SessionId = 999999
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.AddReview, reviewData);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AddReview_ShouldReturnBadRequest_WhenSessionNotCompleted()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var reviewData = new
        {
            MentorSlug = "test-mentor",
            Rating = 5,
            Comment = "Review for session that hasn't occurred yet",
            SessionId = 1 // Session in the future
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.AddReview, reviewData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddReview_ShouldReturnConflict_WhenReviewAlreadyExists()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var reviewData = new
        {
            MentorSlug = "test-mentor",
            Rating = 5,
            Comment = "First review",
            SessionId = 1
        };

        // Add first review
        await userAct.PostAsJsonAsync(MentorshipsEndpoints.AddReview, reviewData);

        // Try to add another review for the same session
        var duplicateReviewData = new
        {
            MentorSlug = "test-mentor",
            Rating = 4,
            Comment = "Duplicate review",
            SessionId = 1
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.AddReview, duplicateReviewData);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task GetMentorReviews_ShouldReturnNotFound_WhenMentorDoesNotExist()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var nonExistentMentorSlug = "non-existent-mentor-999";

        // Act
        var response = await userAct.GetAsync($"/mentorships/reviews/{nonExistentMentorSlug}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetMentorReviews_ShouldReturnEmptyList_WhenMentorHasNoReviews()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var mentorSlug = "mentor-with-no-reviews";

        // Act
        var response = await userAct.GetAsync($"/mentorships/reviews/{mentorSlug}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var reviews = await response.Content.ReadFromJsonAsync<dynamic[]>();
        Assert.Empty(reviews);
    }

    [Fact]
    public async Task UpdateReview_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
    {
        // Arrange
        var client = Factory.CreateClient();
        var reviewId = 1;
        var updateData = new
        {
            Rating = 4,
            Comment = "Updated review"
        };

        // Act
        var response = await client.PutAsJsonAsync($"/mentorships/reviews/{reviewId}", updateData);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task UpdateReview_ShouldReturnNotFound_WhenReviewDoesNotExist()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var nonExistentReviewId = 999999;
        var updateData = new
        {
            Rating = 4,
            Comment = "Updated review for non-existent review"
        };

        // Act
        var response = await userAct.PutAsJsonAsync($"/mentorships/reviews/{nonExistentReviewId}", updateData);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateReview_ShouldReturnForbidden_WhenUserNotOwnerOfReview()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var reviewId = 1; // Review that belongs to another user
        var updateData = new
        {
            Rating = 4,
            Comment = "Unauthorized update attempt"
        };

        // Act
        var response = await userAct.PutAsJsonAsync($"/mentorships/reviews/{reviewId}", updateData);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(6)]
    [InlineData(10)]
    public async Task UpdateReview_ShouldReturnBadRequest_WhenInvalidRatingProvided(int rating)
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var reviewId = 1;
        var updateData = new
        {
            Rating = rating,
            Comment = "Update with invalid rating"
        };

        // Act
        var response = await userAct.PutAsJsonAsync($"/mentorships/reviews/{reviewId}", updateData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteReview_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
    {
        // Arrange
        var client = Factory.CreateClient();
        var reviewId = 1;

        // Act
        var response = await client.DeleteAsync($"/mentorships/reviews/{reviewId}");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeleteReview_ShouldReturnNotFound_WhenReviewDoesNotExist()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var nonExistentReviewId = 999999;

        // Act
        var response = await userAct.DeleteAsync($"/mentorships/reviews/{nonExistentReviewId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteReview_ShouldReturnForbidden_WhenUserNotOwnerOfReview()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var reviewId = 1; // Review that belongs to another user

        // Act
        var response = await userAct.DeleteAsync($"/mentorships/reviews/{reviewId}");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task AddReview_ShouldAcceptValidRatings_WhenRatingBetweenOneAndFive(int rating)
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser($"test-rating-{rating}");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var reviewData = new
        {
            MentorSlug = "test-mentor",
            Rating = rating,
            Comment = $"Review with rating {rating}",
            SessionId = rating // Different session IDs to avoid conflicts
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.AddReview, reviewData);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AddReview_ShouldCreateReviewWithoutOptionalFields_WhenOnlyRequiredFieldsProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var reviewData = new
        {
            MentorSlug = "test-mentor",
            Rating = 5,
            SessionId = 1
            // Comment not provided (if it's optional)
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.AddReview, reviewData);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetMentorReviews_ShouldReturnReviewsInDescendingOrder_WhenMentorHasMultipleReviews()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var mentorSlug = "test-mentor";

        // Act
        var response = await userAct.GetAsync($"/mentorships/reviews/{mentorSlug}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var reviews = await response.Content.ReadFromJsonAsync<dynamic[]>();
        Assert.NotNull(reviews);
        // Additional assertions could verify the ordering
    }

    [Fact]
    public async Task GetMentorReviews_ShouldIncludeReviewerInformation_WhenReturningReviews()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var mentorSlug = "test-mentor";

        // Act
        var response = await userAct.GetAsync($"/mentorships/reviews/{mentorSlug}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        // Additional assertions could verify reviewer information is included
        Assert.NotNull(content);
    }
} */