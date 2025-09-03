using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Users.Features;
using IntegrationsTests.Abstractions;
using IntegrationsTests.Abstractions.Base;
using Snapshooter.Xunit;

namespace IntegrationsTests.Tests.Mentorships.Sessions;

public class SessionBookingTests : MentorshipTestBase
{
    public SessionBookingTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    #region Basic Booking Tests

    [Fact]
    public async Task BookSession_ShouldSucceed_WhenValidDataProvided()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_basic", 75.0m, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_basic");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, MentorshipTestUtilities.TimeFormats.Morning9AM, MentorshipTestUtilities.TimeFormats.Afternoon5PM);

        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);
        var mentorSlug = await MentorshipTestUtilities.GetMentorSlug(mentorArrange);
        var bookingRequest = MentorshipTestUtilities.CreateBookingRequest(
            mentorSlug,
            nextMonday.ToString("yyyy-MM-dd"),
            MentorshipTestUtilities.TimeFormats.Morning10AM,
            MentorshipTestUtilities.TimeFormats.Morning11AM,
            MentorshipTestUtilities.TimeZones.Tunisia,
            "First session"
        );

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        await MentorshipTestUtilities.VerifySuccessfulBooking(response);
    }

    [Fact]
    public async Task BookSession_ShouldFail_WhenUnauthenticated()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_unauth");
        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, MentorshipTestUtilities.TimeFormats.Morning9AM, MentorshipTestUtilities.TimeFormats.Afternoon5PM);

        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);
        var mentorSlug = await MentorshipTestUtilities.GetMentorSlug(mentorArrange);
        var bookingRequest = MentorshipTestUtilities.CreateBookingRequest(
            mentorSlug,
            nextMonday.ToString("yyyy-MM-dd"),
            MentorshipTestUtilities.TimeFormats.Morning10AM,
            MentorshipTestUtilities.TimeFormats.Morning11AM,
            MentorshipTestUtilities.TimeZones.Tunisia
        );

        var unauthClient = Factory.CreateClient();

        // Act
        var response = await unauthClient.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldFail_WhenMentorNotFound()
    {
        // Arrange
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_not_found");

        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);
        var bookingRequest = MentorshipTestUtilities.CreateBookingRequest(
            "non-existent-mentor",
            nextMonday.ToString("yyyy-MM-dd"),
            MentorshipTestUtilities.TimeFormats.Morning10AM,
            MentorshipTestUtilities.TimeFormats.Morning11AM,
            MentorshipTestUtilities.TimeZones.Tunisia
        );

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Time Validation Tests

    [Fact]
    public async Task BookSession_ShouldFail_WhenTimeInPast()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_past");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_past");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, MentorshipTestUtilities.TimeFormats.Morning9AM, MentorshipTestUtilities.TimeFormats.Afternoon5PM);

        var pastDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd");
        var mentorSlug = await MentorshipTestUtilities.GetMentorSlug(mentorArrange);
        var bookingRequest = MentorshipTestUtilities.CreateBookingRequest(
            mentorSlug,
            pastDate,
            MentorshipTestUtilities.TimeFormats.Morning10AM,
            MentorshipTestUtilities.TimeFormats.Morning11AM,
            MentorshipTestUtilities.TimeZones.Tunisia
        );

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldFail_WhenEndTimeBeforeStartTime()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_invalid_time");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_invalid_time");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);
        var bookingRequest = new
        {
            MentorSlug = await MentorshipTestUtilities.GetMentorSlug(mentorArrange),
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "11:00",
            EndTime = "10:00", // End before start
            TimeZoneId = "Africa/Tunis"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldFail_WhenInvalidTimeFormat()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_format");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_format");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);
        var bookingRequest = new
        {
            MentorSlug = await MentorshipTestUtilities.GetMentorSlug(mentorArrange),
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "25:00", // Invalid time
            EndTime = "26:00",   // Invalid time
            TimeZoneId = "Africa/Tunis"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldFail_WhenInvalidDateFormat()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_date_format");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_date_format");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var bookingRequest = new
        {
            MentorSlug = await MentorshipTestUtilities.GetMentorSlug(mentorArrange),
            Date = "2025/01/15", // Wrong format
            StartTime = "10:00",
            EndTime = "11:00",
            TimeZoneId = "Africa/Tunis"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Availability Tests

    [Fact]
    public async Task BookSession_ShouldFail_WhenMentorNotAvailable()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_unavailable");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_unavailable");

        // Set availability for Tuesday only
        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Tuesday, "09:00", "17:00");

        // Try to book on Monday (not available)
        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);
        var bookingRequest = new
        {
            MentorSlug = await MentorshipTestUtilities.GetMentorSlug(mentorArrange),
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "10:00",
            EndTime = "11:00",
            TimeZoneId = "Africa/Tunis"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldFail_WhenOutsideAvailableHours()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_outside_hours");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_outside_hours");

        // Set availability 9-17
        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);
        var bookingRequest = new
        {
            MentorSlug = await MentorshipTestUtilities.GetMentorSlug(mentorArrange),
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "18:00", // Outside available hours
            EndTime = "19:00",
            TimeZoneId = "Africa/Tunis"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Conflict Tests

    [Fact]
    public async Task BookSession_ShouldFail_WhenSessionConflicts()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_conflict");
        var (mentee1Arrange, mentee1Act) = await CreateMentee("mentee1_conflict");
        var (mentee2Arrange, mentee2Act) = await CreateMentee("mentee2_conflict");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);
        var mentorSlug = await MentorshipTestUtilities.GetMentorSlug(mentorArrange);

        // First booking
        var firstBooking = new
        {
            MentorSlug = mentorSlug,
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "10:00",
            EndTime = "11:00",
            TimeZoneId = "Africa/Tunis"
        };

        var firstResponse = await mentee1Act.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, firstBooking);
        Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);

        // Conflicting booking (overlapping time)
        var conflictingBooking = new
        {
            MentorSlug = mentorSlug,
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "10:30", // Overlaps with previous session
            EndTime = "11:30",
            TimeZoneId = "Africa/Tunis"
        };

        // Act
        var response = await mentee2Act.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, conflictingBooking);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldSucceed_WhenBackToBackWithBufferTime()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_buffer", 75.0m, 15); // 15 min buffer
        var (mentee1Arrange, mentee1Act) = await CreateMentee("mentee1_buffer");
        var (mentee2Arrange, mentee2Act) = await CreateMentee("mentee2_buffer");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);
        var mentorSlug = await MentorshipTestUtilities.GetMentorSlug(mentorArrange);

        // First booking
        var firstBooking = new
        {
            MentorSlug = mentorSlug,
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "10:00",
            EndTime = "11:00",
            TimeZoneId = "Africa/Tunis"
        };

        var firstResponse = await mentee1Act.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, firstBooking);
        Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);

        // Second booking respecting buffer time (15 minutes after first session ends)
        var secondBooking = new
        {
            MentorSlug = mentorSlug,
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "11:15", // 15 minutes buffer
            EndTime = "12:15",
            TimeZoneId = "Africa/Tunis"
        };

        // Act
        var response = await mentee2Act.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, secondBooking);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    #endregion

    #region Timezone Tests
    
    [Fact]
    public async Task BookSession_ShouldSucceed_WithAsianTimezone()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_asia_tz");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_asia_tz");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);
        var bookingRequest = new
        {
            MentorSlug = await MentorshipTestUtilities.GetMentorSlug(mentorArrange),
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "17:00", // 5 PM tokyo : equivalent to 9 AM Tunisia 
            EndTime = "23:00",
            TimeZoneId = "Asia/Tokyo"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldHandleTimezoneConversionCorrectly()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_conversion");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_conversion");

        // Mentor available 14:00-18:00 in Africa/Tunis
        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "14:00", "18:00");

        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);
        
        // Mentee books 14:00-15:00 in UTC (which should be 15:00-16:00 in Africa/Tunis)
        var bookingRequest = new
        {
            MentorSlug = await MentorshipTestUtilities.GetMentorSlug(mentorArrange),
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "14:00", // 2 PM UTC = 3 PM Africa/Tunis
            EndTime = "15:00",   // 3 PM UTC = 4 PM Africa/Tunis
            TimeZoneId = "UTC"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    /*
    TODO :  
    [Fact]
    public async Task BookSession_ShouldFail_WithInvalidTimezone()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_invalid_tz");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_invalid_tz");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);
        var bookingRequest = new
        {
            MentorSlug = await MentorshipTestUtilities.GetMentorSlug(mentorArrange),
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "10:00",
            EndTime = "11:00",
            TimeZoneId = "Invalid/Timezone"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }*/

    #endregion

    #region Edge Cases and Special Scenarios

    [Fact]
    public async Task BookSession_ShouldSucceed_WithMinimumDuration()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_min_duration");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_min_duration");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);
        var bookingRequest = new
        {
            MentorSlug = await MentorshipTestUtilities.GetMentorSlug(mentorArrange),
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "10:00",
            EndTime = "10:15", // 15 minutes - minimum duration
            TimeZoneId = "Africa/Tunis"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldSucceed_WithMaximumDuration()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_max_duration");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_max_duration");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);
        var bookingRequest = new
        {
            MentorSlug = await MentorshipTestUtilities.GetMentorSlug(mentorArrange),
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "09:00",
            EndTime = "17:00", // 8 hours - maximum session
            TimeZoneId = "Africa/Tunis"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldSucceed_WithOptionalNote()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_note");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_note");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);
        var bookingRequest = new
        {
            MentorSlug = await MentorshipTestUtilities.GetMentorSlug(mentorArrange),
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "10:00",
            EndTime = "11:00",
            TimeZoneId = "Africa/Tunis",
            Note = "This is a detailed note about the session goals and expectations."
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldSucceed_WithoutOptionalNote()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_no_note");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_no_note");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);
        var bookingRequest = new
        {
            MentorSlug = await MentorshipTestUtilities.GetMentorSlug(mentorArrange),
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "10:00",
            EndTime = "11:00",
            TimeZoneId = "Africa/Tunis"
            // Note intentionally omitted
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldSucceed_OnWeekend()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_weekend");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_weekend");

        // Set availability for Saturday
        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Saturday, "10:00", "16:00");

        var nextSaturday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Saturday);
        var bookingRequest = new
        {
            MentorSlug = await MentorshipTestUtilities.GetMentorSlug(mentorArrange),
            Date = nextSaturday.ToString("yyyy-MM-dd"),
            StartTime = "11:00",
            EndTime = "12:00",
            TimeZoneId = "Africa/Tunis"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldHandleCrossDayTimezone_Correctly()
    {
        // Arrange - test when booking crosses date boundary due to timezone
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_cross_day");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_cross_day");

        // Mentor available Monday in Africa/Tunis
        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "02:00", "06:00");

        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);
        
        // Mentee books in Pacific timezone (Sunday evening = Monday early morning in Tunis)
        var bookingRequest = new
        {
            MentorSlug = await MentorshipTestUtilities.GetMentorSlug(mentorArrange),
            Date = nextMonday.AddDays(-1).ToString("yyyy-MM-dd"), // Sunday in Pacific = Monday in Tunis
            StartTime = "21:00", // 9 PM PST Sunday = ~2 AM Monday in Tunis
            EndTime = "22:00",
            TimeZoneId = "America/Los_Angeles"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BookSession_ShouldSucceed_MultipleConsecutiveDays()
    {
        // Arrange - test booking sessions on consecutive days
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_consecutive");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_consecutive");

        // Set availability for Monday and Tuesday
        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "10:00", "16:00");
        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Tuesday, "10:00", "16:00");

        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);
        var nextTuesday = nextMonday.AddDays(1);
        var mentorSlug = await MentorshipTestUtilities.GetMentorSlug(mentorArrange);

        // Book Monday session
        var mondayBooking = new
        {
            MentorSlug = mentorSlug,
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "11:00",
            EndTime = "12:00",
            TimeZoneId = "Africa/Tunis"
        };

        var mondayResponse = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, mondayBooking);
        Assert.Equal(HttpStatusCode.OK, mondayResponse.StatusCode);

        // Book Tuesday session
        var tuesdayBooking = new
        {
            MentorSlug = mentorSlug,
            Date = nextTuesday.ToString("yyyy-MM-dd"),
            StartTime = "11:00",
            EndTime = "12:00",
            TimeZoneId = "Africa/Tunis"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, tuesdayBooking);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    /*
    TODO : 
    [Fact]
    public async Task BookSession_ShouldSucceed_DuringDaylightSavingTransition()
    {
        // Arrange - test booking during DST transition period
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_dst");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_dst");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        // Use a date during DST transition (last Sunday of March for Europe)
        var dstTransitionDate = new DateTime(2025, 3, 30); // Typical DST transition date
        var bookingRequest = new
        {
            MentorSlug = await MentorshipTestUtilities.GetMentorSlug(mentorArrange),
            Date = dstTransitionDate.ToString("yyyy-MM-dd"),
            StartTime = "10:00",
            EndTime = "11:00",
            TimeZoneId = "Europe/Paris" // Timezone that observes DST
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    */

    #endregion

}
