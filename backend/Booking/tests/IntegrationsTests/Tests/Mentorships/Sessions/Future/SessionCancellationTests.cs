/*
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Users.Features;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships.Sessions;

public class SessionCancellationTests : MentorshipTestBase
{
    public SessionCancellationTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    #region Basic Cancellation Tests

    [Fact]
    public async Task CancelSession_ShouldSucceed_WhenValidSessionAndAuthenticatedMentee()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_cancel_valid");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_cancel_valid");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        var sessionId = await BookValidSession(mentorArrange, menteeAct, DayOfWeek.Monday, "10:00", "11:00");

        var cancelRequest = new
        {
            CancellationReason = "Unable to attend due to emergency"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(
            MentorshipEndpoints.Sessions.Cancel.Replace("{sessionId}", sessionId.ToString()), 
            cancelRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CancelSession_ShouldSucceed_WhenValidSessionAndAuthenticatedMentor()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_cancel_self");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_cancel_mentor");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        var sessionId = await BookValidSession(mentorArrange, menteeAct, DayOfWeek.Monday, "10:00", "11:00");

        var cancelRequest = new
        {
            CancellationReason = "Schedule conflict arose"
        };

        // Act
        var response = await mentorAct.PostAsJsonAsync(
            MentorshipEndpoints.Sessions.Cancel.Replace("{sessionId}", sessionId.ToString()), 
            cancelRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CancelSession_ShouldFail_WhenUnauthenticated()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_cancel_unauth");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_cancel_unauth");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        var sessionId = await BookValidSession(mentorArrange, menteeAct, DayOfWeek.Monday, "10:00", "11:00");

        var cancelRequest = new
        {
            CancellationReason = "Test cancellation"
        };

        var unauthClient = Factory.CreateClient();

        // Act
        var response = await unauthClient.PostAsJsonAsync(
            MentorshipEndpoints.Sessions.Cancel.Replace("{sessionId}", sessionId.ToString()), 
            cancelRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CancelSession_ShouldFail_WhenSessionNotFound()
    {
        // Arrange
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_cancel_not_found");

        var cancelRequest = new
        {
            CancellationReason = "Test cancellation"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(
            MentorshipEndpoints.Sessions.Cancel.Replace("{sessionId}", "999999"), 
            cancelRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CancelSession_ShouldFail_WhenUserNotAuthorized()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_cancel_other");
        var (mentee1Arrange, mentee1Act) = await CreateMentee("mentee1_cancel_other");
        var (mentee2Arrange, mentee2Act) = await CreateMentee("mentee2_cancel_other");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        var sessionId = await BookValidSession(mentorArrange, mentee1Act, DayOfWeek.Monday, "10:00", "11:00");

        var cancelRequest = new
        {
            CancellationReason = "Unauthorized cancellation attempt"
        };

        // Act - mentee2 tries to cancel mentee1's session
        var response = await mentee2Act.PostAsJsonAsync(
            MentorshipEndpoints.Sessions.Cancel.Replace("{sessionId}", sessionId.ToString()), 
            cancelRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Time-based Cancellation Tests

    [Fact]
    public async Task CancelSession_ShouldFail_WhenTooCloseToSessionTime()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_cancel_late");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_cancel_late");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        
        // Book a session very close to current time (should be within 2-hour cancellation window)
        var nearFutureDate = DateTime.UtcNow.AddHours(1); // 1 hour from now
        var sessionId = await BookSessionAtSpecificTime(mentorArrange, menteeAct, nearFutureDate);

        var cancelRequest = new
        {
            CancellationReason = "Too late to cancel"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(
            MentorshipEndpoints.Sessions.Cancel.Replace("{sessionId}", sessionId.ToString()), 
            cancelRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CancelSession_ShouldSucceed_WhenWellInAdvance()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_cancel_advance");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_cancel_advance");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        
        // Book a session well in advance (more than 2 hours)
        var futureDate = DateTime.UtcNow.AddDays(7); // 1 week from now
        var sessionId = await BookSessionAtSpecificTime(mentorArrange, menteeAct, futureDate);

        var cancelRequest = new
        {
            CancellationReason = "Planning conflict"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(
            MentorshipEndpoints.Sessions.Cancel.Replace("{sessionId}", sessionId.ToString()), 
            cancelRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    #endregion

    #region Session Status Tests

    [Fact]
    public async Task CancelSession_ShouldFail_WhenSessionAlreadyCancelled()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_cancel_already");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_cancel_already");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        var sessionId = await BookValidSession(mentorArrange, menteeAct, DayOfWeek.Monday, "10:00", "11:00");

        var cancelRequest = new
        {
            CancellationReason = "First cancellation"
        };

        // First cancellation
        var firstResponse = await menteeAct.PostAsJsonAsync(
            MentorshipEndpoints.Sessions.Cancel.Replace("{sessionId}", sessionId.ToString()), 
            cancelRequest);
        Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);

        // Second cancellation attempt
        var secondCancelRequest = new
        {
            CancellationReason = "Second cancellation attempt"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(
            MentorshipEndpoints.Sessions.Cancel.Replace("{sessionId}", sessionId.ToString()), 
            secondCancelRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Timezone-specific Cancellation Tests

    [Fact]
    public async Task CancelSession_ShouldHandleTimezoneBasedCancellationWindow()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_cancel_tz");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_cancel_tz");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        
        // Book session in a different timezone where the 2-hour window might be interpreted differently
        var sessionTime = DateTime.UtcNow.AddHours(3); // 3 hours from now in UTC
        var sessionId = await BookSessionAtSpecificTime(mentorArrange, menteeAct, sessionTime, "Europe/Paris");

        var cancelRequest = new
        {
            CancellationReason = "Timezone-based cancellation test"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(
            MentorshipEndpoints.Sessions.Cancel.Replace("{sessionId}", sessionId.ToString()), 
            cancelRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CancelSession_ShouldSucceed_WithDifferentTimezoneUsers()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_cancel_tz_diff");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_cancel_tz_diff");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        
        // Book session with mentee in different timezone
        var sessionId = await BookValidSession(mentorArrange, menteeAct, DayOfWeek.Monday, "15:00", "16:00", "America/New_York");

        var cancelRequest = new
        {
            CancellationReason = "Cross-timezone cancellation"
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(
            MentorshipEndpoints.Sessions.Cancel.Replace("{sessionId}", sessionId.ToString()), 
            cancelRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    #endregion

    #region Cancellation Reason Tests

    [Fact]
    public async Task CancelSession_ShouldSucceed_WithDetailedReason()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_cancel_detailed");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_cancel_detailed");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        var sessionId = await BookValidSession(mentorArrange, menteeAct, DayOfWeek.Monday, "10:00", "11:00");

        var cancelRequest = new
        {
            CancellationReason = "Due to an unexpected family emergency that requires immediate attention, I will not be able to attend the scheduled mentoring session. I apologize for any inconvenience this may cause and would like to reschedule at the earliest convenient time for both parties."
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(
            MentorshipEndpoints.Sessions.Cancel.Replace("{sessionId}", sessionId.ToString()), 
            cancelRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CancelSession_ShouldSucceed_WithoutReason()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_cancel_no_reason");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_cancel_no_reason");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        var sessionId = await BookValidSession(mentorArrange, menteeAct, DayOfWeek.Monday, "10:00", "11:00");

        var cancelRequest = new
        {
            CancellationReason = (string?)null
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(
            MentorshipEndpoints.Sessions.Cancel.Replace("{sessionId}", sessionId.ToString()), 
            cancelRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CancelSession_ShouldSucceed_WithEmptyReason()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_cancel_empty");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_cancel_empty");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        var sessionId = await BookValidSession(mentorArrange, menteeAct, DayOfWeek.Monday, "10:00", "11:00");

        var cancelRequest = new
        {
            CancellationReason = ""
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(
            MentorshipEndpoints.Sessions.Cancel.Replace("{sessionId}", sessionId.ToString()), 
            cancelRequest);

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

    private async Task<int> BookValidSession(HttpClient mentorClient, HttpClient menteeClient, DayOfWeek dayOfWeek, 
        string startTime, string endTime, string timeZoneId = "Africa/Tunis")
    {
        var targetDate = GetNextWeekday(dayOfWeek);
        var mentorSlug = await GetMentorSlug(mentorClient);

        var bookingRequest = new
        {
            MentorSlug = mentorSlug,
            Date = targetDate.ToString("yyyy-MM-dd"),
            StartTime = startTime,
            EndTime = endTime,
            TimeZoneId = timeZoneId,
            Note = "Test session for cancellation"
        };

        var response = await menteeClient.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        return result.GetProperty("sessionId").GetInt32();
    }

    private async Task<int> BookSessionAtSpecificTime(HttpClient mentorClient, HttpClient menteeClient, 
        DateTime specificDateTime, string timeZoneId = "Africa/Tunis")
    {
        var mentorSlug = await GetMentorSlug(mentorClient);

        var bookingRequest = new
        {
            MentorSlug = mentorSlug,
            Date = specificDateTime.ToString("yyyy-MM-dd"),
            StartTime = specificDateTime.ToString("HH:mm"),
            EndTime = specificDateTime.AddHours(1).ToString("HH:mm"),
            TimeZoneId = timeZoneId,
            Note = "Test session at specific time"
        };

        var response = await menteeClient.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        return result.GetProperty("sessionId").GetInt32();
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
*/
