/*using System.Net;
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

    [Fact]
    public async Task BecomeMentor_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
    {
        // Arrange
        var client = Factory.CreateClient();
        
        var mentorData = new
        {
            HourlyRate = 50.0m,
            BufferTimeMinutes = 30
        };

        // Act
        var response = await client.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, mentorData);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task BecomeMentor_ShouldReturnBadRequest_WhenNegativeHourlyRateProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var mentorData = new
        {
            HourlyRate = -10.0m,
            BufferTimeMinutes = 30
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, mentorData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BecomeMentor_ShouldReturnBadRequest_WhenZeroHourlyRateProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var mentorData = new
        {
            HourlyRate = 0.0m,
            BufferTimeMinutes = 30
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, mentorData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BecomeMentor_ShouldReturnBadRequest_WhenBufferTimeExceedsMaximum()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var mentorData = new
        {
            HourlyRate = 50.0m,
            BufferTimeMinutes = 120 // Assuming maximum is less than 120
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, mentorData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BecomeMentor_ShouldReturnBadRequest_WhenNegativeBufferTimeProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var mentorData = new
        {
            HourlyRate = 50.0m,
            BufferTimeMinutes = -15
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, mentorData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BecomeMentor_ShouldReturnConflict_WhenUserIsAlreadyMentor()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var mentorData = new
        {
            HourlyRate = 50.0m,
            BufferTimeMinutes = 30
        };

        // First become a mentor
        await userAct.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, mentorData);

        // Act - Try to become mentor again
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, mentorData);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task GetMentorProfile_ShouldReturnNotFound_WhenMentorDoesNotExist()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var nonExistentMentorSlug = "non-existent-mentor-999";

        // Act
        var response = await userAct.GetAsync($"/mentorships/mentor/{nonExistentMentorSlug}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetMentorProfile_ShouldReturnBadRequest_WhenInvalidSlugProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var invalidSlug = ""; // Empty slug

        // Act
        var response = await userAct.GetAsync($"/mentorships/mentor/{invalidSlug}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateMentorProfile_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
    {
        // Arrange
        var client = Factory.CreateClient();
        
        var updateData = new
        {
            HourlyRate = 75.0m,
            BufferTimeMinutes = 45
        };

        // Act
        var response = await client.PutAsJsonAsync(MentorshipsEndpoints.UpdateMentorProfile, updateData);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task UpdateMentorProfile_ShouldReturnBadRequest_WhenNegativeHourlyRateProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var updateData = new
        {
            HourlyRate = -25.0m,
            BufferTimeMinutes = 30
        };

        // Act
        var response = await userAct.PutAsJsonAsync(MentorshipsEndpoints.UpdateMentorProfile, updateData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateMentorProfile_ShouldReturnBadRequest_WhenZeroHourlyRateProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var updateData = new
        {
            HourlyRate = 0.0m,
            BufferTimeMinutes = 30
        };

        // Act
        var response = await userAct.PutAsJsonAsync(MentorshipsEndpoints.UpdateMentorProfile, updateData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateMentorProfile_ShouldReturnNotFound_WhenUserIsNotMentor()
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
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData(15)]
    [InlineData(30)]
    [InlineData(45)]
    [InlineData(60)]
    public async Task BecomeMentor_ShouldAcceptValidBufferTimes_WhenMultiplesOfFifteen(int bufferTime)
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser($"test-{bufferTime}");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var mentorData = new
        {
            HourlyRate = 50.0m,
            BufferTimeMinutes = bufferTime
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, mentorData);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData(10.0)]
    [InlineData(25.5)]
    [InlineData(50.0)]
    [InlineData(100.0)]
    [InlineData(500.0)]
    public async Task BecomeMentor_ShouldAcceptValidHourlyRates_WhenPositiveValues(decimal hourlyRate)
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser($"test-rate-{hourlyRate}");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var mentorData = new
        {
            HourlyRate = hourlyRate,
            BufferTimeMinutes = 30
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, mentorData);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateMentorProfile_ShouldUpdateProfileSuccessfully_AfterBecomingMentor()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        // First become a mentor
        var mentorData = new
        {
            HourlyRate = 50.0m,
            BufferTimeMinutes = 30
        };
        
        var createResponse = await userAct.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, mentorData);
        Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);
        
        // Prepare update data
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
    public async Task BecomeMentor_ShouldCreateUniqueProfile_ForDifferentUsers()
    {
        // Arrange
        var (userArrange1, userAct1) = GetClientsForUser("test1");
        var loginData1 = await CreateUserAndLogin(null, null, userArrange1);
        
        var (userArrange2, userAct2) = GetClientsForUser("test2");
        var loginData2 = await CreateUserAndLogin(null, null, userArrange2);
        
        var mentorData1 = new
        {
            HourlyRate = 50.0m,
            BufferTimeMinutes = 30
        };
        
        var mentorData2 = new
        {
            HourlyRate = 75.0m,
            BufferTimeMinutes = 45
        };

        // Act
        var response1 = await userAct1.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, mentorData1);
        var response2 = await userAct2.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, mentorData2);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
        Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
    }
} */