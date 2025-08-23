using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Users.Features;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships.Sessions;

public class SessionTimezoneEdgeCaseTests : MentorshipTestBase
{
    public SessionTimezoneEdgeCaseTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    #region Daylight Saving Time (DST) Tests

    [Fact]
    public async Task BookSession_ShouldHandleDSTTransition_SpringForward()
    {
        // Arrange - Test DST transition when clocks "spring forward" (lose an hour)
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_dst_spring");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_dst_spring");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Sunday, "01:00", "04:00");

        // DST transition date for 2025 in Europe/Paris (last Sunday of March)
        var dstTransitionDate = new DateTime(2025, 3, 30); // When 2:00 AM becomes 3:00 AM
        
        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = dstTransitionDate.ToString("yyyy-MM-dd"),
            StartTime = "02:30", // This time doesn't exist due to DST
            EndTime = "03:30",
            TimeZoneId = "Europe/Paris"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert - System should handle this gracefully (either accept with adjustment or reject)
        // The exact behavior depends on the timezone library implementation
        Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task BookSession_ShouldHandleDSTTransition_FallBack()
    {
        // Arrange - Test DST transition when clocks "fall back" (gain an hour)
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_dst_fall");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_dst_fall");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Sunday, "01:00", "04:00");

        // DST transition date for 2025 in Europe/Paris (last Sunday of October)
        var dstTransitionDate = new DateTime(2025, 10, 26); // When 3:00 AM becomes 2:00 AM
        
        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = dstTransitionDate.ToString("yyyy-MM-dd"),
            StartTime = "02:30", // This time occurs twice due to DST
            EndTime = "03:30",
            TimeZoneId = "Europe/Paris"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldSucceed_BeforeDSTTransition()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_before_dst");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_before_dst");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Sunday, "01:00", "04:00");

        var beforeDstDate = new DateTime(2025, 3, 29); // Day before DST transition
        
        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = beforeDstDate.ToString("yyyy-MM-dd"),
            StartTime = "02:30",
            EndTime = "03:30",
            TimeZoneId = "Europe/Paris"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldSucceed_AfterDSTTransition()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_after_dst");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_after_dst");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "01:00", "04:00");

        var afterDstDate = new DateTime(2025, 3, 31); // Day after DST transition
        
        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = afterDstDate.ToString("yyyy-MM-dd"),
            StartTime = "02:30",
            EndTime = "03:30",
            TimeZoneId = "Europe/Paris"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    #endregion

    #region Cross-Date Boundary Tests

    [Fact]
    public async Task BookSession_ShouldHandleCrossDateBoundary_PacificToEuropean()
    {
        // Arrange - When it's late evening in Pacific, it's early morning next day in Europe
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_cross_pacific");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_cross_pacific");

        // Mentor available early morning in London time
        await SetupMentorAvailability(mentorArrange, DayOfWeek.Tuesday, "08:00", "12:00");

        var mondayInPacific = GetNextWeekday(DayOfWeek.Monday);
        
        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = mondayInPacific.ToString("yyyy-MM-dd"), // Monday in Pacific
            StartTime = "23:00", // 11 PM Monday Pacific = 8 AM Tuesday London
            EndTime = "23:59",
            TimeZoneId = "America/Los_Angeles"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldHandleCrossDateBoundary_AsianToAmerican()
    {
        // Arrange - When it's early morning in Asia, it's previous day evening in America
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_cross_asian");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_cross_asian");

        // Mentor available Monday evening in New York time
        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "18:00", "22:00");

        var tuesdayInTokyo = GetNextWeekday(DayOfWeek.Tuesday);
        
        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = tuesdayInTokyo.ToString("yyyy-MM-dd"), // Tuesday in Tokyo
            StartTime = "08:00", // 8 AM Tuesday Tokyo = 6 PM Monday New York (in winter)
            EndTime = "09:00",
            TimeZoneId = "Asia/Tokyo"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldHandleCrossDateBoundary_EuropeanToAustralian()
    {
        // Arrange - When it's evening in Europe, it's next day morning in Australia
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_cross_european");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_cross_european");

        // Mentor available Tuesday morning in Sydney time
        await SetupMentorAvailability(mentorArrange, DayOfWeek.Tuesday, "09:00", "12:00");

        var mondayInParis = GetNextWeekday(DayOfWeek.Monday);
        
        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = mondayInParis.ToString("yyyy-MM-dd"), // Monday in Paris
            StartTime = "22:00", // 10 PM Monday Paris = ~8 AM Tuesday Sydney
            EndTime = "23:00",
            TimeZoneId = "Europe/Paris"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    #endregion

    #region Extreme Timezone Offset Tests

    [Fact]
    public async Task BookSession_ShouldHandleExtremePlusOffset_Samoa()
    {
        // Arrange - Test with one of the furthest positive UTC offsets
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_samoa");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_samoa");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = GetNextWeekday(DayOfWeek.Monday);
        
        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "14:00",
            EndTime = "15:00",
            TimeZoneId = "Pacific/Apia" // UTC+13/+14 (with DST)
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldHandleExtremeMinusOffset_BakerIsland()
    {
        // Arrange - Test with one of the furthest negative UTC offsets
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_baker");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_baker");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = GetNextWeekday(DayOfWeek.Monday);
        
        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "14:00",
            EndTime = "15:00",
            TimeZoneId = "Pacific/Midway" // UTC-11
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldHandleNepalTimezone_UnusualOffset()
    {
        // Arrange - Test with Nepal's unusual UTC+5:45 offset
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_nepal");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_nepal");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = GetNextWeekday(DayOfWeek.Monday);
        
        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "14:00",
            EndTime = "15:00",
            TimeZoneId = "Asia/Kathmandu" // UTC+5:45
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldHandleIndiaTimezone_ThirtyMinuteOffset()
    {
        // Arrange - Test with India's UTC+5:30 offset
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_india");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_india");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = GetNextWeekday(DayOfWeek.Monday);
        
        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "10:30", // Use 30-minute boundary
            EndTime = "11:30",
            TimeZoneId = "Asia/Kolkata" // UTC+5:30
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    #endregion

    #region Concurrent Booking Tests with Timezones

    [Fact]
    public async Task BookSession_ShouldPreventConflicts_AcrossTimezones()
    {
        // Arrange - Test that timezone conversions don't create conflicts
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_tz_conflict");
        var (mentee1Arrange, mentee1Act) = await CreateMentee("mentee1_tz_conflict");
        var (mentee2Arrange, mentee2Act) = await CreateMentee("mentee2_tz_conflict");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = GetNextWeekday(DayOfWeek.Monday);
        var mentorSlug = await GetMentorSlug(mentorArrange);

        // First booking in UTC
        var firstBooking = new
        {
            MentorSlug = mentorSlug,
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "13:00", // 1 PM UTC = 2 PM CET
            EndTime = "14:00",   // 2 PM UTC = 3 PM CET
            TimeZoneId = "UTC"
        };

        var firstResponse = await mentee1Act.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, firstBooking);
        Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);

        // Conflicting booking in CET (same time in mentor's timezone)
        var conflictingBooking = new
        {
            MentorSlug = mentorSlug,
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "14:00", // 2 PM CET = 1 PM UTC
            EndTime = "15:00",   // 3 PM CET = 2 PM UTC
            TimeZoneId = "Europe/Paris"
        };

        // Act
        var response = await mentee2Act.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, conflictingBooking);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldAllowNonConflicting_DifferentTimezones()
    {
        // Arrange - Test that non-conflicting times in different timezones work
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_tz_non_conflict");
        var (mentee1Arrange, mentee1Act) = await CreateMentee("mentee1_tz_non_conflict");
        var (mentee2Arrange, mentee2Act) = await CreateMentee("mentee2_tz_non_conflict");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "08:00", "18:00");

        var nextMonday = GetNextWeekday(DayOfWeek.Monday);
        var mentorSlug = await GetMentorSlug(mentorArrange);

        // First booking in Japan time (early in mentor's day)
        var firstBooking = new
        {
            MentorSlug = mentorSlug,
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "17:00", // 5 PM JST = ~9 AM CET
            EndTime = "18:00",   // 6 PM JST = ~10 AM CET
            TimeZoneId = "Asia/Tokyo"
        };

        var firstResponse = await mentee1Act.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, firstBooking);
        Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);

        // Non-conflicting booking in US time (later in mentor's day)
        var secondBooking = new
        {
            MentorSlug = mentorSlug,
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "08:00", // 8 AM EST = ~2 PM CET
            EndTime = "09:00",   // 9 AM EST = ~3 PM CET
            TimeZoneId = "America/New_York"
        };

        // Act
        var response = await mentee2Act.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, secondBooking);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    #endregion

    #region International Date Line Tests

    [Fact]
    public async Task BookSession_ShouldHandleInternationalDateLine_EastToWest()
    {
        // Arrange - Test booking across the International Date Line (east to west)
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_dateline_ew");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_dateline_ew");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = GetNextWeekday(DayOfWeek.Monday);
        
        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "14:00",
            EndTime = "15:00",
            TimeZoneId = "Pacific/Auckland" // East of date line
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldHandleInternationalDateLine_WestToEast()
    {
        // Arrange - Test booking across the International Date Line (west to east)
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_dateline_we");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_dateline_we");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = GetNextWeekday(DayOfWeek.Monday);
        
        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "14:00",
            EndTime = "15:00",
            TimeZoneId = "Pacific/Honolulu" // West of date line
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    #endregion

    #region Year Transition Tests

    [Fact]
    public async Task BookSession_ShouldHandleNewYearTransition()
    {
        // Arrange - Test booking during New Year transition
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_new_year");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_new_year");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Tuesday, "09:00", "17:00");

        // Use December 31st (might be January 1st in some timezones)
        var newYearEve = new DateTime(2025, 12, 31);
        
        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = newYearEve.ToString("yyyy-MM-dd"),
            StartTime = "23:00", // 11 PM on Dec 31
            EndTime = "23:59",
            TimeZoneId = "Pacific/Kiritimati" // UTC+14, first to see new year
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    #endregion

    #region Helper Methods

    private async Task SetupMentorAvailability(HttpClient mentorClient, DayOfWeek dayOfWeek, string startTime, string endTime)
    {
        var availabilityRequest = new
        {
            DayAvailabilities = new[]
            {
                new
                {
                    DayOfWeek = dayOfWeek,
                    IsActive = true,
                    AvailabilityRanges = new[]
                    {
                        new { StartTime = startTime, EndTime = endTime }
                    }
                }
            }
        };

        var response = await mentorClient.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, availabilityRequest);
        response.EnsureSuccessStatusCode();
    }

    private async Task<string> GetMentorSlug(HttpClient mentorClient)
    {
        var response = await mentorClient.GetAsync(UsersEndpoints.GetCurrentUser);
        response.EnsureSuccessStatusCode();
        
        var userInfo = await response.Content.ReadFromJsonAsync<JsonElement>();
        return userInfo.GetProperty("slug").GetString()!;
    }

    private static DateTime GetNextWeekday(DayOfWeek dayOfWeek)
    {
        var today = DateTime.UtcNow.Date;
        var daysUntilTarget = ((int)dayOfWeek - (int)today.DayOfWeek + 7) % 7;
        if (daysUntilTarget == 0) daysUntilTarget = 7; // Next week to avoid past dates
        return today.AddDays(daysUntilTarget);
    }

    #endregion
}
