using System.Net;
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
} 