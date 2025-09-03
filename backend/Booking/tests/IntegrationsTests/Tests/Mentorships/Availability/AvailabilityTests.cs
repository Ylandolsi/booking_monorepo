using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Mentorships.Features.Availability.Get.PerMonth;
using Booking.Modules.Mentorships.Features.Availability.Get.PerDay;
using Booking.Modules.Users.Features;
using Booking.Modules.Users.Features.Authentication.Me;
using IntegrationsTests.Abstractions;
using IntegrationsTests.Abstractions.Base;
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

        var currentUser = await MentorshipTestUtilities.GetCurrentUserInfo(userArrange);

        await MentorshipTestUtilities.SetupMentorAvailabilityWithRanges(userArrange, DayOfWeek.Monday, 
            new[] { ("09:00", "12:00"), ("14:00", "17:00") });

        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);

        // Act
        var (publicArrange, publicAct) = GetClientsForUser("public_user");
        var response = await publicAct.GetAsync(
            $"{MentorshipEndpoints.Availability.GetDaily}?mentorSlug={currentUser.GetProperty("slug").GetString()}&date={nextMonday:yyyy-MM-dd}");
        
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

        var currentUser = await MentorshipTestUtilities.GetCurrentUserInfo(userArrange);
        
        // Set Thursday as inactive
        var bulkRequest = new
        {
            DayAvailabilities = new[]
            {
                new
                {
                    DayOfWeek = DayOfWeek.Thursday,
                    IsActive = false,
                    AvailabilityRanges = new object[0]
                }
            }
        };
        await userArrange.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, bulkRequest);

        var nextThursday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Thursday);

        // Act
        var (publicArrange, publicAct) = GetClientsForUser("public_user_2");
        var response = await publicAct.GetAsync(
            $"{MentorshipEndpoints.Availability.GetDaily}?mentorSlug={currentUser.GetProperty("slug").GetString()}&date={nextThursday:yyyy-MM-dd}");

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

        var currentUser = await MentorshipTestUtilities.GetCurrentUserInfo(userArrange);
        
        // Use the shared utility for mixed availability
        var bulkRequest = MentorshipTestUtilities.CreateMixedAvailabilityData();
        await userArrange.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, bulkRequest);

        var currentDate = DateTime.Now;

        // Act
        var (publicArrange, publicAct) = GetClientsForUser("public_monthly");
        var response = await publicAct.GetAsync(
            $"{MentorshipEndpoints.Availability.GetMonthly}?mentorSlug={currentUser.GetProperty("slug").GetString()}&year={currentDate.Year}&month={currentDate.Month}");
        // await MatchSnapshotAsync(response, "GetMentorAvailabilityByMonth_ShouldReturnCompleteMonth_WithMixedActiveDays" , matchOptions => matchOptions.IgnoreAllFields("date"));

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var monthlyAvailability = await response.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(monthlyAvailability);
        Assert.Equal(currentDate.Year, ((JsonElement)monthlyAvailability.GetProperty("year")).GetInt32());
        Assert.Equal(currentDate.Month, ((JsonElement)monthlyAvailability.GetProperty("month")).GetInt32());
        Assert.True(((JsonElement)monthlyAvailability.GetProperty("days")).GetArrayLength() > 0);
    }

    #endregion


}