using System.Net;
using System.Net.Http.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Users.Features;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships.Mentor;

public class MentorProfileTests : AuthenticationTestBase
{
    public MentorProfileTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task BecomeMentor_ShouldCreateMentorProfile_WhenValidDataProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var mentorData = new
        {
            HourlyRate = 50.0m,
            BufferTimeMinutes = 30
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, mentorData);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task BecomeMentor_ShouldCreateMentorProfile_WithDefaultBufferTime_WhenNotProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var mentorData = new
        {
            HourlyRate = 50.0m
            // BufferTimeMinutes not provided, should default to 30
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, mentorData);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task BecomeMentor_ShouldReturnBadRequest_WhenInvalidBufferTimeProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var mentorData = new
        {
            HourlyRate = 50.0m,
            BufferTimeMinutes = 25 // Invalid: not in 15-minute increments
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, mentorData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetMentorProfile_ShouldReturnProfile_WhenMentorExists()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var mentorSlug = "test-mentor";

        // Act
        var response = await userAct.GetAsync($"/mentorships/mentor/{mentorSlug}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateMentorProfile_ShouldUpdateProfile_WhenValidDataProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var updateData = new
        {
            HourlyRate = 75.0m,
            BufferTimeMinutes = 45
        };

        // Act
        var response = await userAct.PutAsJsonAsync(MentorshipsEndpoints.UpdateMentorProfile, updateData);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateMentorProfile_ShouldUpdateOnlyHourlyRate_WhenBufferTimeNotProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var updateData = new
        {
            HourlyRate = 75.0m
            // BufferTimeMinutes not provided, should keep existing value
        };

        // Act
        var response = await userAct.PutAsJsonAsync(MentorshipsEndpoints.UpdateMentorProfile, updateData);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateMentorProfile_ShouldReturnBadRequest_WhenInvalidBufferTimeProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var updateData = new
        {
            HourlyRate = 75.0m,
            BufferTimeMinutes = 20 // Invalid: not in 15-minute increments
        };

        // Act
        var response = await userAct.PutAsJsonAsync(MentorshipsEndpoints.UpdateMentorProfile, updateData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
} 