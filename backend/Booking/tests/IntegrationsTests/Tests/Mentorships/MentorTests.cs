using System.Net;
using System.Net.Http.Json;
using Booking.Modules.Mentorships.Features;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships;

public class MentorTests : MentorshipTestBase
{
    public MentorTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task BecomeMentor_ShouldCreateMentor_WhenUserIsAuthenticated()
    {
        // Arrange
        var userData = await CreateUserAndLogin();

        var becomeMentorPayload = new
        {
            HourlyRate = 75.0m,
            Bio = "Experienced software developer with 10+ years in full-stack development"
        };

        // Act
        var response = await _client.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, becomeMentorPayload);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task BecomeMentor_ShouldReturnBadRequest_WhenUserIsAlreadyMentor()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();

        var becomeMentorPayload = new
        {
            HourlyRate = 100.0m,
            Bio = "Another attempt to become mentor"
        };

        // Act
        var response = await mentorClient.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, becomeMentorPayload);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BecomeMentor_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var becomeMentorPayload = new
        {
            HourlyRate = 50.0m,
            Bio = "Test bio"
        };

        // Act
        var response = await _client.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, becomeMentorPayload);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    [InlineData(-1)]
    public async Task BecomeMentor_ShouldReturnBadRequest_WhenHourlyRateIsInvalid(decimal invalidRate)
    {
        // Arrange
        var userData = await CreateUserAndLogin();

        var becomeMentorPayload = new
        {
            HourlyRate = invalidRate,
            Bio = "Test bio"
        };

        // Act
        var response = await _client.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, becomeMentorPayload);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateMentorProfile_ShouldUpdateProfile_WhenUserIsMentor()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin(50.0m, "Original bio");

        var updatePayload = new
        {
            HourlyRate = 80.0m,
            Bio = "Updated bio with more experience details"
        };

        // Act
        var response = await mentorClient.PutAsJsonAsync(MentorshipsEndpoints.UpdateMentorProfile, updatePayload);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task SetAvailability_ShouldCreateAvailability_WhenMentorIsAuthenticated()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();

        var availabilityPayload = new
        {
            DayOfWeek = DayOfWeek.Monday,
            StartTime = "09:00",
            EndTime = "17:00"
        };

        // Act
        var response = await mentorClient.PostAsJsonAsync(MentorshipsEndpoints.SetAvailability, availabilityPayload);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task SetAvailability_ShouldReturnBadRequest_WhenTimeRangeIsInvalid()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();

        var invalidAvailabilityPayload = new
        {
            DayOfWeek = DayOfWeek.Monday,
            StartTime = "17:00", // Start time after end time
            EndTime = "09:00"
        };

        // Act
        var response = await mentorClient.PostAsJsonAsync(MentorshipsEndpoints.SetAvailability, invalidAvailabilityPayload);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SetAvailability_ShouldReturnBadRequest_WhenOverlappingWithExistingAvailability()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();

        // Set initial availability
        await SetMentorAvailability(mentorClient, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));

        // Try to set overlapping availability
        var overlappingAvailabilityPayload = new
        {
            DayOfWeek = DayOfWeek.Monday,
            StartTime = "10:00", // Overlaps with existing 9:00-17:00
            EndTime = "18:00"
        };

        // Act
        var response = await mentorClient.PostAsJsonAsync(MentorshipsEndpoints.SetAvailability, overlappingAvailabilityPayload);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateAvailability_ShouldUpdateExistingAvailability_WhenMentorIsAuthenticated()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        var availabilityId = await SetMentorAvailability(mentorClient);

        var updatePayload = new
        {
            DayOfWeek = DayOfWeek.Tuesday,
            StartTime = "10:00",
            EndTime = "16:00"
        };

        // Act
        var response = await mentorClient.PutAsJsonAsync(
            MentorshipsEndpoints.UpdateAvailability.Replace("{availabilityId}", availabilityId.ToString()), 
            updatePayload);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task GetMentorAvailability_ShouldReturnAvailability_WhenMentorHasAvailability()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        
        // Set multiple availability slots
        foreach (var (day, start, end) in MentorshipTestData.Availability.WeekdayAvailability)
        {
            await SetMentorAvailability(mentorClient, day, start, end);
        }

        // Act
        var response = await _client.GetAsync(
            MentorshipsEndpoints.GetMentorAvailability.Replace("{mentorSlug}", mentorData.UserSlug.ToString()));

        // Assert
        response.EnsureSuccessStatusCode();
        var availabilities = await response.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(availabilities);
        Assert.Equal(5, availabilities.Count); // Should have 5 weekday slots
    }

    [Fact]
    public async Task GetMentorSessions_ShouldReturnEmptyList_WhenMentorHasNoSessions()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();

        // Act
        var response = await mentorClient.GetAsync(MentorshipsEndpoints.GetMentorSessions);

        // Assert
        response.EnsureSuccessStatusCode();
        var sessions = await response.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(sessions);
        Assert.Empty(sessions);
    }

    [Fact]
    public async Task GetMentorSessions_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Act
        var response = await _client.GetAsync(MentorshipsEndpoints.GetMentorSessions);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
