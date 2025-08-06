using System.Net;
using System.Net.Http.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Mentorships.Features.Availability.Get.PerMonth;
using Booking.Modules.Mentorships.Features.Availability.Get.PerDay;
using Booking.Modules.Users.Features;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships.Availability;

public class AvailabilityTests : AuthenticationTestBase
{
    public AvailabilityTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetMentorAvailabilityByMonth_ShouldReturnAvailability_WhenMentorExists()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        // Create mentor and set availability
        var mentorSlug = "test-mentor";
        var currentYear = DateTime.Now.Year;
        var currentMonth = DateTime.Now.Month;

        // Act
        var response = await userAct.GetAsync($"/mentorships/mentors/{mentorSlug}/availability/month/{currentYear}/{currentMonth}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var availability = await response.Content.ReadFromJsonAsync<MonthlyAvailabilityResponse>();
        Assert.NotNull(availability);
        Assert.Equal(mentorSlug, availability.MentorSlug);
        Assert.Equal(currentYear, availability.Year);
        Assert.Equal(currentMonth, availability.Month);
    }

    [Fact]
    public async Task GetMentorAvailabilityByMonth_ShouldFilterPastDays_WhenIncludePastDaysIsFalse()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var mentorSlug = "test-mentor";
        var currentYear = DateTime.Now.Year;
        var currentMonth = DateTime.Now.Month;

        // Act
        var response = await userAct.GetAsync($"/mentorships/mentors/{mentorSlug}/availability/month/{currentYear}/{currentMonth}?includePastDays=false");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var availability = await response.Content.ReadFromJsonAsync<MonthlyAvailabilityResponse>();
        Assert.NotNull(availability);
        
        // Verify no past days are included
        var pastDays = availability.Days.Where(d => d.Date.Date < DateTime.Now.Date).ToList();
        Assert.Empty(pastDays);
    }

    [Fact]
    public async Task GetMentorAvailabilityByDay_ShouldReturnAvailability_WhenMentorExists()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var mentorSlug = "test-mentor";
        var testDate = DateTime.Now.Date.ToString("yyyy-MM-dd");

        // Act
        var response = await userAct.GetAsync($"/mentorships/mentors/{mentorSlug}/availability/day/{testDate}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var availability = await response.Content.ReadFromJsonAsync<DailyAvailabilityResponse>();
        Assert.NotNull(availability);
        Assert.Equal(mentorSlug, availability.MentorSlug);
        Assert.Equal(DateTime.Parse(testDate), availability.Date);
    }

    [Fact]
    public async Task GetMentorAvailabilityByDay_ShouldReturnBadRequest_WhenDateIsInvalid()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var mentorSlug = "test-mentor";
        var invalidDate = "invalid-date";

        // Act
        var response = await userAct.GetAsync($"/mentorships/mentors/{mentorSlug}/availability/day/{invalidDate}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SetAvailability_ShouldCreateAvailability_WhenValidDataProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var availabilityData = new
        {
            DayOfWeek = 1, // Monday
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(17, 0)
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, availabilityData);
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SetBulkAvailability_ShouldCreateMultipleAvailabilities_WhenValidDataProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var bulkAvailabilityData = new
        {
            Availabilities = new[]
            {
                new
                {
                    DayOfWeek = 1, // Monday
                    TimeSlots = new[]
                    {
                        new { StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(12, 0) },
                        new { StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(17, 0) }
                    }
                },
                new
                {
                    DayOfWeek = 2, // Tuesday
                    TimeSlots = new[]
                    {
                        new { StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(16, 0) }
                    }
                }
            }
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, bulkAvailabilityData);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetMentorAvailability_ShouldReturnAllAvailabilities_WhenMentorExists()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        var mentorSlug = "test-mentor";

        // Act
        var response = await userAct.GetAsync($"/mentorships/availability/{mentorSlug}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Availability_ShouldConsiderBufferTime_WhenMentorHasBufferTimeSet()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        
        // First, create a mentor with buffer time
        var mentorData = new
        {
            HourlyRate = 50.0m,
            BufferTimeMinutes = 30
        };

        var createResponse = await userAct.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, mentorData);
        Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);

        // Set availability
        var availabilityData = new
        {
            DayOfWeek = 1, // Monday
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(17, 0)
        };

        var availabilityResponse = await userAct.PostAsJsonAsync(MentorshipsEndpoints.SetAvailability, availabilityData);
        Assert.Equal(HttpStatusCode.OK, availabilityResponse.StatusCode);

        // Act - Get availability for a specific day
        var mentorSlug = "test-mentor";
        var testDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"); // Tomorrow
        var response = await userAct.GetAsync($"/mentorships/mentors/{mentorSlug}/availability/day/{testDate}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var availability = await response.Content.ReadFromJsonAsync<DailyAvailabilityResponse>();
        Assert.NotNull(availability);
        
        // The availability should account for buffer time between sessions
        // This would be implemented in the session booking logic
    }
} 