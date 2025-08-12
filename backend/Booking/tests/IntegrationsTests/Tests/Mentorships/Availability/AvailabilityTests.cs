using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Mentorships.Features.Availability.Get.PerMonth;
using Booking.Modules.Mentorships.Features.Availability.Get.PerDay;
using Booking.Modules.Users.Features;
using Booking.Modules.Users.Features.Authentication.Me;
using IntegrationsTests.Abstractions;
using Snapshooter.Xunit;

namespace IntegrationsTests.Tests.Mentorships.Availability;

public class AvailabilityTests : MentorshipTestBase
{
    public AvailabilityTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    #region GetMentorAvailabilityByDay

    [Fact]
    public async Task GetMentorAvailabilityByDay_ShouldReturnCorrectSlots_WhenDayHasAvailability()
    {
        var (userArrange, userAct) = await CreateMentor("mentor_daily_checkt");

        var currentUser = await GetCurrenUserInfo(userArrange);

        var bulkRequest = new
        {
            DayAvailabilities = new[]
            {
                new
                {
                    DayOfWeek = DayOfWeek.Monday,
                    IsActive = true,
                    AvailabilityRanges = new[]
                    {
                        new { StartTime = "09:00", EndTime = "12:00" },
                        new { StartTime = "14:00", EndTime = "17:00" }
                    }
                }
            }
        };
        await userArrange.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, bulkRequest);

        var nextMonday = GetNextWeekday(DayOfWeek.Monday);

        // Act
        var (publicArrange, publicAct) = GetClientsForUser("public_user");
        var response = await publicAct.GetAsync(
            $"{MentorshipEndpoints.Availability.GetDaily}?mentorSlug={currentUser.Slug}&date={nextMonday:yyyy-MM-dd}");
        
        await MatchSnapshotAsync(response, "GetMentorAvailabilityByDay_ShouldReturnCorrectSlots_WhenDayHasAvailability" , matchOptions => matchOptions.IgnoreAllFields("date"));

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var availability = await response.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(availability);
        Assert.True(((JsonElement)availability.GetProperty("isAvailable")).GetBoolean());
        Assert.True(((JsonElement)availability.GetProperty("timeSlots")).GetArrayLength() > 0);
    }

    [Fact]
    public async Task GetMentorAvailabilityByDay_ShouldReturnNoSlots_WhenDayIsInactive()
    {
        var (userArrange, userAct) = await CreateMentor("mentor_daily_checkt");

        var currentUser = await GetCurrenUserInfo(userArrange);

        var bulkRequest = new
        {
            DayAvailabilities = new[]
            {
                new
                {
                    DayOfWeek = DayOfWeek.Thursday,
                    IsActive = false, // Day is inactive
                    AvailabilityRanges = new object[0]
                }
            }
        };
        await userArrange.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, bulkRequest);

        var nextThursday = GetNextWeekday(DayOfWeek.Thursday);

        // Act
        var (publicArrange, publicAct) = GetClientsForUser("public_user_2");
        var response = await publicAct.GetAsync(
            $"{MentorshipEndpoints.Availability.GetDaily}?mentorSlug={currentUser.Slug}&date={nextThursday:yyyy-MM-dd}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var availability = await response.Content.ReadFromJsonAsync<dynamic>();
        Assert.False(((JsonElement)availability.GetProperty("isAvailable")).GetBoolean());
        Assert.Equal(0, ((JsonElement)availability.GetProperty("timeSlots")).GetArrayLength());
        
    }

    #endregion

    #region GetMentorAvailabilityByMonth

    [Fact]
    public async Task GetMentorAvailabilityByMonth_ShouldReturnCompleteMonth_WithMixedActiveDays()
    {
        // Arrange
        var (userArrange, userAct) = await CreateMentor("mentor_daily_checkt");

        var currentUser = await GetCurrenUserInfo(userArrange);
        var bulkRequest = new
        {
            DayAvailabilities = new[]
            {
                new
                {
                    DayOfWeek = DayOfWeek.Monday, IsActive = true,
                    AvailabilityRanges = new[] { new { StartTime = "09:00", EndTime = "17:00" } }
                },
                new
                {
                    DayOfWeek = DayOfWeek.Wednesday, IsActive = true,
                    AvailabilityRanges = new[] { new { StartTime = "10:00", EndTime = "16:00" } }
                },
                new
                {
                    DayOfWeek = DayOfWeek.Thursday, IsActive = true,
                    AvailabilityRanges = new[] { new { StartTime = "08:00", EndTime = "14:00" } }
                },
                /*
                new { DayOfWeek = DayOfWeek.Friday, IsActive = false, AvailabilityRanges = new object[0] }
            */
            }
        };
        await userArrange.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, bulkRequest);

        var currentDate = DateTime.Now;

        // Act
        var (publicArrange, publicAct) = GetClientsForUser("public_monthly");
        var response = await publicAct.GetAsync(
            $"{MentorshipEndpoints.Availability.GetMonthly}?mentorSlug={currentUser.Slug}&year={currentDate.Year}&month={currentDate.Month}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var monthlyAvailability = await response.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(monthlyAvailability);
        Assert.Equal(currentDate.Year, ((JsonElement)monthlyAvailability.GetProperty("year")).GetInt32());
        Assert.Equal(currentDate.Month, ((JsonElement)monthlyAvailability.GetProperty("month")).GetInt32());
        Assert.True(((JsonElement)monthlyAvailability.GetProperty("days")).GetArrayLength() > 0);
    }

    #endregion


    private DateTime GetNextWeekday(DayOfWeek dayOfWeek)
    {
        var today = DateTime.Now.Date;
        var daysUntilTarget = ((int)dayOfWeek - (int)today.DayOfWeek + 7) % 7;
        if (daysUntilTarget == 0) daysUntilTarget = 7; // Next week
        return today.AddDays(daysUntilTarget);
    }


    /*
    [Fact]
    public async Task Availability_ShouldHandleTimeZoneCorrectly_WhenQueryingByDate()
    {
        var (userArrange, userAct) = await CreateMentor("mentorTest");


        await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, new
        {
            DayOfWeek = DayOfWeek.Sunday,
            StartTime = new TimeOnly(8, 0),
            EndTime = new TimeOnly(20, 0)
        });

        var nextSunday = GetNextSunday();

        // Act
        var response = await userAct.GetAsync(
            $"/mentorships/mentors/{loginData.UserSlug}/availability/day?date={nextSunday:yyyy-MM-dd}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var availability = await response.Content.ReadFromJsonAsync<DailyAvailabilityResponse>();
        Assert.NotNull(availability);
        Assert.Equal(nextSunday.Date, availability.Date.Date);
        Assert.True(availability.IsAvailable);
    }
    */
    

    private DateTime GetNextMonday()
    {
        var today = DateTime.Now.Date;
        var daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
        if (daysUntilMonday == 0) daysUntilMonday = 7;
        return today.AddDays(daysUntilMonday);
    }

    private DateTime GetNextSunday()
    {
        var today = DateTime.Now.Date;
        var daysUntilSunday = ((int)DayOfWeek.Sunday - (int)today.DayOfWeek + 7) % 7;
        if (daysUntilSunday == 0) daysUntilSunday = 7;
        return today.AddDays(daysUntilSunday);
    }
}