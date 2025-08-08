/*using System.Net;
using System.Net.Http.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Users.Features;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships.Relationships;

public class MentorshipRelationshipTests : AuthenticationTestBase
{
    public MentorshipRelationshipTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task RequestMentorship_ShouldCreateRequest_WhenValidDataProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var requestData = new
        {
            MentorSlug = "test-mentor",
            Message = "I would like to learn more about Clean Architecture from you"
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.RequestMentorship, requestData);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AcceptMentorship_ShouldAcceptRequest_WhenRequestExists()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var relationshipId = 1; // This would be a real relationship ID in practice

        // Act
        var response = await userAct.PostAsJsonAsync($"/mentorships/relationships/{relationshipId}/accept", new { });

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task RejectMentorship_ShouldRejectRequest_WhenRequestExists()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var relationshipId = 1; // This would be a real relationship ID in practice

        // Act
        var response = await userAct.PostAsJsonAsync($"/mentorships/relationships/{relationshipId}/reject", new { });

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task EndMentorship_ShouldEndRelationship_WhenRelationshipExists()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var relationshipId = 1; // This would be a real relationship ID in practice

        // Act
        var response = await userAct.PostAsJsonAsync($"/mentorships/relationships/{relationshipId}/end", new { });

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetMentorshipRelationships_ShouldReturnRelationships_WhenUserExists()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);

        // Act
        var response = await userAct.GetAsync(MentorshipsEndpoints.GetMentorshipRelationships);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task RequestMentorship_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
    {
        // Arrange
        var client = Factory.CreateClient();
        
        var requestData = new
        {
            MentorSlug = "test-mentor",
            Message = "I would like to learn more about Clean Architecture from you"
        };

        // Act
        var response = await client.PostAsJsonAsync(MentorshipsEndpoints.RequestMentorship, requestData);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RequestMentorship_ShouldReturnBadRequest_WhenMentorSlugIsEmpty()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var requestData = new
        {
            MentorSlug = "",
            Message = "I would like to learn more about Clean Architecture from you"
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.RequestMentorship, requestData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task RequestMentorship_ShouldReturnBadRequest_WhenMessageIsEmpty()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var requestData = new
        {
            MentorSlug = "test-mentor",
            Message = ""
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.RequestMentorship, requestData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task RequestMentorship_ShouldReturnBadRequest_WhenMessageTooLong()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var longMessage = new string('a', 1001); // Assuming max length is 1000
        var requestData = new
        {
            MentorSlug = "test-mentor",
            Message = longMessage
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.RequestMentorship, requestData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task RequestMentorship_ShouldReturnNotFound_WhenMentorDoesNotExist()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var requestData = new
        {
            MentorSlug = "non-existent-mentor-999",
            Message = "I would like to learn from a non-existent mentor"
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.RequestMentorship, requestData);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task RequestMentorship_ShouldReturnBadRequest_WhenUserTriesToRequestThemselvesAsMentor()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        // Assuming the user's slug is derived from their email/username
        var requestData = new
        {
            MentorSlug = "test", // Same as user's slug
            Message = "I would like to mentor myself"
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.RequestMentorship, requestData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task RequestMentorship_ShouldReturnConflict_WhenMentorshipAlreadyRequested()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var requestData = new
        {
            MentorSlug = "test-mentor",
            Message = "First mentorship request"
        };

        // Send first request
        await userAct.PostAsJsonAsync(MentorshipsEndpoints.RequestMentorship, requestData);

        // Try to send another request to the same mentor
        var duplicateRequestData = new
        {
            MentorSlug = "test-mentor",
            Message = "Duplicate mentorship request"
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.RequestMentorship, duplicateRequestData);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task AcceptMentorship_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
    {
        // Arrange
        var client = Factory.CreateClient();
        var relationshipId = 1;

        // Act
        var response = await client.PostAsJsonAsync($"/mentorships/relationships/{relationshipId}/accept", new { });

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AcceptMentorship_ShouldReturnNotFound_WhenRelationshipDoesNotExist()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var nonExistentRelationshipId = 999999;

        // Act
        var response = await userAct.PostAsJsonAsync($"/mentorships/relationships/{nonExistentRelationshipId}/accept", new { });

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AcceptMentorship_ShouldReturnForbidden_WhenUserNotAuthorizedToAccept()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var relationshipId = 1; // Relationship where user is not the mentor

        // Act
        var response = await userAct.PostAsJsonAsync($"/mentorships/relationships/{relationshipId}/accept", new { });

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task AcceptMentorship_ShouldReturnBadRequest_WhenRelationshipAlreadyAccepted()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var relationshipId = 1;

        // Accept the relationship first
        await userAct.PostAsJsonAsync($"/mentorships/relationships/{relationshipId}/accept", new { });

        // Act - Try to accept again
        var response = await userAct.PostAsJsonAsync($"/mentorships/relationships/{relationshipId}/accept", new { });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task RejectMentorship_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
    {
        // Arrange
        var client = Factory.CreateClient();
        var relationshipId = 1;

        // Act
        var response = await client.PostAsJsonAsync($"/mentorships/relationships/{relationshipId}/reject", new { });

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RejectMentorship_ShouldReturnNotFound_WhenRelationshipDoesNotExist()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var nonExistentRelationshipId = 999999;

        // Act
        var response = await userAct.PostAsJsonAsync($"/mentorships/relationships/{nonExistentRelationshipId}/reject", new { });

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task RejectMentorship_ShouldReturnForbidden_WhenUserNotAuthorizedToReject()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var relationshipId = 1; // Relationship where user is not the mentor

        // Act
        var response = await userAct.PostAsJsonAsync($"/mentorships/relationships/{relationshipId}/reject", new { });

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task RejectMentorship_ShouldReturnBadRequest_WhenRelationshipAlreadyProcessed()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var relationshipId = 1;

        // Accept the relationship first
        await userAct.PostAsJsonAsync($"/mentorships/relationships/{relationshipId}/accept", new { });

        // Act - Try to reject after accepting
        var response = await userAct.PostAsJsonAsync($"/mentorships/relationships/{relationshipId}/reject", new { });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task EndMentorship_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
    {
        // Arrange
        var client = Factory.CreateClient();
        var relationshipId = 1;

        // Act
        var response = await client.PostAsJsonAsync($"/mentorships/relationships/{relationshipId}/end", new { });

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task EndMentorship_ShouldReturnNotFound_WhenRelationshipDoesNotExist()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var nonExistentRelationshipId = 999999;

        // Act
        var response = await userAct.PostAsJsonAsync($"/mentorships/relationships/{nonExistentRelationshipId}/end", new { });

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task EndMentorship_ShouldReturnForbidden_WhenUserNotPartOfRelationship()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var relationshipId = 1; // Relationship where user is neither mentor nor mentee

        // Act
        var response = await userAct.PostAsJsonAsync($"/mentorships/relationships/{relationshipId}/end", new { });

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task EndMentorship_ShouldReturnBadRequest_WhenRelationshipNotActive()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var relationshipId = 1; // Relationship that's not in active state

        // Act
        var response = await userAct.PostAsJsonAsync($"/mentorships/relationships/{relationshipId}/end", new { });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task EndMentorship_ShouldReturnBadRequest_WhenRelationshipAlreadyEnded()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var relationshipId = 1;

        // End the relationship first
        await userAct.PostAsJsonAsync($"/mentorships/relationships/{relationshipId}/end", new { });

        // Act - Try to end again
        var response = await userAct.PostAsJsonAsync($"/mentorships/relationships/{relationshipId}/end", new { });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetMentorshipRelationships_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
    {
        // Arrange
        var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync(MentorshipsEndpoints.GetMentorshipRelationships);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetMentorshipRelationships_ShouldReturnEmptyList_WhenUserHasNoRelationships()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("new-user-no-relationships");
        var loginData = await CreateUserAndLogin(null, null, userArrange);

        // Act
        var response = await userAct.GetAsync(MentorshipsEndpoints.GetMentorshipRelationships);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var relationships = await response.Content.ReadFromJsonAsync<dynamic[]>();
        Assert.Empty(relationships);
    }

    [Fact]
    public async Task RequestMentorship_ShouldCreateMultipleRequests_WhenRequestingDifferentMentors()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var request1 = new
        {
            MentorSlug = "mentor-1",
            Message = "Request to mentor 1"
        };
        
        var request2 = new
        {
            MentorSlug = "mentor-2",
            Message = "Request to mentor 2"
        };

        // Act
        var response1 = await userAct.PostAsJsonAsync(MentorshipsEndpoints.RequestMentorship, request1);
        var response2 = await userAct.PostAsJsonAsync(MentorshipsEndpoints.RequestMentorship, request2);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
        Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
    }

    [Fact]
    public async Task MentorshipWorkflow_ShouldCompleteSuccessfully_WhenFollowingHappyPath()
    {
        // Arrange
        var (menteeArrange, menteeAct) = GetClientsForUser("mentee");
        var menteeLoginData = await CreateUserAndLogin(null, null, menteeArrange);
        
        var (mentorArrange, mentorAct) = GetClientsForUser("mentor");
        var mentorLoginData = await CreateUserAndLogin(null, null, mentorArrange);
        
        // Step 1: Request mentorship
        var requestData = new
        {
            MentorSlug = "mentor",
            Message = "I would like to learn Clean Architecture"
        };
        
        var requestResponse = await menteeAct.PostAsJsonAsync(MentorshipsEndpoints.RequestMentorship, requestData);
        Assert.Equal(HttpStatusCode.OK, requestResponse.StatusCode);
        
        // Get the relationship ID (in real scenario, this would come from the response or be queried)
        var relationshipId = 1;
        
        // Step 2: Accept mentorship
        var acceptResponse = await mentorAct.PostAsJsonAsync($"/mentorships/relationships/{relationshipId}/accept", new { });
        Assert.Equal(HttpStatusCode.OK, acceptResponse.StatusCode);
        
        // Step 3: Verify relationship exists
        var relationshipsResponse = await menteeAct.GetAsync(MentorshipsEndpoints.GetMentorshipRelationships);
        Assert.Equal(HttpStatusCode.OK, relationshipsResponse.StatusCode);
        
        // Step 4: End mentorship
        var endResponse = await menteeAct.PostAsJsonAsync($"/mentorships/relationships/{relationshipId}/end", new { });

        // Assert
        Assert.Equal(HttpStatusCode.OK, endResponse.StatusCode);
    }
} */