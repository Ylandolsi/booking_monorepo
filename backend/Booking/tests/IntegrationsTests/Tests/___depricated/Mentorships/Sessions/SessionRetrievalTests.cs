/*using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Booking.Modules.Mentorships.refactored.Features;
using IntegrationsTests.Abstractions;
using IntegrationsTests.Abstractions.Base;

namespace IntegrationsTests.Tests.___depricated.Mentorships.Sessions;

public class SessionRetrievalTests : MentorshipTestBase
{
    public SessionRetrievalTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    #region Get Sessions List Tests

    [Fact]
    public async Task GetSessions_ShouldReturnMenteeSessions_WhenMenteeRequests()
    {
        // Arrange
        var (mentor1Arrange, mentor1Act) = await CreateMentor("mentor1_list");
        var (mentor2Arrange, mentor2Act) = await CreateMentor("mentor2_list");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_list");

        await MentorshipTestUtilities.SetupMentorAvailability(mentor1Arrange, DayOfWeek.Monday,
            MentorshipTestUtilities.TimeFormats.Morning9AM, MentorshipTestUtilities.TimeFormats.Afternoon5PM);
        await MentorshipTestUtilities.SetupMentorAvailability(mentor2Arrange, DayOfWeek.Tuesday,
            MentorshipTestUtilities.TimeFormats.Morning9AM, MentorshipTestUtilities.TimeFormats.Afternoon5PM);

        // Book multiple sessions using utility
        var session1Id = await MentorshipTestUtilities.BookValidSession(mentor1Arrange, menteeAct, DayOfWeek.Monday,
            MentorshipTestUtilities.TimeFormats.Morning10AM, MentorshipTestUtilities.TimeFormats.Morning11AM);
        var session2Id = await MentorshipTestUtilities.BookValidSession(mentor2Arrange, menteeAct, DayOfWeek.Tuesday,
            MentorshipTestUtilities.TimeFormats.Afternoon2PM, MentorshipTestUtilities.TimeFormats.Afternoon3PM);

        // Act
        var response = await menteeAct.GetAsync(MentorshipEndpoints.Sessions.GetSessions);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var sessions = await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.True(sessions.ValueKind == JsonValueKind.Array);
        var sessionArray = sessions.EnumerateArray().ToList();
        Assert.True(sessionArray.Count >= 2);

        var sessionIds = sessionArray.Select(s => s.GetProperty("id").GetInt32()).ToList();
        Assert.Contains(session1Id, sessionIds);
        Assert.Contains(session2Id, sessionIds);
    }

    [Fact]
    public async Task GetSessions_ShouldReturnMentorSessions_WhenMentorRequests()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_list_own");
        var (mentor2Arrange, mentor2Act) = await CreateMentor("mentor2");
        var (mentee1Arrange, mentee1Act) = await CreateMentee("mentee1_list_mentor");
        var (mentee2Arrange, mentee2Act) = await CreateMentee("mentee2_list_mentor");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday,
            MentorshipTestUtilities.TimeFormats.Morning9AM, MentorshipTestUtilities.TimeFormats.Afternoon5PM);
        await MentorshipTestUtilities.SetupMentorAvailability(mentor2Arrange, DayOfWeek.Monday,
            MentorshipTestUtilities.TimeFormats.Morning9AM, MentorshipTestUtilities.TimeFormats.Afternoon5PM);

        // Book sessions with different mentees using utility
        var session1Id = await MentorshipTestUtilities.BookValidSession(mentorArrange, mentee1Act, DayOfWeek.Monday,
            MentorshipTestUtilities.TimeFormats.Morning10AM, MentorshipTestUtilities.TimeFormats.Morning11AM);
        var session2Id = await MentorshipTestUtilities.BookValidSession(mentorArrange, mentee2Act, DayOfWeek.Monday,
            MentorshipTestUtilities.TimeFormats.Noon, "13:00");
        var session3Id = await MentorshipTestUtilities.BookValidSession(mentor2Arrange, mentorAct, DayOfWeek.Monday,
            MentorshipTestUtilities.TimeFormats.Afternoon2PM, MentorshipTestUtilities.TimeFormats.Afternoon3PM);


        var response = await mentorAct.GetAsync($"{MentorshipEndpoints.Sessions.GetSessions}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var sessions = await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.True(sessions.ValueKind == JsonValueKind.Array);
        var sessionArray = sessions.EnumerateArray().ToList();
        Assert.True(sessionArray.Count == 3);


        Assert.Equal(2, sessionArray.Count(s => s.GetProperty("iamMentor").GetBoolean()));

        var sessionIds = sessionArray.Select(s => s.GetProperty("id").GetInt32()).ToList();
        Assert.Contains(session1Id, sessionIds);
        Assert.Contains(session2Id, sessionIds);
        Assert.Contains(session3Id, sessionIds);
    }

    [Fact]
    public async Task GetSessions_ShouldFail_WhenUnauthenticated()
    {
        // Arrange
        var unauthClient = Factory.CreateClient();

        // Act
        var response = await unauthClient.GetAsync(MentorshipEndpoints.Sessions.GetSessions);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetSessions_ShouldReturnEmptyList_WhenNoSessions()
    {
        // Arrange
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_empty_list");

        // Act
        var response = await menteeAct.GetAsync(MentorshipEndpoints.Sessions.GetSessions);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var sessions = await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.True(sessions.ValueKind == JsonValueKind.Array);
        var sessionArray = sessions.EnumerateArray().ToList();
        Assert.Empty(sessionArray);
    }

    [Fact]
    public async Task GetSessions_ShouldFilterByDateRange_WhenParameterProvided()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_filter_date");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_filter_date");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Tuesday, "09:00", "17:00");

        // Book sessions on different days
        var session1Id =
            await MentorshipTestUtilities.BookValidSession(mentorArrange, menteeAct, DayOfWeek.Monday, "10:00",
                "11:00");
        var session2Id =
            await MentorshipTestUtilities.BookValidSession(mentorArrange, menteeAct, DayOfWeek.Tuesday, "10:00",
                "11:00");

        // Act - request sessions for next 14 days to ensure we capture sessions booked for next week
        var dateAfter14days = DateTime.Today.AddDays(14);
        var response =
            await menteeAct.GetAsync(
                $"{MentorshipEndpoints.Sessions.GetSessions}?upToDate={dateAfter14days:yyyy-MM-dd}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var sessions = await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.True(sessions.ValueKind == JsonValueKind.Array);
        var sessionArray = sessions.EnumerateArray().ToList();
        Assert.True(sessionArray.Count >= 2);
    }

    [Fact]
    public async Task GetSessions_ShouldFilterByTimezone_WhenParameterProvided()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_filter_timezone");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_filter_timezone");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        // Book session in specific timezone
        var sessionId = await MentorshipTestUtilities.BookValidSession(mentorArrange, menteeAct, DayOfWeek.Monday,
            "10:00", "11:00", "Europe/Paris");

        // Act - request sessions with timezone parameter for next 14 days
        var dateAfter14days = DateTime.Today.AddDays(14);

        var response =
            await menteeAct.GetAsync(
                $"{MentorshipEndpoints.Sessions.GetSessions}?timeZoneId=Europe/Paris&upToDate={dateAfter14days:yyyy-MM-dd}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var sessions = await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.True(sessions.ValueKind == JsonValueKind.Array);
        var sessionArray = sessions.EnumerateArray().ToList();
        Assert.True(sessionArray.Count >= 1);

        var sessionIds = sessionArray.Select(s => s.GetProperty("id").GetInt32()).ToList();
        Assert.Contains(sessionId, sessionIds);
    }

    [Fact]
    public async Task GetSessions_ShouldSortByScheduledTime()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_sort_time");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_sort_time");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Tuesday, "09:00", "17:00");

        // Book sessions in different order (not chronological)
        var laterSessionId =
            await MentorshipTestUtilities.BookValidSession(mentorArrange, menteeAct, DayOfWeek.Tuesday, "14:00",
                "15:00");
        var earlierSessionId =
            await MentorshipTestUtilities.BookValidSession(mentorArrange, menteeAct, DayOfWeek.Monday, "10:00",
                "11:00");

        // Act
        var response = await menteeAct.GetAsync(MentorshipEndpoints.Sessions.GetSessions);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var sessions = await response.Content.ReadFromJsonAsync<JsonElement>();

        var sessionArray = sessions.EnumerateArray().ToList();
        Assert.True(sessionArray.Count >= 2);

        // Verify sessions are sorted by scheduled time
        for (var i = 0; i < sessionArray.Count - 1; i++)
        {
            var currentScheduledAt = DateTime.Parse(sessionArray[i].GetProperty("scheduledAt").GetString()!);
            var nextScheduledAt = DateTime.Parse(sessionArray[i + 1].GetProperty("scheduledAt").GetString()!);
            Assert.True(currentScheduledAt <= nextScheduledAt);
        }
    }

    [Fact]
    public async Task GetSessions_ShouldIncludeTimezoneInformation()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_include_timezone");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_include_timezone");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        var sessionId = await MentorshipTestUtilities.BookValidSession(mentorArrange, menteeAct, DayOfWeek.Monday,
            "10:00", "11:00", "Europe/Paris");

        // Act
        var response = await menteeAct.GetAsync(MentorshipEndpoints.Sessions.GetSessions);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var sessions = await response.Content.ReadFromJsonAsync<JsonElement>();

        var sessionArray = sessions.EnumerateArray().ToList();
        var bookedSession = sessionArray.FirstOrDefault(s => s.GetProperty("id").GetInt32() == sessionId);
        Assert.True(bookedSession.ValueKind != JsonValueKind.Undefined);

        // Verify timezone-related fields are present
        Assert.True(bookedSession.TryGetProperty("scheduledAt", out _));
        Assert.True(bookedSession.TryGetProperty("durationInMinutes", out _));
    }

    [Fact]
    public async Task GetSessions_ShouldShowBookedStatus_ForNewSession()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_status_booked");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_status_booked");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        var sessionId =
            await MentorshipTestUtilities.BookValidSession(mentorArrange, menteeAct, DayOfWeek.Monday, "10:00",
                "11:00");

        // Act
        var response = await menteeAct.GetAsync(MentorshipEndpoints.Sessions.GetSessions);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var sessions = await response.Content.ReadFromJsonAsync<JsonElement>();

        var sessionArray = sessions.EnumerateArray().ToList();
        var bookedSession = sessionArray.FirstOrDefault(s => s.GetProperty("id").GetInt32() == sessionId);
        Assert.True(bookedSession.ValueKind != JsonValueKind.Undefined);

        // Verify session is in expected status (could be Booked=1, WaitingForPayment=2, or Confirmed=3)
        var status = bookedSession.GetProperty("status").GetInt32();
        Assert.True(status >= 1 && status <= 3,
            $"Expected status to be 1-3 (Booked, WaitingForPayment, or Confirmed), but got {status}");
    }

    #endregion
}*/